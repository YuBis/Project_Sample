using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageBoard : BaseBoard
{
    [SerializeField]
    Image m_fillImage = null;

    public override BoardType BOARD_TYPE => BoardType.DAMAGE;

    public override void OnRelease()
    {

    }

    public override void SetBoardData(params object[] objs)
    {
        base.SetBoardData(objs);


    }
}
