using Com.LuisPedroFonseca.ProCamera2D;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class MobSpawner : BaseObject
{
    [SerializeField]
    Transform m_spawnPoint = null;

    [SerializeField]
    TeamType m_teamType = TeamType.ENEMY_1;

    [SerializeField]
    string m_characterKey = null;

    GameCharacter m_targetCharacter = null;

    readonly float m_spawnCheckTime = 10.0f;
    double m_lastSpawnCheckedTick = 0;

    bool m_bSpawning = false;

    private void OnEnable()
    {
        m_bSpawning = false;
        StartCoroutine(SpawnChecker());
    }

    public void MakeMob()
    {
        var characterData = CharacterTemplateManager.Instance.GetData(m_characterKey);
        if (characterData == null)
        {
            Universe.LogError(m_characterKey + " : Character template not found!");
            return;
        }

        m_bSpawning = true;
        CharacterManager.Instance.MakeCharacter(characterData.KEY, m_teamType, _CallBackSpawnMob);
    }

    void _CallBackSpawnMob(GameObject obj)
    {
        m_bSpawning = false;
        if (!obj)
            return;

        m_targetCharacter = obj.GetComponent<GameCharacter>();

        m_targetCharacter.TRANSFORM.position = new Vector3(0, 0);
        m_targetCharacter.TRANSFORM.SetParent(m_spawnPoint, false);
    }

    IEnumerator SpawnChecker()
    {
        while(true)
        {
            yield return null;

            if (m_bSpawning)
                continue;

            if(m_lastSpawnCheckedTick + TimeSpan.FromSeconds(m_spawnCheckTime).Ticks < DateTime.Now.Ticks)
            {
                m_lastSpawnCheckedTick = DateTime.Now.Ticks;

                if (m_targetCharacter != null && m_targetCharacter.CHARACTER_STATE != CharacterState.DIE)
                    continue;

                m_targetCharacter = null;

                MakeMob();
            }
        }
    }
}
