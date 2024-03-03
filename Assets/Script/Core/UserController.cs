using UnityEngine;

public class UserController : BaseObject
{
    private GameCharacter controlTarget;

    float m_horizontalMove = 0f;
    bool m_bJump = false;

	public void SetControlTarget(GameCharacter target)
    {
		controlTarget = target;
    }

    //private void Update()
    //{
    //    if (controlTarget == null)
    //        return;

    //    if (controlTarget.CHARACTER_STATE == CharacterState.DIE)
    //        return;

    //    if (controlTarget.DELAY > 0)
    //        return;

    //    m_horizontalMove = controlTarget != null ? 
    //        Input.GetAxisRaw("Horizontal") * (float)controlTarget.MOVE_SPEED : 0;

        
    //}

    //private void FixedUpdate()
    //{
    //    if (controlTarget == null)
    //        return;

    //    if (controlTarget.DELAY > 0)
    //    {
    //        controlTarget.DELAY -= Time.deltaTime;
    //        return;
    //    }
    //    else if (controlTarget.DELAY < 0)
    //    {
    //        controlTarget.DELAY = 0;
    //    }

    //    //controlTarget.Move(m_horizontalMove * Time.fixedDeltaTime, m_bJump);

    //    if (m_horizontalMove != 0)
    //    {
    //        if (!m_bIsMoving)
    //        {
    //            controlTarget.GetComponent<BaseAI>().AddNextAI(AIStateType.MOVE);
    //            m_bIsMoving = true;
    //        }
    //    }
    //    else
    //    {
    //        if (m_bIsMoving)
    //        {
    //            controlTarget.GetComponent<BaseAI>().AddNextAI(AIStateType.IDLE);
    //            m_bIsMoving = false;
    //        }
    //    }

    //    m_bJump = false;
    //}

    bool m_bIsMoving = false;
}