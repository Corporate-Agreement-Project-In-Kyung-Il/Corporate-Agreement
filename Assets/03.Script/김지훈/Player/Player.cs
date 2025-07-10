using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageAble, ICameraPosition, IBuffSelection
{
    private static readonly int IsRun = Animator.StringToHash("isRun");
    private static readonly int IsAttack = Animator.StringToHash("isAttack");


    //IDamageAble 요소
    public Collider2D mainCollider => col;
    public GameObject GameObject => gameObject;
    public float Damage => playerStat.attackDamage;
    public float CurrentHp => playerStat.health;

    public float attackRange;

    
    //ICameraPosition 요소 
    public Transform cameraMoveTransform => gameObject.transform;
    public bool canMove => cameraMove;
    
    //IBuffSelection 요소 
    public PlayerStat buffplayerStat { get; }
    
    //Component 받아오는 요소
    [SerializeField] private PlayerData data;
    private Collider2D col;
    private Rigidbody2D rigid;
    private PlayerStat playerStat = new PlayerStat();
    private Animator animator;
    private Weapon weapon;
        
    //CharacterState 정의 요소
    [SerializeField] private CharacterState currentCharacterState = CharacterState.Run;
    [SerializeField] private CharacterState prevCharacterState = CharacterState.Run;
    
    //기타 선언 변수
    [SerializeField] private bool cameraMove = true;
    [SerializeField] private Vector2 detectionRange; 

    
    private void Awake()
    {
        TryGetComponent(out col);
        TryGetComponent(out rigid);
        weapon = GetComponentInChildren<Weapon>();
        animator = GetComponentInChildren<Animator>();
        
        
        playerStat.health = data.health;
        playerStat.moveSpeed = data.moveSpeed;
        
        playerStat.character_ID = data.character_ID;
        
        playerStat.characterClass = data.characterClass;
        playerStat.characterName = data.characterName;
        playerStat.characterGrade = data.characterGrade;
        
        playerStat.attackDamage = data.attackDamage;
        playerStat.attackSpeed = data.attackSpeed;
        playerStat.attackRange = data.attackRange;

        playerStat.criticalProbability = data.criticalProbability;
        playerStat.detectionRange = detectionRange;
        
        playerStat.training_type = data.training_type;
        playerStat.equip_item = data.equip_item;
        playerStat.skill_possed = data.skill_possed;
        
        
        attackRange = playerStat.attackRange; //궁수의 직선 공격 범위를 위해서
        
        
    }

    private void Start()
    {
        weapon.playerAnimator = animator;
        InputGameManagerSkillID(playerStat.characterClass ,playerStat.skill_possed[1]);
    }

    private Vector2 enemyDetectionCenter;
    private Collider2D[] enemyDetectionCol;
    private void Update()
    {
        
        enemyDetectionCenter = Vector2.up * transform.position.y;
        enemyDetectionCol = Physics2D.OverlapBoxAll(enemyDetectionCenter, playerStat.detectionRange, 0f, LayerMask.GetMask("Enemy"));

        switch (currentCharacterState)
        {
            case CharacterState.Run:
                animator.SetBool(IsRun, true);
                performRun();
                break;
            
            case CharacterState.Attack:
                performAttack();
                break;
            
            case CharacterState.Die:
                performDie();
                break;
        }

       
    }
    
    private Vector2 targetPos;

    private void performRun()
    {
        if (enemyDetectionCol.Length > 0)
        {
            ChangeState(CharacterState.Attack);
        }
        
        //Debug.Log($"targetPos : {targetPos}");
        targetPos = rigid.position + Vector2.up * (playerStat.moveSpeed * Time.deltaTime);
        rigid.MovePosition(targetPos); //Vector3.Lerp(transform.position, targetPos, Time.deltaTime));
    }
    
    private void performDie()
    {
        gameObject.SetActive(false);
    }

    public bool isTarget = false;
    public Collider2D target;
    private float attackTimer = 0f;
    
    private void performAttack()
    {
        if(enemyDetectionCol.Length <= 0)
        {
            isTarget = false;
            ChangeState(CharacterState.Run);
        }
        
        attackTimer -= Time.deltaTime;
        
        if (attackTimer > 0f)
            return;
        
        Vector2 boxSize = new Vector2(playerStat.attackRange, playerStat.attackRange);

        if (isTarget.Equals(false))
        {
            Collider2D enemyAttackCol = Physics2D.OverlapBox(transform.position, boxSize, 0f, LayerMask.GetMask("Enemy"));
            target = enemyAttackCol;
        }
        
        if (target != null)
        {
            isTarget = true;
            animator.SetBool(IsAttack, true);
            isTarget = weapon.Attack(target);
            
            attackTimer = 1f / playerStat.attackSpeed;
        }

    }
    
    public void TakeDamage(CombatEvent combatEvent)
    {
        playerStat.health -= combatEvent.Damage * 0.6f ;
        
        if (playerStat.health <= 0)
        {
            cameraMove = false;
            ChangeState(CharacterState.Die);
            return;
        }
        
       // Debug.Log($"{gameObject.name}이 데미지를 입음.");
    }

    public void ChangeState(CharacterState newState)
    {
        animator.SetBool(IsRun, false);
        animator.SetBool(IsAttack, false);
        prevCharacterState = currentCharacterState;
        currentCharacterState = newState;
    }

    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(enemyDetectionCenter, playerStat.detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector2(playerStat.attackRange, playerStat.attackRange));
    }

    private void InputGameManagerSkillID(character_class character, int canskillID)
    {
        switch (character)
        {
            case character_class.전사 :
                GameManagerJiHun.Instance.characterID[0] = canskillID;
                break;
            case character_class.궁수 :
                GameManagerJiHun.Instance.characterID[1] = canskillID;
                break;
            case character_class.마법사 :
                GameManagerJiHun.Instance.characterID[2] = canskillID;
                break;
        }
    }
}

public enum CharacterState
{
    Run,
    Attack,
    Die
}
