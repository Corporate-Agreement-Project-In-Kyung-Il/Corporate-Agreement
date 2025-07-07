using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageAble, ICameraPosition
{
    protected static readonly int IsRun = Animator.StringToHash("isRun");
    private static readonly int Attack = Animator.StringToHash("attack");

    //IDamageAble 요소
    public Collider2D mainCollider => col;
    public GameObject GameObject => gameObject;
    public float Damage => playerStat.attackDamage;
    
    //ICameraPosition 요소 
    public Transform cameraMoveTransform => gameObject.transform;
    public bool canMove => cameraMove;
    
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
    private bool cameraMove = true;
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
    }
    
    private Vector2 enemyDetectionCenter;
    
    private void Update()
    {
        enemyDetectionCenter = Vector2.up * transform.position.y;
        Collider2D[] enemyDetectionCol = Physics2D.OverlapBoxAll(enemyDetectionCenter, playerStat.detectionRange, 0f, LayerMask.GetMask("Enemy"));
        
        if (enemyDetectionCol.Length > 0) 
            ChangeState(CharacterState.Attack);

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
    
    private Vector2 targetPos;

    private void performRun()
    {
        Debug.Log($"targetPos : {targetPos}");
        animator.SetBool(IsRun, true);
        targetPos = rigid.position + Vector2.up * (playerStat.moveSpeed * Time.deltaTime);
        rigid.MovePosition(targetPos); //Vector3.Lerp(transform.position, targetPos, Time.deltaTime));
    }
    
    private void performDie()
    {

    }

    private void performAttack()
    {
        Vector2 boxSize = new Vector2(playerStat.attackRange, playerStat.attackRange);
        Collider2D[] enemyAttackRange =
            Physics2D.OverlapBoxAll(transform.position, boxSize, 0f, LayerMask.GetMask("Enemy"));
        animator.SetTrigger(Attack);
        
    }
    
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            rigid.velocity = Vector2.zero;  // 강제로 속도 제거
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(enemyDetectionCenter, playerStat.detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector2(playerStat.attackRange, playerStat.attackRange));
    }
    
}

public enum CharacterState
{
    Run,
    Attack,
    Die
}
