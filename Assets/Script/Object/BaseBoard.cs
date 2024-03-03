using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBoard : BaseObject
{
    public virtual BoardType BOARD_TYPE => BoardType.NONE;

    public GameCharacter OWNER { get; set; } = null;

    public virtual void OnRelease()
    {

    }

    public virtual void SetBoardData(params object[] objs)
    {

    }

    private void FixedUpdate()
    {
        if (OWNER?.BOARD_POS == null)
            return;

        TRANSFORM.position = BoardManager.Instance.GetUIPosition(OWNER.BOARD_POS.position);
    }
}
