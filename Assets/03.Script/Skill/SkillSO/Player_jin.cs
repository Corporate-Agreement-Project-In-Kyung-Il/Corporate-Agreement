using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_jin : MonoBehaviour, IDamageAble, ICameraPosition
{
    protected static readonly int IsRun = Animator.StringToHash("isRun");
    private static readonly int Attack = Animator.StringToHash("attack");

    //IDamageAble 요소
    public Collider2D mainCollider => col;
    public GameObject GameObject => gameObject;
    public float Damage => playerStat.attackDamage;
    public float CurrentHp => playerStat.health;

    //스킬ID
    public List<int> SkillID => playerStat.skill_possed;
    public ISkillID[] skills = new ISkillID[2];
    public SkillBase[] skillPrefab = new SkillBase[2];
    
    //ICameraPosition 요소 
    public Transform cameraMoveTransform => gameObject.transform;
    public bool canMove => cameraMove;

    //Component 받아오는 요소
    [SerializeField] public PlayerData data;
    private Collider2D col;
    private Rigidbody2D rigid;
    private PlayerStat playerStat = new PlayerStat();
    private Animator animator;
    private Weapon weapon;

    //CharacterState 정의 요소
    [SerializeField] private CharacterState_jin currentCharacterState = CharacterState_jin.Run;
    [SerializeField] private CharacterState_jin prevCharacterState = CharacterState_jin.Run;

    //기타 선언 변수
    private bool cameraMove = true;
    [SerializeField] private Vector2 detectionRange;

    private float[] skillCooldownTimers = new float[2];

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
        /*enemyDetectionCenter = Vector2.up * transform.position.y;
        Collider2D[] enemyDetectionCol = Physics2D.OverlapBoxAll(enemyDetectionCenter, playerStat.detectionRange, 0f, LayerMask.GetMask("Enemy"));

        if (enemyDetectionCol.Length > 0)
            ChangeState(CharacterState_jin.Attack);

        switch (currentCharacterState)
        {
            case CharacterState_jin.Run:
                performRun();
                break;

            case CharacterState_jin.Attack:
                performAttack();
                break;

            case CharacterState_jin.Die:
                performDie();
                break;
        }*/
        skillCooldownTimers[0] -= Time.deltaTime;
        skillCooldownTimers[1] -= Time.deltaTime;
        SkillCondition();
    }

    private void SkillCondition()
    {
        if (skillCooldownTimers[0] <= 0f)
        {
            UseSkill(0);
            ResetCooldown(0);
        }

        if (skillCooldownTimers[1] <= 0f)
        {
            UseSkill(1);
            ResetCooldown(1);
        }
    }

    private void UseSkill(int index)
    {
        if (skills[index] is ActiveSkillSO active)
        {
            Debug.Log($"[액티브] {active.Skill_Name} 발동! 쿨타임: {active.Skill_Cooldown}");
            // 공격/이펙트/범위 등 구현
        }
        else if (skills[index] is BuffSO buff)
        {
            Debug.Log($"[버프] {buff.Skill_Name} 발동! 쿨타임 : {buff.Skill_Cooldown}, 지속시간: {buff.Skill_Duration}초");
            // 스탯 증가 등 버프 효과 적용
        }
    }

    private void ResetCooldown(int index)
    {
        if (skills[index] is ActiveSkillSO active)
            skillCooldownTimers[index] = active.Skill_Cooldown;
        else if (skills[index] is BuffSO buff)
            skillCooldownTimers[index] = buff.Skill_Cooldown;
    }

    private Vector2 targetPos;

    private void performRun()
    {
        /*Debug.Log($"targetPos : {targetPos}");
        animator.SetBool(IsRun, true);
        targetPos = rigid.position + Vector2.up * (playerStat.moveSpeed * Time.deltaTime);
        rigid.MovePosition(targetPos);*/ //Vector3.Lerp(transform.position, targetPos, Time.deltaTime));
    }

    private void performDie()
    {
        // gameObject.SetActive(false);
    }

    public bool isTarget = false;
    public Collider2D target;

    private void performAttack()
    {
        /*Vector2 boxSize = new Vector2(playerStat.attackRange, playerStat.attackRange);

        if (isTarget.Equals(false))
        {
            Collider2D enemyAttackCol = Physics2D.OverlapBox(transform.position, boxSize, 0f, LayerMask.GetMask("Enemy"));
            target = enemyAttackCol;
        }

        if (target != null)
        {
            isTarget = true;
            animator.SetTrigger(Attack);
            isTarget = weapon.Attack(target);
        }*/
    }

    public void UseSkill()
    {
    }

    public void TakeDamage(CombatEvent combatEvent)
    {
        // if (playerStat.health <= 0)
        // {
        //     cameraMove = false;
        //     ChangeState(CharacterState_jin.Die);
        //     return;
        // }
        // Debug.Log($"{gameObject.name}이 데미지를 입음.");
    }

    public void ChangeState(CharacterState_jin newState)
    {
        // prevCharacterState = currentCharacterState;
        // currentCharacterState = newState;
        // animator.SetBool(IsRun, false);
    }

    private void OnDrawGizmos()
    {
        // Gizmos.color = Color.yellow;
        // Gizmos.DrawWireCube(enemyDetectionCenter, playerStat.detectionRange);
        // Gizmos.color = Color.red;
        // Gizmos.DrawWireCube(transform.position, new Vector2(playerStat.attackRange, playerStat.attackRange));
    }
}

public enum CharacterState_jin
{
    Run,
    Attack,
    Die
}