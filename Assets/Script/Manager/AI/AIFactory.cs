using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIType
{ 
    NONE,
    PLAYER,
    AGGRESSIVE,
    NON_AGGRESSIVE,
}

public enum AIStateType
{
    NONE,
    SPAWN,
    IDLE,
    MOVE,
    PATROL,
    TARGETING,
    ATTACK,
    HIT,
    DIE,

    Count,
}

public delegate BaseAI AIFactoryDelegate(GameCharacter parent, AIType type);

public class AIFactory : BaseManager<AIFactory>
{
    Dictionary<AIType, AIFactoryDelegate> m_dicAIFactoryDelegate = new();

    public override void InitData()
    {
        m_dicAIFactoryDelegate[AIType.PLAYER] = _CreatePlayerAI;
        m_dicAIFactoryDelegate[AIType.AGGRESSIVE] = _CreateAggressiveAI;
        m_dicAIFactoryDelegate[AIType.NON_AGGRESSIVE] = _CreateNonAggressiveAI;
    }

    BaseAI _CreatePlayerAI(GameCharacter parent, AIType type) => parent.GAMEOBJECT.AddComponent<PlayerAI>();
    BaseAI _CreateAggressiveAI(GameCharacter parent, AIType type) => parent.GAMEOBJECT.AddComponent<AggressiveAI>();
    BaseAI _CreateNonAggressiveAI(GameCharacter parent, AIType type) => parent.GAMEOBJECT.AddComponent<NonAggressiveAI>();

    public BaseAI InjectAI(GameCharacter obj, AIType aiType)
    {
        if (aiType == AIType.NONE)
            return null;

        var ai = m_dicAIFactoryDelegate[aiType].Invoke(obj, aiType);
        ai.GAME_CHARACTER = obj;
        ai.SPAWN_POS = obj.TRANSFORM.position;

        return ai;
    }
}