using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Security.Cryptography;
using UnityEngine;

public struct stStatSheetInfo
{
    public StatType STAT_NAME { get; private set; }
    public double VALUE { get; private set; }
    public double VALUE_PER_LV { get; private set; }

    public void LoadData(SimpleJSON.JSONNode nodeData)
    {
        STAT_NAME = Universe.GetEnum<StatType>(nodeData, "STAT_NAME");
        VALUE = Universe.GetDouble(nodeData, "VALUE");
        VALUE_PER_LV = Universe.GetDouble(nodeData, "VALUE_PER_LV");
    }
}

public class StatSheetData : BaseData
{
    Dictionary<StatType, stStatSheetInfo> m_dicStatSheet = new();
    public IReadOnlyDictionary<StatType, stStatSheetInfo> DIC_STAT_SHEET => m_dicStatSheet; 

    public override void LoadData(SimpleJSON.JSONNode nodeData, string key)
    {
        base.LoadData(nodeData, key);

        JSONArray nodeArray = nodeData as JSONArray;
        if (nodeArray == null)
            return;

        for (int i = 0; i < nodeArray.Count; ++i)
        {
            JSONNode data = nodeArray[i];
            if (data == null)
                continue;

            var statSheet = new stStatSheetInfo();
            statSheet.LoadData(data);

            m_dicStatSheet.Add(statSheet.STAT_NAME, statSheet);
        }
    }
}