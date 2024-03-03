using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AggressiveAI : BaseAI
{
    override protected IEnumerator _Idle()
    {
        yield return StartCoroutine(base._Idle());

        if (m_aiChangeTicks == 0)
        {
            m_aiChangeTicks = DateTime.Now.Ticks + TimeSpan.FromSeconds(Universe.GetDoubleRandom(1, 5)).Ticks;
        }

        if (DateTime.Now.Ticks > m_aiChangeTicks)
        {
            var dir = Universe.GetIntRandom(-1, 2);
            AddNextAI(AIStateType.PATROL, null, null, new Vector3(dir, 0, 0));
        }
    }

    override protected IEnumerator _Patrol()
    {
        yield return StartCoroutine(base._Patrol());

        if (m_aiChangeTicks == 0)
        {
            m_aiChangeTicks = DateTime.Now.Ticks + TimeSpan.FromSeconds(Universe.GetDoubleRandom(1, 5)).Ticks;
        }

        var targetCharacter = CharacterManager.Instance.GetEnemyInSight(GAME_CHARACTER);

        if (targetCharacter != null)
        {
            AddNextAI(AIStateType.TARGETING, targetCharacter);
        }
        else
        {
            if (DateTime.Now.Ticks > m_aiChangeTicks || _IsFrontGroundEmpty())
            {
                AddNextAI(AIStateType.IDLE);
            }
        }
    }

    override protected IEnumerator _Targeting()
    {
        yield return StartCoroutine(base._Targeting());

        var dir = m_target.TRANSFORM.position.x > TRANSFORM.position.x ? 1 : -1;

        if (dir > 0)
            TRANSFORM.localRotation = Quaternion.Euler(0, 0, 0);
        else
            TRANSFORM.localRotation = Quaternion.Euler(0, 180, 0);

        Vector2 nextBlock = new Vector2(RIGIDBODY.position.x + dir * 0.5f, RIGIDBODY.position.y);

        Debug.DrawRay(nextBlock, Vector3.down, new Color(0, 1, 0));

        RaycastHit2D raycast = Physics2D.Raycast(nextBlock, Vector3.down, 1, LayerMask.GetMask("Ground"));

        if (raycast.collider == null)
        {
            AddNextAI(AIStateType.IDLE);
            yield break;
        }

        RIGIDBODY.velocity = new Vector3(dir, RIGIDBODY.velocity.y);
    }

    override protected void _ToDie()
    {
        base._ToDie();

        CharacterManager.Instance.RemoveCharacter(GAME_CHARACTER);
    }
}