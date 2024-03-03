using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTemplateData : BaseData
{
    public string PREFAB { get; private set; }
    public string STAT_SHEET { get; private set; }
    public List<string> SKILL_LIST { get; private set; }
    public AIType AI_TYPE { get; private set; }
    public string NAME { get; private set; }
    public bool BODY_CHECK { get; private set; }

    public override void LoadData(SimpleJSON.JSONNode nodeData, string key)
    {
        base.LoadData(nodeData, key);

        PREFAB = Universe.GetString(nodeData, "PREFAB");
        STAT_SHEET = Universe.GetString(nodeData, "STAT_SHEET");
        SKILL_LIST = Universe.GetStringList(nodeData, "SKILL_LIST");
        AI_TYPE = Universe.GetEnum<AIType>(nodeData, "AI_TYPE");
        NAME = Universe.GetString(nodeData, "NAME");
        BODY_CHECK = Universe.GetInteger(nodeData, "BODY_CHECK") == 1;
    }
}