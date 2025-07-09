using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveAttackSystem : MonoBehaviour
{
    [Header("목표 트랙킹")]
    public Transform target;
    public float rotateSpeedDegPerSec = 180f; // 프레임당 회전 제한 (degrees per second)
    public float moveSpeed = 5f;

    [Header("생명 주기")]
    public float lifeTime = 5f;
    public GameObject explosionEffect;
    private bool moving = true;

    private Rigidbody2D rb;
    private Animator ani;

    void Start()
    {
        TryGetComponent(out rb);
        TryGetComponent(out ani);
        Destroy(gameObject, lifeTime);
    }

    void FixedUpdate()
    {
        if (moving.Equals(false))
            return;
        
        if (target == null)
        {
            rb.velocity = transform.up * moveSpeed;
            return;
        }

        // 타겟 방향 계산
        Vector2 direction = ((Vector2)target.position - rb.position).normalized;

        // 목표 회전값 계산
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f; // 위쪽을 기준으로 보정
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);

        // 회전 제한량 계산
        float maxDelta = rotateSpeedDegPerSec * Time.fixedDeltaTime;

        // 현재 방향에서 목표 방향으로 제한된 회전 적용
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxDelta);

        // 현재 forward(=up) 방향으로 이동
        rb.velocity = transform.up * moveSpeed;

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance < 0.1f)
        {
            ani.SetTrigger("Explosion");
            moving = false;
            transform.rotation = Quaternion.identity;
            rb.velocity = Vector2.zero;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Enemy")).Equals(false))
            return;
    }
}
