using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = System.Diagnostics.Debug;
using Random = UnityEngine.Random;

public class FollowCamera : MonoBehaviour
{
    private static FollowCamera instance;
    
    [Header("카메라가 따라갈 target")]
    public Transform target;
    [Header("카메라가 target을 따라가는 속도")]
    public float moveSpeed = 5f;
    [Header("카메라 Y축 따라가는 정도 조절하고 카메라가 z축으로 얼마나 떨어져저 보여줄지 결정하는 것")] 
    public Vector3 offset;
    
    [Header("카메라 흔들림 구현")]
    public float shakeDuration = 0.5f;  // 흔들리는 시간
    public float shakeMagnitude = 0.2f; // 흔들림 세기

    private Vector3 shakeOffset;
    private float shakeTimer = 0f;
    private Vector3 cameraPosition = new Vector3(0.5f, 1, -10);
    private void Awake()
    {
        instance = this;
    }

    void FixedUpdate()
    {
        if (AliveExistSystem.Instance.playerList.Count <= 0) 
            return;
        
        Follow();
    }
    
    private void LateUpdate()
    {
        Vector3 basePosition = transform.position;

        if (shakeTimer > 0f)
        {
            shakeOffset = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f),
                0f
            ) * shakeMagnitude;

            shakeTimer -= Time.deltaTime;
        }
        else
        {
            shakeOffset = Vector3.zero;
        }

        // Follow()에서 설정한 위치에 흔들림만 더하기
        transform.position = basePosition + shakeOffset;
    }
    
    //private void UpdateShake()
    //{
    //    if (shakeTimer > 0f)
    //    {
    //
    //        Vector3 randomOffset = new Vector3(
    //            Random.Range(-1f, 1f),
    //            Random.Range(-1f, 1f),
    //            0f
    //        ) * shakeMagnitude;
    //
    //        transform.position += randomOffset;
    //
    //        shakeTimer -= Time.fixedDeltaTime;
    //    }
    //    else
    //    {
    //        transform.position = Vector3.right  * 0.5f + Vector3.up * realTarget.y + Vector3.forward * realTarget.z;
    //    }
    //}

    private Vector3 realTarget;
    private Vector3 velocity = Vector3.zero;
    
    [Header("카메라가 적을 만났을 때 댐핑되느 정도")]
    public float dampingValue = 0.08f;
    private void Follow()
    {
        realTarget = target.position + offset;
        Vector3 targetPos = new Vector3(0.5f, realTarget.y, realTarget.z);
        
        float distance = realTarget.y - transform.position.y;
        if (distance < dampingValue)
        {
            transform.position = targetPos;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.fixedDeltaTime * moveSpeed);
        }
    }

    public static void Shake()
    {
        instance.shakeTimer = instance.shakeDuration;
    }

    private void InitalPosition()
    {
        transform.position = cameraPosition;
    }
    private void OnEnable()
    {
        StageEvent.stageClearEvent += InitalPosition;
        DamgeEvent.ShakeEvent += Shake;
    }

    private void OnDisable()
    {
        StageEvent.stageClearEvent -= InitalPosition;
        DamgeEvent.ShakeEvent -= Shake;
        
    }

    
    [Conditional("UNITY_EDITOR")] //에디터에서만 돈다. Runtime에서는 삭제되는 코드임
    private void OnValidate()
    {
        Debug.Assert(target == null ,"target이 비어있습니다.");
        
        #if UNITY_EDITOR
        Debug.Assert(target == null ,"target이 비어있습니다.");
    #endif
    }
}
