using UnityEngine;
using System.Collections.Generic;
using SimpleJSON;
using Spine;
using static UnityEditor.LightingExplorerTableColumn;

public class StringTableManager : BaseManager<StringTableManager>
{
    Dictionary<string, string> m_dicStringTable = new();

    public override void InitData()
    {
        LoadData(StaticString.STRING_TABLE, _LoadStringTable);
    }

    void _LoadStringTable(JSONObject jsonObject)
    {
        foreach (KeyValuePair<string, JSONNode> data in jsonObject)
        {
            var objData = data.Value as JSONObject;
            if (objData == null)
                continue;

            foreach (var tableObj in objData)
            {
                var stringKey = tableObj.Key;

                if (m_dicStringTable.ContainsKey(stringKey))
                {
                    Universe.LogDebug("Stringtable already has string! : " + stringKey);
                    continue;
                }

                var stringValue = Universe.GetString(tableObj, "VALUE");
                m_dicStringTable[stringKey] = stringValue;
            }
        }
    }

    public string GetString(string key)
    {
        if (m_dicStringTable.TryGetValue(key, out var value))
            return value;

        return key;
    }
}