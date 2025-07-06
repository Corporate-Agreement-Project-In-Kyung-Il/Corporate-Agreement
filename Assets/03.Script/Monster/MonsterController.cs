using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterController : MonoBehaviour, IDamageAble
{
    //IDamageAble 요소
    public Collider2D mainCollider => collider2D;
    public GameObject GameObject => gameObject;
    public float Damage => monsterStat.damage;
    
    //Component 받아오는 요소
    public MonsterData monsterData;
    public MonsterStat monsterStat = new MonsterStat();
    private Collider2D collider2D;
    
    //CharacterState 정의 요소
    [SerializeField] private EnemyState currentCharacterState = EnemyState.Idle;
    [SerializeField] private EnemyState prevCharacterState = EnemyState.Idle;


    private void Awake()
    {
        TryGetComponent(out collider2D);

        monsterStat.health = monsterData.Monster_HP;
        
        monsterStat.damage = monsterData.Monster_Attack;
        monsterStat.playerDetectionRange = monsterData.playerDetectionRange;
        monsterStat.attackRange = monsterData.attackRange;
        monsterStat.attackSpeed = monsterData.attackSpeed;
        
        monsterStat.quantity = monsterData.Monster_quantity;
        monsterStat.moveSpeed = monsterData.moveSpeed;
    }

    private void Update()
    {
        switch (currentCharacterState)
        {
            case EnemyState.Idle :
                performIdle();
                break;
            case EnemyState.Run :
                performRun();
                break;
            case EnemyState.Attack:
                performAttack();
                break;
            case EnemyState.Die :
                performDie();
                break;
        }
    }

    private void performIdle()
    {
        
    }

    private void performRun()
    {
        
    }

    private void performAttack()
    {
        
    }

    private void performDie()
    {
        
    }
    
    public void TakeDamage(CombatEvent combatEvent)
    {
        if (monsterStat.health <= 0)
        {
            Destroy(gameObject);
        }
    }
}

public class MonsterStat
{
    public float health;
    public float damage;
    public float quantity;
    
    public float attackRange;
    public float attackSpeed;
    public float moveSpeed;
    public float playerDetectionRange;
}

public enum EnemyState
{
    Idle,
    Run,
    Attack,
    Die
}
