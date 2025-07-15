using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
public enum BuffEffectType
{
    Shield_Protection,
    Steel_Shield,
    Projectile_Hit,
    Archer_Strong_Mind,
    Wizard_Strong_Mind,
    // 나중에 쉽게 추가 가능
}
public class Player : MonoBehaviour, IDamageAble, ICameraPosition, IBuffSelection
{
    private static readonly int IsRun = Animator.StringToHash("isRun");
    private static readonly int IsAttack = Animator.StringToHash("isAttack");
    private static readonly int Attack = Animator.StringToHash("attack");

    // ===== Player_fusion.cs 내용 =====
    public Collider2D mainCollider => col;
    public GameObject GameObject => gameObject;
    public float Damage => playerStat.attackDamage;
    public float CurrentHp => playerStat.health;
    public Transform cameraMoveTransform => gameObject.transform;
    public bool canMove => cameraMove;
    public PlayerStat buffplayerStat
    {
        get => playerStat;
        set => playerStat = value;
    }

    public List<int> SkillID => playerStat.skill_possed;
    public ISkillID[] skills = new ISkillID[2];
    public GameObject skillPrefab;
    public GameObject skillPrefab2;

    [SerializeField, Tooltip("게임 시작할 때 받아오는 초기값이 저장된 곳.\n" + " 초기에 Player의 Stat을 조절하고 싶으면 여기")] 
    public PlayerData data;
    
    [Tooltip("playerStat으로 게임 도중 Stat을 조절하고 싶으면 여기.")]
    public PlayerStat playerStat = new PlayerStat();
    
    private Collider2D col;
    private Rigidbody2D rigid;
    private Animator animator;
    private Weapon weapon2;

    
    [SerializeField, Tooltip("현재의 player의 상태")] private CharacterState currentCharacterState = CharacterState.Run;
    [SerializeField] private CharacterState prevCharacterState = CharacterState.Run;

    private bool cameraMove = true;
    [SerializeField] private Vector2 detectionRange;
    private Vector2 enemyDetectionCenter;
    private Collider2D[] enemyDetectionCol;
    public bool isTarget = false;
    public Collider2D target;
    public float attackRange;
    private float attackTimer = 0f;
    private float[] skillCooldownTimers = new float[2];

    public Dictionary<BuffEffectType, bool> activeBuffs = new();
    private Dictionary<BuffEffectType, float> buffCooldownTimers = new();

    public float shieldBlockChance = 0f;
    public float damageReductionRate = 0f;

    private Vector2 targetPos;

    private void Awake()
    {
        TryGetComponent(out col);
        TryGetComponent(out rigid);
        weapon2 = GetComponentInChildren<Weapon>();
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

        attackRange = playerStat.attackRange;
    }

    private void Start()
    {
        if (weapon2 != null) weapon2.playerAnimator = animator;

        if (skills[0] is ActiveSkillSO skill1)
        {
            skillPrefab = skill1.SkillPrefab;
            if (skillPrefab.TryGetComponent(out ActiveSkillBase activeScript))
            {
                activeScript.owner = this;
            }
        }
        else if (skills[0] is BuffSO buff1)
        {
            skillPrefab = buff1.SkillPrefab;
        }

        if (skills[1] is ActiveSkillSO skill2)
        {
            skillPrefab2 = skill2.SkillPrefab;
            if (skillPrefab2.TryGetComponent(out ActiveSkillBase activeScript))
            {
                activeScript.owner = this;
            }
        }
        else if (skills[1] is BuffSO buff2) skillPrefab2 = buff2.SkillPrefab;

        InputGameManagerSkillID(playerStat.characterClass, playerStat.skill_possed[1]);
    }

    private void Update()
    {
        List<BuffEffectType> keys = new List<BuffEffectType>(buffCooldownTimers.Keys);
        foreach (var key in keys) buffCooldownTimers[key] -= Time.deltaTime;
        skillCooldownTimers[0] -= Time.deltaTime;
        skillCooldownTimers[1] -= Time.deltaTime;

        enemyDetectionCenter = Vector2.up * transform.position.y;
        enemyDetectionCol = Physics2D.OverlapBoxAll(enemyDetectionCenter, playerStat.detectionRange, 0f,
            LayerMask.GetMask("Enemy"));

        switch (currentCharacterState)
        {
            case CharacterState.Run: animator.SetBool(IsRun, true); performRun(); break;
            case CharacterState.Attack: performAttack(); break;
            case CharacterState.Die: performDie(); break;
        }
    }

    private void performRun()
    {
        if (enemyDetectionCol.Length > 0)
        {
            ChangeState(CharacterState.Attack);
        }

        targetPos = rigid.position + Vector2.up * (playerStat.moveSpeed * Time.deltaTime);
        rigid.MovePosition(targetPos);
    }

    private void performDie()
    {
        data.isDead = true;
        gameObject.SetActive(false);
    }
    
    private void performAttack()
    {
        if(enemyDetectionCol.Length <= 0)
        {
            isTarget = false;
            target = null;
            ChangeState(CharacterState.Run);
            return;
        }
        
        attackTimer -= Time.deltaTime;
        
        if (attackTimer > 0f)
            return;
        
        Vector2 boxSize = new Vector2(playerStat.attackRange, playerStat.attackRange);

        if (isTarget.Equals(false))
        {
            Collider2D[] enemyAttackCol = Physics2D.OverlapBoxAll(transform.position, boxSize, 0f, LayerMask.GetMask("Enemy"));
            float minDistance = 100f;
            Collider2D closestEnemy = null;
            for (int i = 0; i < enemyAttackCol.Length; i++)
            {
                if (enemyAttackCol[i] == null) continue;

                float distance = Vector2.Distance(transform.position, enemyAttackCol[i].transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestEnemy = enemyAttackCol[i];
                }
            }
            target = closestEnemy;
        }
        
        if (target != null)
        {
            isTarget = true;
            animator.SetBool(IsAttack, true);
            isTarget = weapon2.Attack(target);
            
            bool isStillTarget = weapon2.Attack(target);
            
            if (HasBuff(BuffEffectType.Archer_Strong_Mind))
            {
                Debug.Log("🏹 아처 스트롱 마인드 발동! 추가 공격");
                weapon2.Attack(target);
            }
            
            isTarget = isStillTarget;
            attackTimer = 1f / playerStat.attackSpeed;
            SkillCondition();
        }

    }

    public void TakeDamage(CombatEvent combatEvent)
    {
        float finalDamage = combatEvent.Damage * (1 - damageReductionRate);
        playerStat.health -= finalDamage;
        DamgeEvent.OnTriggerPlayerDamageEvent(this);
        
        if (playerStat.health <= 0)
        {
            cameraMove = false;
            AliveExistSystem.Instance.RemovePlayerFromList(col);
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
            GameObject prefab = index == 0 ? skillPrefab : skillPrefab2;
            Instantiate(prefab, transform.position, Quaternion.identity);
        }

        if (skills[index] is BuffSO buff)
        {
            GameObject prefab = index == 0 ? skillPrefab : skillPrefab2;
            GameObject buffObj = Instantiate(prefab, transform.position, Quaternion.identity, transform);
            foreach (var comp in buffObj.GetComponents<MonoBehaviour>())
            {
                if (comp is ISkillID skill)
                {
                    if (comp is Shield_Protection shield) shield.Initialize(this, buff);
                    else if (comp is SteelShield steel) steel.Initialize(this, buff);
                    else if (comp is ProjectileHit proj) proj.Initialize(this, buff);
                    else if (comp is Archer_Strong_Mind archer) archer.Initialize(this, buff);
                    else if (comp is WizardStrongMind wizard) wizard.Initialize(this, buff);
                }
            }
        }
    }

    private void ResetCooldown(int index)
    {
        if (skills[index] is ActiveSkillSO active) skillCooldownTimers[index] = active.Skill_Cooldown;
        else if (skills[index] is BuffSO buff) skillCooldownTimers[index] = buff.Skill_Cooldown;
    }

    public bool HasBuff(BuffEffectType buff) => activeBuffs.TryGetValue(buff, out bool isActive) && isActive;
    public void SetBuffState(BuffEffectType buff, bool isActive) => activeBuffs[buff] = isActive;

    private bool CanUseBuff(BuffEffectType type) =>
        !buffCooldownTimers.ContainsKey(type) || buffCooldownTimers[type] <= 0f;

    public void TriggerBuff(BuffSO buff)
    {
        if (!Enum.TryParse(buff.Skill_Buff_Type, out BuffEffectType effect)) return;
        if (HasBuff(effect) || !CanUseBuff(effect)) return;

        SetBuffState(effect, true);
        StartCoroutine(RemoveBuffAfter(buff.Skill_Duration, effect, buff.Skill_Cooldown));
    }

    private IEnumerator RemoveBuffAfter(float duration, BuffEffectType effect, float cooldown)
    {
        yield return new WaitForSeconds(duration);
        SetBuffState(effect, false);
        buffCooldownTimers[effect] = cooldown;
    }

    private void InputGameManagerSkillID(character_class character, int canskillID)
    {
        switch (character)
        {
            case character_class.전사:
                GameManager.Instance.characterSkillID[0] = 100011;
                GameManager.Instance.characterSkillID[1] = canskillID;
                GameManager.Instance.playerStatAdjust.DependencyPlayerStat[0] = buffplayerStat;
                break;
            case character_class.궁수:
                GameManager.Instance.characterSkillID[2] = 100015;
                GameManager.Instance.characterSkillID[3] = canskillID;
                GameManager.Instance.playerStatAdjust.DependencyPlayerStat[1] = buffplayerStat;
                break;
            case character_class.마법사:
                GameManager.Instance.characterSkillID[4] = 100018;
                GameManager.Instance.characterSkillID[5] = canskillID;
                GameManager.Instance.playerStatAdjust.DependencyPlayerStat[2] = buffplayerStat;
                break;
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
