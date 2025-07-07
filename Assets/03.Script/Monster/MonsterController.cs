using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.XR;

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
        monsterStat.playerDetectionRange = new Vector2(3f, 7f);
        
        monsterStat.damage = monsterData.Monster_Attack;
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
    
    private Vector2 playerDetectionCenter;
    private Vector2 target;

    private void performIdle()
    {
        playerDetectionCenter = Vector2.up * transform.position.y;
        Collider2D[] playerCol = Physics2D.OverlapBoxAll(playerDetectionCenter, monsterStat.playerDetectionRange, 0f,
            LayerMask.GetMask("Player"));

        float mindistance = 10f;
        Transform closestPlayer = null;

        for (int i = 0; i < playerCol.Length; i++)
        {
            float dist = Vector2.Distance(transform.position, playerCol[i].transform.position);

            if (dist < mindistance)
            {
                mindistance = dist;
                closestPlayer = playerCol[i].transform;
            }
        }

        if (closestPlayer != null)
        {
            target = closestPlayer.position;
            ChangeState(EnemyState.Run);
        }
    }
    
    private void performRun()
    {
        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime);
        float distance = Vector3.Distance(transform.position, target);
        
        if (distance < monsterStat.attackRange)
            ChangeState(EnemyState.Attack);
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
    
    public void ChangeState(EnemyState newState)
    {
        prevCharacterState = currentCharacterState;
        currentCharacterState = newState;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(playerDetectionCenter, monsterStat.playerDetectionRange);
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
    public Vector2 playerDetectionRange;
}

public enum EnemyState
{
    Idle,
    Run,
    Attack,
    Die
}
