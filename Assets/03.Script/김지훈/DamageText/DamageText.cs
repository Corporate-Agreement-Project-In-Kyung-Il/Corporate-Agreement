using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour, IObjectPoolItem
{
    [Header("Text가 곂쳐 나올 때 위로 뜨는 정도")]
    public float TextUpPosition;
    public string Key { get; set; }
    public GameObject GameObject => gameObject;
    
    private Camera camera;
    public TMP_Text text;
    
    private Transform target;
    private float duration;
    
    private static Dictionary<Transform, List<bool>> targetSpawnCounter = new Dictionary<Transform, List<bool>>();
    private int myOrderIndex = 0; // 내가 몇 번째로 생성된 DamageText인지
    private void Awake()
    {
        TryGetComponent(out text);
        camera = Camera.main;
    }

    public void Set(Transform target, string content, float duration, Color color)
    {
        this.target = target;
        this.duration = duration;
        text.text = content;
        text.color = color;
        
        if (!targetSpawnCounter.ContainsKey(target))
            targetSpawnCounter[target] = new List<bool>();
            //targetSpawnCounter[target] = 0;

        //myOrderIndex = targetSpawnCounter[target];
        //targetSpawnCounter[target]++;
        
        List<bool> slots = targetSpawnCounter[target];
        
        int index = slots.FindIndex(s => s == false);
        if (index == -1)
        {
            index = slots.Count;
            slots.Add(true);
        }else
        {
            slots[index] = true;
        }
        myOrderIndex = index;
    }

    private void Update()
    {
        if (target == null || duration < 0)
        {
            ReturnToPool();
            return;
        }
        
        duration -= Time.deltaTime;
        
        Vector3 offset = Vector3.up * (0.2f +TextUpPosition * myOrderIndex);
        transform.position = target.position + offset;

        transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward,
            camera.transform.rotation * Vector3.up);
    }

    public void ReturnToPool()
    {
        if (target != null && targetSpawnCounter.ContainsKey(target))
        {
            List<bool> slots = targetSpawnCounter[target];
            if (myOrderIndex >= 0 && myOrderIndex < slots.Count)
            {
                slots[myOrderIndex] = false;
            }

            // 전체 슬롯이 비었다면 해당 타겟에 대한 슬롯도 정리
            bool allEmpty = true;
            foreach (bool b in slots)
            {
                if (b)
                {
                    allEmpty = false;
                    break;
                }
            }

            if (allEmpty)
            {
                targetSpawnCounter.Remove(target);
            }
        }
        
        ObjectPoolSystem.Instance.ReturnToPool(this);
    }
}
