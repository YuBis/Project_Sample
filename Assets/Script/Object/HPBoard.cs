using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBoard : BaseBoard
{
    [SerializeField]
    Image m_fillImage = null;

    [SerializeField]
    TMPro.TextMeshProUGUI m_hpLabel = null;

    float m_maxHP = 0;
    float m_curHP = 0;

    public override BoardType BOARD_TYPE => BoardType.HP;

    public override void OnRelease()
    {
        
    }

    public override void SetBoardData(params object[] objs)
    {
        base.SetBoardData(objs);

        if (OWNER == null)
            return;

        m_maxHP = (float)OWNER.GetStatValue(StatType.HP);

        Refresh();
    }

    public void Refresh()
    {
        if (OWNER == null)
            return;

        m_curHP = (float)OWNER.CURRENT_HP;

        m_fillImage.fillAmount = m_curHP / m_maxHP;
        m_hpLabel.text = $"{m_curHP} / {m_maxHP}";
    }
}
