using UnityEngine;
using System.Collections.Generic;
using SimpleJSON;

public enum SkillType
{
    SKILL_NORMAL,
    ATTACK_NORMAL,
    BODY_CHECK,
    
    BUFF,
}

public class SkillTemplateManager : BaseGameDataManager<SkillTemplateManager, SkillTemplateData>
{
    public override void InitData()
    {
        LoadData(StaticString.SKILL_DATA);
    }
}