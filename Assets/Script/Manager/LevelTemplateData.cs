using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelTemplateData : BaseData
{
    public int LEVEL { get; private set; }
    public long EXP { get; private set; }

    public override void LoadData(SimpleJSON.JSONNode nodeData, string key)
    {
        base.LoadData(nodeData, key);

        LEVEL = Universe.GetInteger(nodeData, "LEVEL");
        EXP = Universe.GetLong(nodeData, "EXP");
    }
}