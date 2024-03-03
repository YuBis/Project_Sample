using SimpleJSON;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class BaseGameDataManager<MANAGER_TYPE, DATA_TYPE> : BaseManager<MANAGER_TYPE>
    where DATA_TYPE : BaseData, new()
    where MANAGER_TYPE : DataLoader, new()
{
    protected Dictionary<string, DATA_TYPE> m_dicData = new Dictionary<string, DATA_TYPE>();

    protected void LoadData(string assetKey) => LoadData(assetKey, LoadDataFile);

    protected void LoadDataFile(JSONObject jsonObject)
    {
        foreach (KeyValuePair<string, JSONNode> data in jsonObject)
        {
            var objData = data.Value as JSONObject;
            if (objData == null)
                continue;

            foreach (var tableObj in objData)
            {
                var tableData = new DATA_TYPE();
                tableData.LoadData(tableObj.Value, tableObj.Key);
                m_dicData.Add(tableData.KEY, tableData);
            }
        }
    }

    public DATA_TYPE GetData(string key)
    {
        if (m_dicData.TryGetValue(key, out var data))
            return data;

        return null;
    }
}
