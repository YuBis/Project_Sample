using UnityEngine;
using System.Collections.Generic;
using SimpleJSON;
using System;
using static Spine.Unity.Examples.MixAndMatchSkinsExample;
using System.Linq;

public enum TeamType
{ 
    ALLY,
    ENEMY_1,
}

public enum CharacterState
{
    NORMAL,
    SPAWNING,
    DIE
}

public enum SearchType
{
    SIGHT,
    AREA,
}


public class CharacterManager : BaseManager<CharacterManager>
{
    public Dictionary<TeamType, List<GameCharacter>> m_dicGameCharacterList = new();

    public override void InitData()
    {
        ClearAllCharacter();
    }

    public void ClearAllCharacter()
    {
        m_dicGameCharacterList.Clear();
    }

    public void MakeCharacter(string characterKey, TeamType teamType, Action<GameObject> callBack = null)
    {
        var characterTemplate = CharacterTemplateManager.Instance.GetData(characterKey);
        if (characterTemplate == null)
            return;

        ObjectPoolManager.Instance.GetObject(characterTemplate.PREFAB, (go) =>
        {
            _CallbackMakeCharacter(characterKey, go, teamType, callBack);
        });
    }

    void _CallbackMakeCharacter(string characterKey, GameObject character, TeamType teamType, Action<GameObject> finalCallback)
    {
        List<GameCharacter> list = null;
        if (!m_dicGameCharacterList.TryGetValue(teamType, out list))
            list = new();

        var gameCharacter = character.GetComponent<GameCharacter>();
        gameCharacter.Initialize(characterKey, teamType);
        if (gameCharacter.CHARACTER_DATA == null)
            return;

        list.Add(gameCharacter);

        gameCharacter.AI?.AddNextAI(AIStateType.SPAWN);

        m_dicGameCharacterList[teamType] = list;

        finalCallback?.Invoke(character);
    }

    public GameCharacter GetCharacter(TeamType teamType, string characterKey)
    {
        if (!m_dicGameCharacterList.TryGetValue(teamType, out var list))
            return null;

        return list.FirstOrDefault(c => c.TEMPLATE_KEY == characterKey);
    }

    public void RemoveCharacter(GameCharacter gameCharacter)
    {
        if (gameCharacter.CHARACTER_STATE != CharacterState.DIE)
            gameCharacter.OnDeath();

        if (m_dicGameCharacterList.TryGetValue(gameCharacter.TEAM_TYPE, out var list))
        {
            list.Remove(gameCharacter);
            m_dicGameCharacterList[gameCharacter.TEAM_TYPE] = list;
        }

        ObjectPoolManager.Instance.ReleaseObject(gameCharacter.GAMEOBJECT);
    }

    public GameCharacter GetEnemyInSight(GameCharacter myCharacter)
    {
        if (myCharacter == null || myCharacter.TRANSFORM == null)
            return null;

        var teamType = myCharacter.TEAM_TYPE;
        var myPos = myCharacter.TRANSFORM.position;
        var sight = myCharacter.STAT_DATA.GetStat(StatType.SIGHT);

        GameCharacter nearCharacter = null;
        double lastDist = 10000;

        var enumerator = m_dicGameCharacterList.GetEnumerator();
        while (enumerator.MoveNext())
        {
            if (enumerator.Current.Key == teamType)
                continue;

            var characterList = enumerator.Current.Value;
            foreach(var targetCharacter in characterList)
            {
                if (targetCharacter == null)
                    continue;

                if (targetCharacter.CHARACTER_STATE != CharacterState.NORMAL)
                    continue;

                var targetPos = targetCharacter.TRANSFORM.position;

                var r = sight * targetCharacter.TRANSFORM.localScale.x / 2;
                var range = Vector3.Distance(myPos, targetPos);

                if (range > r)
                    continue;

                if (range < lastDist)
                {
                    lastDist = range;
                    nearCharacter = targetCharacter;
                }
            }
            
        }

        return nearCharacter;
    }

    public List<GameCharacter> GetEnemiesInRange(GameCharacter myCharacter, Rect rangeRect, int target = 1)
    {
        if (myCharacter == null || myCharacter.TRANSFORM == null || myCharacter.CHARACTER_STATE == CharacterState.DIE)
            return null;

        var teamType = myCharacter.TEAM_TYPE;
        var myPos = myCharacter.TRANSFORM.position;

        Dictionary<float, GameCharacter> dicEnemies = new();

        rangeRect.position += new Vector2(myCharacter.TRANSFORM.position.x, myCharacter.TRANSFORM.position.y);

        bool flip = myCharacter.TRANSFORM.localRotation.y != 0;
        if(flip)
        {
            rangeRect.x -= rangeRect.width;
        }

        var enumerator = m_dicGameCharacterList.GetEnumerator();
        while (enumerator.MoveNext())
        {
            if (enumerator.Current.Key == teamType)
                continue;

            var characterList = enumerator.Current.Value;
            foreach (var targetCharacter in characterList)
            {
                if (targetCharacter == null)
                    continue;

                if (targetCharacter.CHARACTER_STATE != CharacterState.NORMAL)
                    continue;

                var targetPos = targetCharacter.TRANSFORM.position;

                if (!rangeRect.Contains(targetPos))
                    continue;

                var range = Vector3.Distance(myPos, targetPos);

                dicEnemies.Add(range, targetCharacter);
            }

        }

        var targetEnemies = dicEnemies.OrderBy(t => t.Key).Select(t => t.Value).ToList();
        if (targetEnemies.Count <= target)
            return targetEnemies;

        return targetEnemies.GetRange(0, target);
    }
}