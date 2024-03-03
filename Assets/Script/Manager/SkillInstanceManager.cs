using UnityEngine;
using System.Collections.Generic;
using SimpleJSON;
using System.Linq;

public class SkillInstanceManager : BaseInstanceDataManager<SkillInstanceManager, SkillInstanceData>
{
    public SkillInstanceData CreateInstanceData(SkillTemplateData skillTemplateData)
    {
        SkillInstanceData skillInstanceData = new SkillInstanceData(skillTemplateData);
        skillInstanceData.Initialize();

        return skillInstanceData;
    }

    public List<SkillInstanceData> MakeDefaultSkills(CharacterTemplateData characterData)
    {
        List<SkillInstanceData> listSkillInstance = new();

        if (characterData == null)
            return listSkillInstance;

        foreach(var skillKey in characterData.SKILL_LIST)
        {
            var skillData = SkillTemplateManager.Instance.GetData(skillKey);
            if (skillData == null)
                continue;

            if (skillData.TYPE != SkillType.ATTACK_NORMAL && skillData.TYPE != SkillType.BODY_CHECK)
                continue;

            listSkillInstance.Add(CreateInstanceData(skillData));
        }

        return listSkillInstance;
    }

    public List<SkillInstanceData> GetSkillListByType(SkillType skillType) =>
        m_dicData.Values.Where(x => x.SKILL_TEMPLATE.TYPE == skillType).ToList();
}