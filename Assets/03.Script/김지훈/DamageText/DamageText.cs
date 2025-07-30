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
    
    private static Dictionary<Transform, int> targetSpawnCounter = new Dictionary<Transform, int>();
    private int myOrderIndex = 0; // 내가 몇 번째로 생성된 DamageText인지
    private void Awake()
    {
        TryGetComponent(out text);
        camera = Camera.main;
    }

    public void Set(Transform target, string content, float duration, Color color)
    {
        this.target = target;
        text.text = content;
        this.duration = duration;
        text.color = color;
        
        if (!targetSpawnCounter.ContainsKey(target))
            targetSpawnCounter[target] = 0;

        myOrderIndex = targetSpawnCounter[target];
        targetSpawnCounter[target]++;
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
        ObjectPoolSystem.Instance.ReturnToPool(this);
    }
}
