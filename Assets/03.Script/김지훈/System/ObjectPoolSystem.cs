using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObjectPoolSystem : MonoBehaviour
{
    public static ObjectPoolSystem Instance;

    [System.Serializable]
    public class ObjectPoolData
    {
        public string Key;
        public GameObject Prefab;
        public byte ExpandSize;
    }

    public ObjectPoolData[] ObjectPoolDatas;
    private Dictionary<string, ObjectPool> objectPoolDic = new Dictionary<string, ObjectPool>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        foreach (var data in ObjectPoolDatas)
        {
            CreatePool(data);
        }
    }

    public void CreatePool(ObjectPoolData data)
    {
        if (objectPoolDic.ContainsKey(data.Key))
        {
            Debug.Log($"{data.Key}가 이미 존재함");
            return;
        }
        var pool = new ObjectPool();
        var poolItem = data.Prefab.GetComponent<IObjectPoolItem>();

        if (poolItem == null)
        {
            Debug.Log("옳바른 형식이 아닌 Prefab임");
            return;
        }
        pool.Initialize(poolItem, transform, data.ExpandSize, data.Key);
        objectPoolDic.Add(data.Key, pool);
    }

    public IObjectPoolItem GetObjectOrNull(string key)
    {
        if (objectPoolDic.ContainsKey(key) == false)
        {
            Debug.Log($"{key} 값의 Pool이 존재하지 않습니다.");
            return null;
        }

        return objectPoolDic[key].Get();
    }

    public void ReturnToPool(IObjectPoolItem item)
    {
        if (objectPoolDic.ContainsKey(item.Key) == false)
        {
            Debug.LogWarning($"{item.Key} 값의 Pool이 존재하지 않음.");
            return;
        }

        objectPoolDic[item.Key].Return(item);
    }
}