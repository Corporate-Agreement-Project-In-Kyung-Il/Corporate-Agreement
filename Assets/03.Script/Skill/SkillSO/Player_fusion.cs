using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class Player_fusion : MonoBehaviour, IDamageAble, ICameraPosition, IBuffSelection
{
    // Animator Hash
    private static readonly int IsRun = Animator.StringToHash("isRun");
    private static readonly int IsAttack = Animator.StringToHash("isAttack");
    private static readonly int Attack = Animator.StringToHash("attack");

    // Interfaces
    public Collider2D mainCollider => col;
    public GameObject GameObject => gameObject;
    public float Damage => playerStat.attackDamage;
    public float CurrentHp => playerStat.health;
    public Transform cameraMoveTransform => gameObject.transform;
    public bool canMove => cameraMove;
    public PlayerStat buffplayerStat => playerStat;

    // Components
    [SerializeField] private PlayerData data;
    private Collider2D col;
    private Rigidbody2D rigid;
    private Animator animator;
    private Weapon weapon;
    private PlayerStat playerStat = new PlayerStat();

    // States
    [SerializeField] private CharacterState currentCharacterState = CharacterState.Run;
    [SerializeField] private CharacterState prevCharacterState = CharacterState.Run;
    [SerializeField] private bool cameraMove = true;

    // Skills
    public List<int> SkillID => playerStat.skill_possed;
    public ISkillID[] skills = new ISkillID[2];
    public GameObject skillPrefab;
    public GameObject skillPrefab2;
    private float[] skillCooldownTimers = new float[2];

    // Detection
    [SerializeField] private Vector2 detectionRange;
    private Vector2 enemyDetectionCenter;
    private Collider2D[] enemyDetectionCol;
    public bool isTarget = false;
    public Collider2D target;
    public float attackRange;
    private float attackTimer = 0f;

    // Buff
    private Dictionary<BuffEffectType, bool> activeBuffs = new();
    private Dictionary<BuffEffectType, float> buffCooldownTimers = new();
    private float shieldBlockChance = 0f;
    private float damageReductionRate = 0f;

    private void Awake()
    {
        TryGetComponent(out col);
        TryGetComponent(out rigid);
        weapon = GetComponentInChildren<Weapon>();
        animator = GetComponentInChildren<Animator>();

        // PlayerStat 초기화
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

        attackRange = playerStat.attackRange;
    }

    private void Start()
    {
        weapon.playerAnimator = animator;

        if (skills[0] is ActiveSkillSO skill1) skillPrefab = skill1.SkillPrefab;
        else if (skills[0] is BuffSO buff1) skillPrefab = buff1.SkillPrefab;

        if (skills[1] is ActiveSkillSO skill2) skillPrefab2 = skill2.SkillPrefab;
        else if (skills[1] is BuffSO buff2) skillPrefab2 = buff2.SkillPrefab;

        InputGameManagerSkillID(playerStat.characterClass, playerStat.skill_possed[1]);
    }

    private void Update()
    {
        // 버프 쿨타임 감소
        List<BuffEffectType> keys = new List<BuffEffectType>(buffCooldownTimers.Keys);
        foreach (var key in keys) buffCooldownTimers[key] -= Time.deltaTime;

        // 스킬 쿨타임 감소 및 발동
        skillCooldownTimers[0] -= Time.deltaTime;
        skillCooldownTimers[1] -= Time.deltaTime;
        SkillCondition();

        // 적 탐지
        enemyDetectionCenter = Vector2.up * transform.position.y;
        enemyDetectionCol = Physics2D.OverlapBoxAll(enemyDetectionCenter, playerStat.detectionRange, 0f, LayerMask.GetMask("Enemy"));

        // 상태 처리
        switch (currentCharacterState)
        {
            case CharacterState.Run: animator.SetBool(IsRun, true); performRun(); break;
            case CharacterState.Attack: performAttack(); break;
            case CharacterState.Die: performDie(); break;
        }
    }
    private Vector2 targetPos;
    private void performRun()
    {
        if (enemyDetectionCol.Length > 0) ChangeState(CharacterState.Attack);
        targetPos = rigid.position + Vector2.up * (playerStat.moveSpeed * Time.deltaTime);
        rigid.MovePosition(targetPos);
    }

    private void performDie()
    {
        gameObject.SetActive(false);
    }

    private void performAttack()
    {
        if (enemyDetectionCol.Length <= 0)
        {
            isTarget = false;
            ChangeState(CharacterState.Run);
            return;
        }

        attackTimer -= Time.deltaTime;
        if (attackTimer > 0f) return;

        Vector2 boxSize = new Vector2(playerStat.attackRange, playerStat.attackRange);
        if (!isTarget)
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
        float finalDamage = combatEvent.Damage * (1 - damageReductionRate);
        playerStat.health -= finalDamage;

        if (playerStat.health <= 0)
        {
            cameraMove = false;
            ChangeState(CharacterState.Die);
            return;
        }

        if (Random.value < shieldBlockChance)
        {
            Debug.Log("🛡️ 공격 무효화됨!");
            return;
        }

        playerStat.health -= combatEvent.Damage;
        if (playerStat.health <= 0)
        {
            cameraMove = false;
            ChangeState(CharacterState.Die);
        }
    }

    public void ChangeState(CharacterState newState)
    {
        animator.SetBool(IsRun, false);
        animator.SetBool(IsAttack, false);
        prevCharacterState = currentCharacterState;
        currentCharacterState = newState;
    }

    private void SkillCondition()
    {
        if (skillCooldownTimers[0] <= 0f) { UseSkill(0); ResetCooldown(0); }
        if (skillCooldownTimers[1] <= 0f) { UseSkill(1); ResetCooldown(1); }
    }

    private void UseSkill(int index)
    {
        if (skills[index] is ActiveSkillSO active)
        {
            Debug.Log($"[액티브] {active.Skill_Name} 발동! 쿨타임: {active.Skill_Cooldown}");
            Instantiate(skillPrefab);
        }
        else if (skills[index] is BuffSO buff)
        {
            Instantiate(skillPrefab2);
            TriggerBuff(buff);
        }
    }

    private void ResetCooldown(int index)
    {
        if (skills[index] is ActiveSkillSO active) skillCooldownTimers[index] = active.Skill_Cooldown;
        else if (skills[index] is BuffSO buff) skillCooldownTimers[index] = buff.Skill_Cooldown;
    }

    private void InputGameManagerSkillID(character_class character, int canskillID)
    {
        switch (character)
        {
            case character_class.전사: GameManagerJiHun.Instance.characterID[0] = canskillID; break;
            case character_class.궁수: GameManagerJiHun.Instance.characterID[1] = canskillID; break;
            case character_class.마법사: GameManagerJiHun.Instance.characterID[2] = canskillID; break;
        }
    }

    // ─── Buff Logic ─────────────────────────────────────────────────────────────
    public void SetDamageReductionRate(float rate)
    {
        damageReductionRate = rate;
        Debug.Log($"🛡️ 데미지 경감률 설정됨: {rate * 100}%");
    }

    public float GetDamageReductionRate() => damageReductionRate;

    public void SetShieldBlockChance(float chance)
    {
        shieldBlockChance = chance;
        Debug.Log($"🛡️ 방어 확률 설정됨: {chance * 100}%");
    }

    public void SetAttackSpeed(float newSpeed)
    {
        playerStat.attackSpeed = newSpeed;
        Debug.Log($"공격 속도 변경됨: {newSpeed}");
    }

    public void SetAttackDamage(float newDamage)
    {
        playerStat.attackDamage = newDamage;
        Debug.Log($"공격력 변경됨: {newDamage}");
    }

    public bool HasBuff(BuffEffectType buff) => activeBuffs.TryGetValue(buff, out bool isActive) && isActive;

    public void SetBuffState(BuffEffectType buff, bool isActive) => activeBuffs[buff] = isActive;

    private bool CanUseBuff(BuffEffectType type)
    {
        return !buffCooldownTimers.ContainsKey(type) || buffCooldownTimers[type] <= 0f;
    }

    public void TriggerBuff(BuffSO buff)
    {
        if (!Enum.TryParse(buff.Skill_Buff_Type, out BuffEffectType effect))
        {
            Debug.LogWarning($"[TriggerBuff] BuffEffectType 파싱 실패: {buff.Skill_Buff_Type}");
            return;
        }

        if (!CanUseBuff(effect))
        {
            Debug.Log($"❌ {effect} 쿨타임 중: {buffCooldownTimers[effect]:F2}s 남음");
            return;
        }

        Debug.Log($"✅ 버프 발동: {effect}");
        SetBuffState(effect, true);
        StartCoroutine(RemoveBuffAfter(buff.Skill_Duration, effect));
        buffCooldownTimers[effect] = buff.Skill_Cooldown;
    }

    private IEnumerator RemoveBuffAfter(float duration, BuffEffectType effect)
    {
        yield return new WaitForSeconds(duration);
        SetBuffState(effect, false);
        Debug.Log($"버프 종료됨: {effect}");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(enemyDetectionCenter, playerStat.detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector2(playerStat.attackRange, playerStat.attackRange));
    }
}

