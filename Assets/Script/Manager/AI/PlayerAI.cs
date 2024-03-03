using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerAI : BaseAI
{
    protected override IEnumerator _Move()
    {
        yield return StartCoroutine(base._Move());

        var keyPressed = KeyMappingManager.Instance.GetHorizontalAxisKeyPressed();

        if (keyPressed != 0)
        {
            var force = keyPressed * (float)GAME_CHARACTER.MOVE_SPEED * Time.fixedDeltaTime;

            if (force > 0)
                TRANSFORM.localRotation = Quaternion.Euler(0, 0, 0);
            else
                TRANSFORM.localRotation = Quaternion.Euler(0, 180, 0);

            Vector3 targetVelocity = new Vector2(force, RIGIDBODY.velocity.y);
            RIGIDBODY.velocity = Vector3.SmoothDamp(RIGIDBODY.velocity, targetVelocity, ref m_velocity, 0.05f);
        }
        else
        {
            if (!GAME_CHARACTER.IS_JUMPING)
            {
                RIGIDBODY.velocity = new Vector2(0, 0);
                AddNextAI(AIStateType.IDLE);
                yield break;
            }
        }

        if (KeyMappingManager.Instance.IsJumpKeyPressed())
        {
            GAME_CHARACTER.Jump();
        }

        if (KeyMappingManager.Instance.IsAttackKeyPressed())
        {
            AddNextAI(AIStateType.ATTACK);
        }
    }
    override protected IEnumerator _Idle()
    {
        yield return StartCoroutine(base._Idle());

        if (KeyMappingManager.Instance.IsAttackKeyPressed())
        {
            AddNextAI(AIStateType.ATTACK);
            yield break;
        }

        if (KeyMappingManager.Instance.GetHorizontalAxisKeyPressed() != 0)
        {
            AddNextAI(AIStateType.MOVE);
        }
        else
        {
            if (Input.GetButtonDown("Jump"))
            {
                GAME_CHARACTER.Jump();
            }
        }
    }

    override protected IEnumerator _Attack()
    {
        yield return StartCoroutine(base._Attack());

        RIGIDBODY.velocity = new Vector2(0, 0);

        if(m_isAttack == false)
            AddNextAI(AIStateType.IDLE);
    }

    override protected string _GetAnimationName()
    {
        return m_currentAIState switch
        {
            AIStateType.IDLE => "idle_1",
            AIStateType.MOVE or AIStateType.PATROL or AIStateType.TARGETING => "walk",
            AIStateType.ATTACK => "sword_attack",
            AIStateType.DIE => "dead",
            _ => "walk"
        };
    }
}