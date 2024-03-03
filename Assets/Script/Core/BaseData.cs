using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseData
{
    public string KEY { get; protected set; }

    virtual public void LoadData(SimpleJSON.JSONNode nodeData)
    {
        LoadData(nodeData, Universe.GetString("KEY", nodeData));
    }

    virtual public void LoadData(SimpleJSON.JSONNode nodeData, string key)
    {
        KEY = key;
    }
}
