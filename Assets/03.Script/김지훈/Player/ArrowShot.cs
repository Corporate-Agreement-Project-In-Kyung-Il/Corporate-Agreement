using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ArrowShot : MonoBehaviour, IObjectPoolItem
{
    private Collider2D collider;
    private Vector3 prevPosition;
    public Transform target;
    public Player player;
    public float straightAttackRange;
    
    public bool isTargetNotDead = true;
    public float arrowDamage;

    [Header("유도 설정")] 
    [SerializeField] private float initialSpeed = 2f;
    [Header("최대 속도")] public float maxSpeed = 5f;
    [Header("타켓에 다가가는 속도")]
    [SerializeField] private float timeSinceStart = 2.1f;  
    private Vector3 lastTarget = new Vector3(0, 80f, 0f);
    
    //IObjectPoolItem
    public string Key { get; set; }
    public GameObject GameObject => gameObject;
    public void ReturnToPool()
    {
        target = null;
        transform.rotation = Quaternion.Euler(0f, 0f, 45f);
        timeSinceStart = 2.1f;
        gameObject.SetActive(false);
        ObjectPoolSystem.Instance.ReturnToPool(this);
    }
    
    private void Start()
    {
        TryGetComponent(out collider);
        collider.enabled = true;
        targetList = AliveExistSystem.Instance.monsterList;     
    }

    private void Update()
    {
        if (targetList.Count <= 0)
        {
            target.position = lastTarget;
            MoveToEnemyHurt();
            return;
        }
        
        //transform.position = Vector2.MoveTowards(transform.position, target.position, Time.deltaTime);
        if (target.gameObject.activeSelf.Equals(false))
        {
            FindNextTarget();
        }
        else
        { 
            MoveToEnemyHurt();
        }
    }

    private List<Collider2D> targetList;
    private void FindNextTarget()
    {
        //현재 위치를 기준으로 제일 가까운 애를 공격하게                                                                         
        targetList = AliveExistSystem.Instance.monsterList;                               
        
        float minDistance = 100f;                                                                            
        Transform closestTarget = null;


        
        for (int i = 0; i < targetList.Count; i++)                                                           
        {                                                                                                    
            if (targetList[i] != null && targetList[i].gameObject.activeSelf.Equals(false))                  
                continue;                                                                                    
                                                                                                             
            float dist = Vector2.Distance(transform.position, targetList[i].transform.position);             
                                                                                                             
            if (dist < minDistance)                                                                          
            {                                                                                                
                minDistance = dist;                                                                          
                closestTarget = targetList[i].transform;                                                     
            }                                                                                                
        }                                                                                                    
                                                                                                             
        if (closestTarget != null)                                                                           
        {                                                                                                    
            target = closestTarget;                                                                          
        }
        
    }
    [Header("휘어지는 정도")] public float curveFloat;
    public float straight;

    private float velocity;
    private void MoveToEnemyHurt()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        timeSinceStart += Time.deltaTime;  
        float t = timeSinceStart;
        float curveSpeed = Mathf.Min(Mathf.Pow(t, curveFloat), maxSpeed);
        velocity = curveSpeed * initialSpeed * Time.deltaTime;
        if (distance <= straight || target.transform.position.y < transform.position.y)
        {
            Vector3 toTarget = (target.position - transform.position).normalized;
            Vector3 newDirection = Vector3.Slerp(transform.up, toTarget, Time.deltaTime * 10f); // 전환 속도 조절
            transform.up = newDirection;
            transform.position += transform.up * velocity;
            
            //Vector3 nextPos = Vector2.MoveTowards(transform.position, target.position, velocity);
            //Vector3 moveDir = (nextPos - transform.position).normalized;
            //
            //if (moveDir != Vector3.zero)
            //{
            //    transform.up = moveDir;
            //}
            //transform.position = nextPos;
        }
        else
        {
            float dynamicRotateSpeed = Mathf.Lerp(180f, 120f, distance / 5f); // 가까울수록 빠르게 회전
            Vector3 dirToTarget = (target.position - transform.position).normalized;
        
            float targetAngle = Mathf.Atan2(dirToTarget.y, dirToTarget.x) * Mathf.Rad2Deg - 90f;
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
            float maxDelta = dynamicRotateSpeed * Time.deltaTime;
            
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxDelta);
            transform.position += transform.up * (velocity);
        }
       // Debug.Log($"{gameObject.name }의 거리 = {distance}");
    }

    //private void MoveToLastTarget()
    //{
    //    float distanceToTarget = Vector3.Distance(transform.position, lastTarget);
    //    float dynamicRotateSpeed = Mathf.Lerp(180f, 90f, distanceToTarget / 5f); // 가까울수록 빠르게 회전
    //    Vector3 dirToTarget = (lastTarget - transform.position).normalized;
    //    float targetAngle = Mathf.Atan2(dirToTarget.y, dirToTarget.x) * Mathf.Rad2Deg - 90f;
    //    Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
    //
    //    float maxDelta = dynamicRotateSpeed * Time.deltaTime;
    //    
    //    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxDelta);
    //
    //    transform.position += transform.up * (velocity);
    //}
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Enemy")).Equals(false))
            return;

        if (other.gameObject.TryGetComponent(out IDamageAble enemyDamage) && other.transform.Equals(target))
        {
            CombatEvent combatEvent = new CombatEvent();
            combatEvent.Receiver = enemyDamage;
            combatEvent.Sender = player;
            combatEvent.Damage = arrowDamage;
            combatEvent.collider = other;
            CombatSystem.instance.AddCombatEvent(combatEvent);
            
            ReturnToPool();

            if (enemyDamage.CurrentHp <= 0 && other.transform.Equals(target))
            {
                isTargetNotDead = false;
                return;
            }
            isTargetNotDead = true;
        }
    }
    
    private void OnEnable()
    {
        StageClearEvent.stageClearEvent += ReturnToPool;
    }

    private void OnDisable()
    {
        StageClearEvent.stageClearEvent -= ReturnToPool;
    }
}
