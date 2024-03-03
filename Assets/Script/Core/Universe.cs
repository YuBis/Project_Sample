using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Universe
{
    static Carrier m_carrier = null;
    static System.Random m_rand = new System.Random();

    static public Carrier GetCarrier()
    {
        if (m_carrier == null)
        {
            var carrier = new GameObject("CARRIER");
            m_carrier = carrier.AddComponent<Carrier>();
            UnityEngine.Object.DontDestroyOnLoad(carrier);
        }

        return m_carrier;
    }

    static public void StartCoroutine(IEnumerator coroutine)
    {
        if (coroutine == null)
            return;

        GetCarrier()?.StartCoroutine(coroutine);
    }

    static public void LogError(string str) => Debug.LogError(str);
    static public void LogWarning(string str) => Debug.LogWarning(str);
    static public void LogDebug(string str) => Debug.Log(str);

    static public IEnumerator CallAfterTime(int sec, System.Action callback)
    {
        yield return new WaitForSeconds(sec);
        callback?.Invoke();
    }

    static public bool IsJsonAvailable(JSONNode nodeData, string strKey)
    {
        if (nodeData == null)
            return false;

        if (nodeData[strKey].Tag == JSONNodeType.None)
            return false;

        return true;
    }

    static public string GetDefaultKey(JSONNode nodeData) => GetString(nodeData, "KEY");

    static public string GetString(JSONNode nodeData, string strKey)
    {
        if(!IsJsonAvailable(nodeData, strKey))
            return string.Empty;

        return nodeData[strKey];
    }

    static public JSONArray GetJsonArray(JSONNode nodeData, string strKey)
    {
        if (!IsJsonAvailable(nodeData, strKey))
            return null;

        return nodeData[strKey] as JSONArray;
    }

    static public T GetEnum<T>(JSONNode nodeData, string strKey) where T : Enum
    {
        if (!IsJsonAvailable(nodeData, strKey))
            return default;

        string strData = nodeData[strKey];

        if (Enum.IsDefined(typeof(T), strData) == false)
        {
            LogError("Cannot find " + strData + " from " + typeof(T).Name);
            return default;
        }

        return (T)Enum.Parse(typeof(T), strData);
    }

    static public float GetFloat(JSONNode nodeData, string strKey)
    {
        if (!IsJsonAvailable(nodeData, strKey))
            return 0;

        return nodeData[strKey].AsFloat;
    }

    static public int GetInteger(JSONNode nodeData, string strKey)
    {
        if (!IsJsonAvailable(nodeData, strKey))
            return 0;

        return nodeData[strKey].AsInt;
    }

    static public long GetLong(JSONNode nodeData, string strKey)
    {
        if (!IsJsonAvailable(nodeData, strKey))
            return 0;

        return nodeData[strKey].AsLong;
    }

    static public bool GetBool(JSONNode nodeData, string strKey)
    {
        if (!IsJsonAvailable(nodeData, strKey))
            return false;

        return nodeData[strKey].AsBool;
    }

    static public double GetDouble(JSONNode nodeData, string strKey)
    {
        if (!IsJsonAvailable(nodeData, strKey))
            return 0;

        return nodeData[strKey].AsDouble;
    }

    static public List<string> GetStringList(JSONNode nodeData, string strKey)
    {
        var list = new List<string>();

        var listData = GetJsonArray(nodeData, strKey);
        if (listData == null)
            return list;

        for (int i = 0; i < listData.Count; ++i)
        {
            list.Add(listData[i]);
        }

        return list;
    }

    static public double GetDoubleRandom(double min, double max)
    {
        if (min >= max)
            return min;

        return m_rand.NextDouble() * (max - min) * min;
    }

    static public int GetIntRandom(int min, int max)
    {
        if (min >= max)
            return min;

        return m_rand.Next(min, max);
    }
}
