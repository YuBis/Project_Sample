using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class NonAggressiveAI : BaseAI
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

        if (m_targetPos.x > TRANSFORM.position.x)
            TRANSFORM.localRotation = Quaternion.Euler(0, 0, 0);
        else
            TRANSFORM.localRotation = Quaternion.Euler(0, 180, 0);

        if (m_aiChangeTicks == 0)
        {
            m_aiChangeTicks = DateTime.Now.Ticks + TimeSpan.FromSeconds(Universe.GetDoubleRandom(1, 5)).Ticks;
        }

        if (DateTime.Now.Ticks > m_aiChangeTicks || _IsFrontGroundEmpty())
        {
            AddNextAI(AIStateType.IDLE);
        }
    }

    override protected void _ToDie()
    {
        base._ToDie();

        CharacterManager.Instance.RemoveCharacter(GAME_CHARACTER);
    }
}