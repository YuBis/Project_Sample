using System.Collections.Generic;
using UnityEngine;

public class SkillTemplateData : BaseData
{
    public Rect RANGE { get; private set; }
    public double RUNNING_TIME { get; private set; }
    public string STAT_SHEET { get; private set; }
    public string EFFECT { get; private set; }
    public string EFFECT_HIT { get; private set; }
    public string NAME { get; private set; }
    public string DESC { get; private set; }
    public string ICON { get; private set; }
    public int MAX_LEVEL { get; private set; }
    public SkillType TYPE { get; private set; }

    public override void LoadData(SimpleJSON.JSONNode nodeData, string key)
    {
        base.LoadData(nodeData, key);

        RUNNING_TIME = Universe.GetDouble(nodeData, "RUNNING_TIME");
        STAT_SHEET = Universe.GetString(nodeData, "STAT_SHEET");
        EFFECT = Universe.GetString(nodeData, "EFFECT");
        EFFECT_HIT = Universe.GetString(nodeData, "EFFECT_HIT");
        NAME = Universe.GetString(nodeData, "NAME");
        DESC = Universe.GetString(nodeData, "DESC");
        ICON = Universe.GetString(nodeData, "ICON");
        MAX_LEVEL = Universe.GetInteger(nodeData, "MAX_LEVEL");
        TYPE = Universe.GetEnum<SkillType>(nodeData, "TYPE");

        RANGE = new Rect(
            Universe.GetInteger(nodeData, "RANGE_LB_X"),
            Universe.GetInteger(nodeData, "RANGE_LB_Y"),
            Universe.GetInteger(nodeData, "RANGE_WIDTH"),
            Universe.GetInteger(nodeData, "RANGE_HEIGHT")
            );

    }
}