using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public float moveSpeed = 5f;
    [Header("카메라 Y축 따라가는 정도 조절하고 카메라가 z축으로 얼마나 떨어질지 결정하는 것")] 
    public Vector3 offset;
    
    void FixedUpdate()
    {
        Vector3 realTarget = target.position + offset;
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
}
