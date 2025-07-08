using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShot : MonoBehaviour
{
    private Collider2D collider;
    private Vector3 prevPosition;
    public Transform target;
    public Player player;
    
    public bool isTargetNotDead = true;
    public float arrowDamage;

    private void Start()
    {
        TryGetComponent(out collider);
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
        collider.enabled = false;
        
        //현재 위치를 기준으로 제일 가까운 애를 공격하게                                                                         
        List<Collider2D> targetList = MonsterExistSystem.Instance.monsterList;                               
                                                                                                             
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
        //else
        //{
        //    gameObject.SetActive(false);
        //}
        
    }
    
    private void MoveToEnemyHurt()
    {
        Vector3 nextPos = Vector2.MoveTowards(transform.position, target.position, Time.deltaTime);
        Vector3 moveDir = (nextPos - transform.position).normalized;
        
        if (moveDir != Vector3.zero)
        {
            transform.up = moveDir;
        }
        transform.position = nextPos;
        
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance < 0.1f)
        {
            collider.enabled = true;
        }
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
            gameObject.SetActive(false);
            
            if (enemyDamage.CurrentHp <= 0 && other.transform.Equals(target))
            {
                isTargetNotDead = false;
                return;
            }
            isTargetNotDead = true;
        }

    }
}
