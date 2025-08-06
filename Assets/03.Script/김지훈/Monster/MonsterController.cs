using System;
using System.Collections;
using System.Collections.Generic;
using _03.Script.엄시형.Monster;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;
public class MonsterController : BaseMonster, IDamageAble
{
    //IDamageAble 요소
    public Collider2D mainCollider => collider2D;
    public GameObject GameObject => gameObject;
    public float Damage => monsterStat.damage;
    
    public float MaxHp => monsterStat.maxHp;
    public float CurrentHp => monsterStat.health;
    public Vector2 playerDetection;

    //Component 받아오는 요소
    [Tooltip("초기에 실행할 때 monsterData로 체력, 공격력등을 받아오는 데이터\n" + "게임 시작할 때 monster의 체력을 올리고 싶다면 여기서 바꾸기")] 
    public MonsterData monsterData;
    [Tooltip("MonsterData 정보를 받아와서 실질적으로 Monster를 관리하는 것.\n" + "게임 도중에 monster의 체력을 올리고 싶다면 여기를 바꾸기.")]
    public MonsterStat monsterStat = new MonsterStat();
    private Collider2D collider2D;
    private Weapon weapon;
    
    //CharacterState 정의 요소
    [SerializeField, Tooltip("현재 EnemyState의 상태")] private EnemyState currentCharacterState = EnemyState.Idle;
    [SerializeField] private EnemyState prevCharacterState = EnemyState.Idle;


    private float attackTimer;
    
    private void Awake()
    {
        TryGetComponent(out collider2D);
        TryGetComponent(out weapon);
        
        monsterStat.playerDetectionRange = playerDetection;
    }

    public void SetMonsterData(MonsterData monsterData)
    {
        monsterStat.maxHp = monsterData.Monster_HP;
        monsterStat.health = monsterData.Monster_HP;
        monsterStat.damage = monsterData.Monster_Attack;
        monsterStat.attackRange = monsterData.attackRange;
        monsterStat.attackSpeed = monsterData.attackSpeed;
        monsterStat.quantity = monsterData.Monster_quantity;
        monsterStat.moveSpeed = monsterData.moveSpeed;
    }
    
    public void SetBossData(MonsterData monsterData)
    {
        monsterStat.maxHp = monsterData.Boss_HP;
        monsterStat.health = monsterData.Boss_HP;
        monsterStat.damage = monsterData.Boss_Attack;
        monsterStat.attackRange = monsterData.attackRange;
        monsterStat.attackSpeed = monsterData.attackSpeed;
        monsterStat.quantity = monsterData.Monster_quantity;
        monsterStat.moveSpeed = monsterData.moveSpeed;
    }
    
    
    private void Start()
    {
        CombatSystem.instance.RegisterMonster(this);
    }

    private void Update()
    {
        
        switch (currentCharacterState)
        {
            case EnemyState.Idle :
                performIdle();
                break;
            case EnemyState.Attack:
                performAttack();
                break;
            case EnemyState.Die :
                performDie();
                break;
        }
        
                
        if (monsterStat.health <= 0)
        {
            ChangeState(EnemyState.Die);
        }
    }
    
    private Vector2 playerDetectionCenter;
    private Transform target;
    private Collider2D[] playerCol;

    //실시간으로 가까운 거리에 있는 Player를 탐색하면서 이동함.
    private void performIdle()
    {
        playerDetectionCenter = Vector2.up * transform.position.y;
        playerCol = Physics2D.OverlapBoxAll(playerDetectionCenter, monsterStat.playerDetectionRange, 0f,
            LayerMask.GetMask("Player"));
        
        float mindistance = 10f;
        Transform closestPlayer = null;

        for (int i = 0; i < playerCol.Length; i++)
        {
            float dist = Vector2.Distance(transform.position, playerCol[i].transform.position);

            if (dist < mindistance && playerCol[i].gameObject.activeSelf)
            {
                mindistance = dist;
                closestPlayer = playerCol[i].transform;
            }
        }

        if (closestPlayer != null)
        {
            target = closestPlayer;

            transform.position = Vector2.Lerp(transform.position, target.position, Time.deltaTime);

            float distance = Vector2.Distance(transform.position, target.position);
            if (distance < monsterStat.attackRange || collider2D.IsTouching(target.GetComponent<Collider2D>()))
                ChangeState(EnemyState.Attack);
        }
    }
    

    private void performAttack()
    {
        attackTimer -= Time.deltaTime;
        if (attackTimer > 0f)
            return;
        
        float distance = Vector2.Distance(transform.position, target.position);

        if (distance > monsterStat.attackRange && collider2D.IsTouching(target.GetComponent<Collider2D>()).Equals(false))
        {
            ChangeState(EnemyState.Idle);
            return;
        }

        if (target.gameObject.activeSelf.Equals(false))
        {
            ChangeState(EnemyState.Idle);
        }
        

        if (target.gameObject.TryGetComponent(out IDamageAble playerDamage))
        {
            CombatEvent combatEvent = new CombatEvent();

            combatEvent.Receiver = playerDamage;
            combatEvent.Sender = this;
            combatEvent.Damage = monsterStat.damage;
            combatEvent.collider = playerDamage.mainCollider;
            
            CombatSystem.instance.AddCombatEvent(combatEvent);
        }
        attackTimer = 1f / monsterStat.attackSpeed;

    }

    private void performDie()
    {
        gameObject.SetActive(false);
    }
    
    
    public void TakeDamage(CombatEvent combatEvent)
    {
        monsterStat.health -= combatEvent.Damage;
        DamgeEvent.OnTriggerMonsterDamageEvent(this);   
        
        if (monsterStat.health <= 0)
        {
            ChangeState(EnemyState.Die);
        }
        //Debug.Log($"{combatEvent.Sender}가 {gameObject.name}에게 피해 줌");
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

    private void OnDisable()
    {
        AliveExistSystem.Instance.RemoveEnemyFromList(collider2D);
    }


    public override MonsterType Type => m_Type;
}
[System.Serializable]
public class MonsterStat
{
    public float maxHp;
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
    Idle, //Idle에서 run하고 다함
    Attack,
    Die
}
