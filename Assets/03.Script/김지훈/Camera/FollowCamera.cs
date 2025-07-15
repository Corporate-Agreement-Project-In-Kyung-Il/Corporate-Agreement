using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FollowCamera : MonoBehaviour
{
    private static FollowCamera instance;
    
    [Header("카메라가 따라갈 target")]
    public Transform target;
    [Header("카메라가 target을 따라가느 속도")]
    public float moveSpeed = 5f;
    [Header("카메라 Y축 따라가는 정도 조절하고 카메라가 z축으로 얼마나 떨어져저 보여줄지 결정하는 것")] 
    public Vector3 offset;
    
    [Header("카메라 흔들림 구현")]
    public float shakeDuration = 0.5f;  // 흔들리는 시간
    public float shakeMagnitude = 0.2f; // 흔들림 세기

    private Vector3 shakeOffset;
    private float shakeTimer = 0f;

    private void Awake()
    {
        instance = this;
    }

    void FixedUpdate()
    {
        if (AliveExistSystem.Instance.playerList.Count <= 0) 
            return;
        
        UpdateShake();
        Follow();
    }
    private void UpdateShake()
    {
        if (shakeTimer > 0f)
        {

            Vector3 randomOffset = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f),
                0f
            ) * shakeMagnitude;

            transform.position += randomOffset;

            shakeTimer -= Time.fixedDeltaTime;
        }
        else
        {
            transform.position = Vector3.up * realTarget.y + Vector3.forward * realTarget.z;
        }
    }

    private Vector3 realTarget;
    private void Follow()
    {
        realTarget = target.position + offset;
        float distance = target.position.y - transform.position.y;
        
        if (realTarget.y > transform.position.y) 
            if (distance > 0.01f) 
            { 
                transform.position = Vector3.Lerp(transform.position, Vector3.up * realTarget.y + Vector3.forward * realTarget.z, moveSpeed * Time.deltaTime); 
            }
            else 
            { 
                transform.position = Vector3.up * realTarget.y + Vector3.forward * realTarget.z; 
            }
    }

    public static void Shake()
    {
        instance.shakeTimer = instance.shakeDuration;
    }
    
}
