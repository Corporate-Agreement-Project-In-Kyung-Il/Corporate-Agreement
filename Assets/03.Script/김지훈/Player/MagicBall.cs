using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MagicBall : MonoBehaviour
{
    private static readonly int Explosion = Animator.StringToHash("Explosion");
    
    private Animator animator;
    private Collider2D collider;
    private AnimatorStateInfo currentStateInfo;
    public Transform target;
    public Player player;
    
    public bool isTargetNotDead = true;
    public float magicDamage;
    
    private void Start()
    {
        TryGetComponent(out animator);
        TryGetComponent(out collider);
    }

    void Update()
    {
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
        currentStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        transform.position = Vector2.Lerp(transform.position, target.position, Time.deltaTime);
        
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance < 0.1f)
        {
            animator.SetTrigger(Explosion);
        }
        

        if (currentStateInfo.IsName("Explosion") && currentStateInfo.normalizedTime >= 0.25f &&
            currentStateInfo.normalizedTime < 0.5f)
        {
            collider.enabled = true;
        }
        else
        {
            collider.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Enemy")).Equals(false))
            return;
        
        if(other.gameObject.TryGetComponent(out IDamageAble enemyDamage))
        {
            CombatEvent combatEvent = new CombatEvent();
            combatEvent.Receiver = enemyDamage;
            combatEvent.Sender = player;
            combatEvent.Damage = magicDamage;
            combatEvent.collider = other;
            
            CombatSystem.instance.AddCombatEvent(combatEvent);

            if (enemyDamage.CurrentHp <= 0 && other.transform.Equals(target))
            {
                isTargetNotDead = false;
                gameObject.SetActive(false);
                return;
            }
            gameObject.SetActive(false);
            isTargetNotDead = true;
        }
    }
}
