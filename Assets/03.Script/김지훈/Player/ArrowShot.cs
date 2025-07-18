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

    [Header("타켓에 다가가는 속도")]
    [SerializeField] private float timeSinceStart = 2.1f;  
    
    //IObjectPoolItem
    public string Key { get; set; }
    public GameObject GameObject => gameObject;
    public void ReturnToPool()
    {
        target = null;
        timeSinceStart = 2.1f;
        gameObject.SetActive(false);
        ObjectPoolSystem.Instance.ReturnToPool(this);
    }
    
    private void Start()
    {
        TryGetComponent(out collider);
        collider.enabled = true;
    }

    private void Update()
    {
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

    private void FindNextTarget()
    {
        
        //현재 위치를 기준으로 제일 가까운 애를 공격하게                                                                         
        List<Collider2D> targetList = AliveExistSystem.Instance.monsterList;                               
                                                                                                             
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
    
    private void MoveToEnemyHurt()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        timeSinceStart += Time.deltaTime;  
        float t = timeSinceStart;
        float curveSpeed = Mathf.Pow(t, 2f);
        float straight = straightAttackRange * 3 / 4;
        if (distance <= straight)
        {
            Vector3 nextPos = Vector2.MoveTowards(transform.position, target.position, curveSpeed * Time.deltaTime);
            Vector3 moveDir = (nextPos - transform.position).normalized;
            
            if (moveDir != Vector3.zero)
            {
                transform.up = moveDir;
            }
            transform.position = nextPos;
        }
        else
        {
            float dynamicRotateSpeed = Mathf.Lerp(180f, 120f, distance / 5f); // 가까울수록 빠르게 회전
            Vector3 dirToTarget = (target.position - transform.position).normalized;
        
            float targetAngle = Mathf.Atan2(dirToTarget.y, dirToTarget.x) * Mathf.Rad2Deg - 90f;
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
            float maxDelta = dynamicRotateSpeed * Time.deltaTime;
            
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxDelta);
            transform.position += transform.up * (curveSpeed * Time.deltaTime);
        }
       // Debug.Log($"{gameObject.name }의 거리 = {distance}");
    }

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

}
