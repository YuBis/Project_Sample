using Com.LuisPedroFonseca.ProCamera2D.TopDownShooter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Spine.Unity;
using Spine;

public struct stNextAI
{
    public AIStateType m_nextState;
    public GameCharacter m_targetChar;
    public Vector3 m_targetPos;
    public string m_nextSkill;
}

public class BaseAI : BaseObject
{
    protected AIStateType m_beforeAIState = AIStateType.NONE;
    protected AIStateType m_currentAIState = AIStateType.NONE;

    delegate IEnumerator UpdateFunc();
    delegate void ChangeFunc();

    UpdateFunc[] m_arrUpdateFunc = new UpdateFunc[(int)AIStateType.Count];
    ChangeFunc[] m_arrChangeFunc = new ChangeFunc[(int)AIStateType.Count];

    protected Queue<stNextAI> m_listNextAI = new();
    protected GameCharacter m_target = null;
    protected Vector3 m_targetPos;
    protected double m_aiChangeTicks = 0;
    protected Vector3 m_velocity = Vector3.zero;

    protected SkeletonAnimation m_skeletonAnimation;

    protected SkillInstanceData m_nextSkillData = null;

    public Vector3 SPAWN_POS { get; set; }
    public GameCharacter GAME_CHARACTER { get; set; } = null;

    public bool m_isAttack = false;

    private void Awake()
    {
        m_arrUpdateFunc[(int)AIStateType.SPAWN] = _Spawn;
        m_arrUpdateFunc[(int)AIStateType.IDLE] = _Idle;
        m_arrUpdateFunc[(int)AIStateType.MOVE] = _Move;
        m_arrUpdateFunc[(int)AIStateType.PATROL] = _Patrol;
        m_arrUpdateFunc[(int)AIStateType.TARGETING] = _Targeting;
        m_arrUpdateFunc[(int)AIStateType.ATTACK] = _Attack;
        m_arrUpdateFunc[(int)AIStateType.HIT] = _Hit;
        m_arrUpdateFunc[(int)AIStateType.DIE] = _Die;

        m_arrChangeFunc[(int)AIStateType.SPAWN] = _ToSpawn;
        m_arrChangeFunc[(int)AIStateType.MOVE] = _ToMove;
        m_arrChangeFunc[(int)AIStateType.IDLE] = _ToIdle;
        m_arrChangeFunc[(int)AIStateType.PATROL] = _ToPatrol;
        m_arrChangeFunc[(int)AIStateType.TARGETING] = _ToTargeting;
        m_arrChangeFunc[(int)AIStateType.ATTACK] = _ToAttack;
        m_arrChangeFunc[(int)AIStateType.HIT] = _ToHit;
        m_arrChangeFunc[(int)AIStateType.DIE] = _ToDie;
    }

    private void OnEnable()
    {
        m_listNextAI.Clear();
        m_target = null;
        m_targetPos = new Vector3();
        m_skeletonAnimation = GetComponent<SkeletonAnimation>();
        if(m_skeletonAnimation != null)
        {
            m_skeletonAnimation.AnimationState.Start += _HandleAnimationStateStart;
            m_skeletonAnimation.AnimationState.Event += _HandleAnimationStateEvent;
            m_skeletonAnimation.AnimationState.Complete += _HandleAnimationStateComplete;
        }

        StartCoroutine(_AILoop());
    }

    private void OnDisable()
    {
        if (m_skeletonAnimation != null)
        {
            m_skeletonAnimation.AnimationState.Start -= _HandleAnimationStateStart;
            m_skeletonAnimation.AnimationState.Event -= _HandleAnimationStateEvent;
            m_skeletonAnimation.AnimationState.Complete -= _HandleAnimationStateComplete;
        }
    }

    virtual protected string _GetAnimationName()
    {
        return m_currentAIState switch
        {
            AIStateType.IDLE => "idle",
            AIStateType.MOVE or AIStateType.PATROL or AIStateType.TARGETING => "walk",
            AIStateType.ATTACK => "shoot",
            AIStateType.DIE => "death",
            _ => "walk"
        };
    }

    void _ChangeAnimation(bool isLoopAni = true)
    {
        if (m_skeletonAnimation == null)
            return;

        var aniName = _GetAnimationName();

        if (m_skeletonAnimation.Skeleton != null)
        {
            if (m_skeletonAnimation.Skeleton.Data.FindAnimation(aniName) == null)
                return;
        }

        m_skeletonAnimation.loop = isLoopAni;
        m_skeletonAnimation.AnimationState.SetAnimation(0, aniName, isLoopAni);
    }

    virtual protected bool SyncDirForSkillUse()
    {
        return true;
    }

    virtual protected IEnumerator _Spawn()
    {
        AddNextAI(AIStateType.IDLE);
        yield break;
    }

    virtual protected IEnumerator _Idle()
    {
        yield break;
    }

    virtual protected IEnumerator _Move()
    {
        yield break;
    }

    virtual protected IEnumerator _Attack()
    {
        m_nextSkillData = GAME_CHARACTER.LIST_SKILL_INSTANCE.FirstOrDefault(x => x.SKILL_TEMPLATE.TYPE == SkillType.ATTACK_NORMAL);
        yield break;
    }

    virtual protected IEnumerator _Patrol()
    {
        yield break;
    }

    virtual protected IEnumerator _Targeting()
    {
        yield break;
    }

    virtual protected IEnumerator _Hit()
    {
        AddNextAI(AIStateType.IDLE);
        yield break;
    }

    virtual protected IEnumerator _Die()
    {
        yield break;
    }

    virtual protected void _ToSpawn()
    {
        if (GAME_CHARACTER != null)
            GAME_CHARACTER.CHARACTER_STATE = CharacterState.SPAWNING;

        _ChangeAnimation(false);
    }

    virtual protected void _ToIdle()
    {
        if (GAME_CHARACTER != null)
            GAME_CHARACTER.CHARACTER_STATE = CharacterState.NORMAL;

        _ChangeAnimation();
    }

    virtual protected void _ToMove()
    {
        if (GAME_CHARACTER != null)
            GAME_CHARACTER.CHARACTER_STATE = CharacterState.NORMAL;

        _ChangeAnimation();
    }

    virtual protected void _ToPatrol()
    {
        if (GAME_CHARACTER != null)
            GAME_CHARACTER.CHARACTER_STATE = CharacterState.NORMAL;

        _ChangeAnimation();
    }

    virtual protected void _ToTargeting()
    {
        if (GAME_CHARACTER != null)
            GAME_CHARACTER.CHARACTER_STATE = CharacterState.NORMAL;

        _ChangeAnimation();
    }

    virtual protected void _ToAttack()
    {
        if (GAME_CHARACTER != null)
            GAME_CHARACTER.CHARACTER_STATE = CharacterState.NORMAL;

        _ChangeAnimation(false);
    }

    virtual protected void _ToHit()
    {
        if (GAME_CHARACTER != null)
            GAME_CHARACTER.CHARACTER_STATE = CharacterState.NORMAL;

        _ChangeAnimation(false);
    }

    virtual protected void _ToDie()
    {
        if (GAME_CHARACTER != null)
        {
            GAME_CHARACTER.CHARACTER_STATE = CharacterState.DIE;
            GAME_CHARACTER.OnDeath();
        }

        _ChangeAnimation(false);

        //m_skeletonAnimation.AnimationState.End += delegate
        //{
        //    CharacterManager.Instance.RemoveCharacter(GAMEOBJECT);
        //};
    }

    virtual public void AddNextAI(AIStateType nextStateType, GameCharacter targetChar = null, string skillKey = null, Vector3 targetPos = new Vector3())
    {
        stNextAI nextAI = new stNextAI
        {
            m_nextState = nextStateType,
            m_targetChar = targetChar,
            m_targetPos = targetPos,
            m_nextSkill = skillKey
        };

        m_listNextAI.Enqueue(nextAI);
    }

    void _ChangeAI(stNextAI nextAI)
    {
        m_beforeAIState = m_currentAIState;
        m_currentAIState = nextAI.m_nextState;

        if(m_currentAIState == AIStateType.TARGETING)
        {
            m_target = nextAI.m_targetChar;
            m_targetPos = m_target?.TRANSFORM.position ?? TRANSFORM.position;
        }
        else
        {
            m_targetPos = nextAI.m_targetPos;
        }

        m_aiChangeTicks = 0;

        m_arrChangeFunc[(int)nextAI.m_nextState]?.Invoke();
    }

    private IEnumerator _AILoop()
    {
        while (true)
        {
            if (m_listNextAI.Count > 0)
            {
                _ChangeAI(m_listNextAI.Dequeue());
            }

            yield return m_arrUpdateFunc[(int)m_currentAIState]?.Invoke() ?? null;
        }
    }

    protected IEnumerator _WaitDelay()
    {
        if (GAME_CHARACTER == null)
            yield break;

        while (GAME_CHARACTER.DELAY > 0)
        {
            GAME_CHARACTER.DELAY -= Time.deltaTime;
            yield return null;
        }
    }

    protected bool _IsFrontGroundEmpty()
    {
        RIGIDBODY.velocity = new Vector3(m_targetPos.x, RIGIDBODY.velocity.y);

        Vector2 nextBlock = new Vector2(RIGIDBODY.position.x + m_targetPos.x * 0.5f, RIGIDBODY.position.y);

        Debug.DrawRay(nextBlock, Vector3.down, new Color(0, 1, 0));

        RaycastHit2D raycast = Physics2D.Raycast(nextBlock, Vector3.down, 1, LayerMask.GetMask("Ground"));

        return raycast.collider == null;
    }

    void _HandleAnimationStateStart(TrackEntry trackEntry)
    {
        if(m_currentAIState == AIStateType.ATTACK)
            m_isAttack = true;
    }

    void _HandleAnimationStateComplete(TrackEntry trackEntry)
    {
        if (m_currentAIState == AIStateType.ATTACK)
            m_isAttack = false;
    }

    void _HandleAnimationStateEvent(TrackEntry trackEntry, Spine.Event e)
    {
        if (m_currentAIState == AIStateType.ATTACK)
            GAME_CHARACTER.UseSkill(m_nextSkillData);
    }
}