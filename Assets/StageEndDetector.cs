using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class StageEndDetector : MonoBehaviour
{
    public Collider2D nextSceneCollider;
    public bool mb_IsDetected;
    public event Action OnStageEnd;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log(other.Equals(nextSceneCollider));
        // Debug.Log(other.name);
        
        // TODO : 다음 스테이지 넘어가게, mb_IsDetected 나중에 false해줘야함
        if (mb_IsDetected == false 
            && other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            mb_IsDetected = true;
            Debug.Log("다음 스테이지");
            OnStageEnd.Invoke();
            mb_IsDetected = false;
        }
    }
}