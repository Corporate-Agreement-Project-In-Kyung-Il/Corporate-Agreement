using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IObjectPoolItem
{
    string Key { get; set; }
    GameObject GameObject { get;}
    void ReturnToPool();
}

public class ObjectPool
{
    private Queue<IObjectPoolItem> Pool { get; set; }
    private IObjectPoolItem Sample { get; set; }
    private Transform Parent { get; set; }
    private byte ExpandSize { get; set; }

    public void Initialize(IObjectPoolItem sample, Transform parent, byte expandSize, string key)
    {
        Pool = new Queue<IObjectPoolItem>();
        Sample = sample;
        Parent = parent;
        ExpandSize = expandSize;

        Sample.Key = key;
        Expand();
    }

    private void Expand()
    {
        for (int i = 0; i < ExpandSize; i++)
        {
            var instance = GameObject.Instantiate(Sample.GameObject, Parent).GetComponent<IObjectPoolItem>();
            instance.Key = Sample.Key;
            Return(instance);
        }
    }

    public IObjectPoolItem Get()
    {
        if (Pool.Count == 0)
        {
            Expand();
        }

        return Pool.Dequeue();
    }

    public void Return(IObjectPoolItem item)
    {
        item.GameObject.SetActive(false);
        Pool.Enqueue(item);
    }
}
