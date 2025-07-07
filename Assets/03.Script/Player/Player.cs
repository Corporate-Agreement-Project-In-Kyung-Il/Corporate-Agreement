using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour, IDamageAble, ICameraPosition
{
    protected static readonly int IsRun = Animator.StringToHash("isRun");

    //IDamageAble 요소
    public Collider2D mainCollider => col;
    public GameObject GameObject => gameObject;
    public float Damage => playerStat.attackDamage;
    
    //ICameraPosition 요소 
    public Transform cameraMoveTransform => gameObject.transform;
    public bool canMove => cameraMove;
    
    //Component 받아오는 요소
    [SerializeField] protected PlayerData data;
    protected Collider2D col;
    protected PlayerStat playerStat = new PlayerStat();
    protected Animator animator;
    protected WeaponActive weapon;
        
    //CharacterState 정의 요소
    [SerializeField] private CharacterState currentCharacterState = CharacterState.Run;
    [SerializeField] private CharacterState prevCharacterState = CharacterState.Run;
    
    //기타 선언 변수
    protected bool cameraMove = true;
    [SerializeField] protected Vector2 detectionRange; 

    
    protected void Awake()
    {
        TryGetComponent(out col);
        weapon = GetComponentInChildren<WeaponActive>();
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
    }
    
    protected Vector2 enemyDetectionCenter;
    
    protected void Update()
    {
        enemyDetectionCenter = Vector2.up * transform.position.y;
        Collider2D enemyDetectionCol = Physics2D.OverlapBox(enemyDetectionCenter, playerStat.detectionRange, 0f, LayerMask.GetMask("Enemy"));

        if (enemyDetectionCol != null) 
        {
            ChangeState(CharacterState.Attack);
        }

        switch (currentCharacterState)
        {
            case CharacterState.Run:
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
    
    protected Vector3 targetPos;
    protected void performRun()
    {
        targetPos = transform.position + Vector3.up * playerStat.moveSpeed;
        animator.SetBool(IsRun, true);
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime);
    }
    
    protected void performDie()
    {
        
    }
    
    protected abstract void performAttack();
    
    public void TakeDamage(CombatEvent combatEvent)
    {
        if (playerStat.health <= 0)
        {
            cameraMove = false;
            ChangeState(CharacterState.Die);
            return;
        }
    }

    public void ChangeState(CharacterState newState)
    {
        prevCharacterState = currentCharacterState;
        currentCharacterState = newState;
        animator.SetBool(IsRun, false);
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(enemyDetectionCenter, playerStat.detectionRange);
    }
}

public enum CharacterState
{
    Run,
    Attack,
    Die
}
