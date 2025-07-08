using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShot : MonoBehaviour
{
    private Collider2D collider;
    
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
        transform.position = Vector2.Lerp(transform.position, target.position, Time.deltaTime);
        
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
