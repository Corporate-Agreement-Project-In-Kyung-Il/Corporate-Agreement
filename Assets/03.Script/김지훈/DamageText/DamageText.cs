using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour, IObjectPoolItem
{
    public string Key { get; set; }
    public GameObject GameObject => gameObject;
    
    private Camera camera;
    public TMP_Text text;
    
    private Transform target;
    private float duration;
    
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
    }

    private void Update()
    {
        if (duration < 0)
        {
            ReturnToPool();
            return;
        }
        
        duration -= Time.deltaTime;
        
        transform.position = target.position;
        transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward, 
            camera.transform.rotation * Vector3.up);
    }

    public void ReturnToPool()
    {
        ObjectPoolSystem.Instance.ReturnToPool(this);
    }
}
