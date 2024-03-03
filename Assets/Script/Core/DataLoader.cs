using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public struct stLoadingData
{
    public Action<JSONObject> Callback;
    public string FileName;
}

public class DataLoader
{
    public static int LOADING_DATA = 0;

    public virtual void InitData() { }

    protected void LoadData(string assetKey, Action<JSONObject> callBack)
    {
        var op = Addressables.LoadAssetAsync<TextAsset>(assetKey);
        if (op.IsValid() == false)
            return;

        System.Threading.Interlocked.Increment(ref LOADING_DATA);

        Universe.StartCoroutine(_LoadDataAsync(assetKey, op, callBack));
    }

    IEnumerator _LoadDataAsync(string assetKey, AsyncOperationHandle<TextAsset> operation, Action<JSONObject> callBack)
    {
        yield return operation;

        System.Threading.Interlocked.Decrement(ref LOADING_DATA);

        var result = operation.Result?.text;
        if (string.IsNullOrEmpty(result))
        {
            Universe.LogError($"{assetKey} : Cannot load asset!");
            yield break;
        }

        if (Available(result) is var obj && obj != null)
            callBack?.Invoke(obj);
    }

    JSONObject Available(string assetData)
    {
        if (string.IsNullOrEmpty(assetData))
            return null;

        return JSON.Parse(assetData) as JSONObject;
    }
}
