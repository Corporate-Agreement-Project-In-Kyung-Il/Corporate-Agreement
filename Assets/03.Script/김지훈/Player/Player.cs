using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

public class Player : MonoBehaviour, IDamageAble, IBuffSelection, ISpriteSelection
{
    private static readonly int IsRun = Animator.StringToHash("isRun");
    private static readonly int IsAttack = Animator.StringToHash("isAttack");
    private static readonly int Attack = Animator.StringToHash("attack");

    // ===== Player_fusion.cs 내용 =====
    public Collider2D mainCollider => col;
    public GameObject GameObject => gameObject;
    public float Damage => playerStat.attackDamage;
    public float CurrentHp => playerStat.health;

    /// <summary>
    ///  ISpriteSelection의 Sprite
    /// </summary>
    public Sprite PlayerSprite => data.playerUISprite;
    public Sprite WeaponSprite => WEAPON.CurrentSprite;
    public Sprite Skill1Icon => PlayerSkill1Icon;
    public Sprite Skill2Icon => PlayerSkill2Icon;
    public string Skill1Name { get; set; }
    public string Skill2Name { get; set; }

    public Sprite PlayerSkill1Icon;
    public Sprite PlayerSkill2Icon;

    /// <summary>
    /// BuffPlayerStat
    /// </summary>
    public PlayerStat buffplayerStat
    {
        get => playerStat;
        set => playerStat = value;
    }

    public Weapon WEAPON => weapon2;

    public List<int> SkillID => playerStat.skill_possed;
    public ISkillID[] skills = new ISkillID[2];
    public GameObject skillPrefab;
    public GameObject skillPrefab2;

    public float[] MaxskillCoolTimer => MaxskillCooldownTimers;
    public float[] CurrentCoolTimer => currentSkillCooldownTimers;

    [Header("버프 아이콘 크기")] 
    public float buffsize_x;
    public float buffsize_y;
    public float buffsize_z;
    
    [Header("버프 아이콘 위치")] 
    public float buff1position_x;
    public float buff1position_y;
    public float buff1position_z;
    public float buff2position_x;
    public float buff2position_y;
    public float buff2position_z;
    
    
    [SerializeField, Header("게임 시작할 때 받아오는 초기값이 저장된 곳."),
     Tooltip("게임 시작할 때 받아오는 초기값이 저장된 곳.\n" + " 초기에 Player의 Stat을 조절하고 싶으면 여기")]
    public PlayerData data;

    [Header("playerStat으로 게임 도중 Stat을 조절하고 싶으면 여기."), Tooltip("playerStat으로 게임 도중 Stat을 조절하고 싶으면 여기.")]
    private PlayerStat playerStat = new PlayerStat();

    public float resetHp; 
    private Collider2D col;
    private Rigidbody2D rigid;
    private Animator animator;
    private Weapon weapon2;


    [SerializeField, Header("현재의 player의 상태."), Tooltip("현재의 player의 상태")]
    private CharacterState currentCharacterState = CharacterState.Run;

    [SerializeField] private CharacterState prevCharacterState = CharacterState.Run;

    private bool cameraMove = true;
    [SerializeField] private Vector2 detectionRange;
    private Vector2 enemyDetectionCenter;
    private Collider2D[] enemyDetectionCol;
    public bool isTarget = false;
    public Collider2D target;
    public float attackRange;
    private float attackTimer = 0f;
    private float[] MaxskillCooldownTimers = new float[2];
    private float[] currentSkillCooldownTimers = new float[2];

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

        initialSetPlayerStats(data);
    }

    private void Start()
    {
        if (weapon2 != null)
        {
            weapon2.playerAnimator = animator;
        }

        if (skills[0] is ActiveSkillSO skill1)
        {
            skillPrefab = skill1.SkillPrefab;
            if (skillPrefab.TryGetComponent(out ActiveSkillBase activeScript))
            {
                activeScript.owner = this;
                PlayerSkill1Icon=activeScript.SkillIcon;
                Skill1Name = activeScript.SkillName;
            }
        }
        else if (skills[0] is BuffSO buff1)
        {
            skillPrefab = buff1.SkillPrefab;
            if (skillPrefab.TryGetComponent(out BuffBase buffScript))
            {
                PlayerSkill1Icon = buffScript.SkillIcon;
                Skill1Name = buffScript.SkillName;
            }
        }

        if (skills[1] is ActiveSkillSO skill2)
        {
            skillPrefab2 = skill2.SkillPrefab;
            if (skillPrefab2.TryGetComponent(out ActiveSkillBase activeScript))
            {
                activeScript.owner = this;
                PlayerSkill2Icon=activeScript.SkillIcon;
                Skill2Name = activeScript.SkillName;
            }
        }
        else if (skills[1] is BuffSO buff2)
        {
            skillPrefab2 = buff2.SkillPrefab;
            if (skillPrefab2.TryGetComponent(out BuffBase buffScript))
            {
                PlayerSkill2Icon=buffScript.SkillIcon;
                Skill2Name = buffScript.SkillName;
            }
        }

        InputGameManagerSkillID(playerStat.characterClass, playerStat.skill_possed[1]);
    }

    private void Update()
    {
        List<BuffEffectType> keys = new List<BuffEffectType>(buffCooldownTimers.Keys);
        foreach (var key in keys) buffCooldownTimers[key] -= Time.deltaTime;
        currentSkillCooldownTimers[0] -= Time.deltaTime;
        currentSkillCooldownTimers[1] -= Time.deltaTime;

        enemyDetectionCenter = Vector2.right * 0.5f + Vector2.up * transform.position.y;

        if (rigid.velocity != Vector2.zero)
        {
            rigid.velocity = Vector2.zero;
        }

        switch (currentCharacterState)
        {
            case CharacterState.Run:
                animator.SetBool(IsRun, true);
                break;
            case CharacterState.Attack:
                performAttack();
                break;
            case CharacterState.Die:
                performDie();
                break;
        }
    }

    private void FixedUpdate()
    {
        enemyDetectionCol = Physics2D.OverlapBoxAll(enemyDetectionCenter, playerStat.detectionRange, 0f,
            LayerMask.GetMask("Enemy"));
        if (currentCharacterState.Equals(CharacterState.Run))
        {
            performRun();
        }
    }

    private void performRun()
    {
        rigid.velocity = Vector3.zero;

        if (enemyDetectionCol.Length.Equals(0))
        {
            targetPos = rigid.position + Vector2.up * (playerStat.moveSpeed * Time.fixedDeltaTime);
            rigid.MovePosition(targetPos);
            return;
        }

        float minDistance = 100f;
        Collider2D closestEnemy = null;

        for (int i = 0; i < enemyDetectionCol.Length; i++)
        {
            if (enemyDetectionCol[i] == null) continue;

            float distance = Vector2.Distance(transform.position, enemyDetectionCol[i].transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = enemyDetectionCol[i];
            }
        }

        target = closestEnemy;

        if (Vector3.Distance(transform.position, target.transform.position) < playerStat.attackRange ||
            target.IsTouching(col))
        {
            attackTimer = 1f / playerStat.attackSpeed;
            ChangeState(CharacterState.Attack);
        }
        else
        {
            Vector2 dir = ((Vector2)target.transform.position - rigid.position).normalized;
            Vector2 nextPos = rigid.position + dir * (playerStat.moveSpeed * Time.fixedDeltaTime);
            rigid.MovePosition(nextPos);
        }
    }

    private void performDie()
    {
        data.isDead = true;
        gameObject.SetActive(false);
        AliveExistSystem.Instance.RemovePlayerFromList(col);
    }

    private void performAttack()
    {
        if (enemyDetectionCol.Length <= 0 || target == null)
        {
            isTarget = false;
            target = null;
            ChangeState(CharacterState.Run);
            return;
        }

        //보스몹을 거리 때문에 인식 못하는데 OverlapBoxnonAlloc으로 하기보다는 거리로 재고 나서
        //만약 보스몹이 한 Stage 2마리 넣거나 큰 몹을 넣는다면 overlapBoxnonAlloc으로 수정
        float distance = Vector2.Distance(transform.position, target.transform.position);

        if (distance > playerStat.attackRange)
        {
            if (enemyDetectionCol.Length > 0)
            {
                target = enemyDetectionCol[0];
                animator.SetBool(IsAttack, true);
            }
            else
            {
                target = null;
                return;
            }
        }
        else
        {
            animator.SetBool(IsAttack, true);
        }

        attackTimer -= Time.deltaTime;

        if (attackTimer > 0f)
            return;

        isTarget = true;
        isTarget = weapon2.Attack(target);

        //bool isStillTarget = weapon2.Attack(target);

        if (HasBuff(BuffEffectType.Archer_Strong_Mind))
        {
            Debug.Log("🏹 아처 스트롱 마인드 발동! 추가 공격");
            weapon2.isSkill = true;
            weapon2.Attack(target);
        }

       // isTarget = isStillTarget;
        attackTimer = 1f / playerStat.attackSpeed;
        SkillCondition();
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
        if (currentSkillCooldownTimers[0] <= 0f)
        {
            UseSkill(0);
            ResetCooldown(0);
        }

        if (currentSkillCooldownTimers[1] <= 0f)
        {
            UseSkill(1);
            ResetCooldown(1);
        }
    }

    private void UseSkill(int index)
    {
        if (skills[index] is ActiveSkillSO active)
        {
            GameObject prefab = index == 0 ? skillPrefab : skillPrefab2;
            Instantiate(prefab, transform.position, prefab.transform.rotation);
        }

        if (skills[index] is BuffSO buff)
        {
            // 위치 오프셋 설정
            Vector3 spawnOffset = Vector3.zero;
            if (index == 0) spawnOffset = new Vector3(buff1position_x, buff1position_y, buff1position_z); // 왼쪽
            if (index == 1) spawnOffset = new Vector3(buff2position_x, buff2position_y, buff2position_z);  // 오른쪽

            // 프리팹 선택 및 생성
            GameObject prefab = index == 0 ? skillPrefab : skillPrefab2;
            GameObject buffObj = Instantiate(prefab, transform.position + spawnOffset, Quaternion.identity, transform);

            // 스케일 조정
            buffObj.transform.localScale = new Vector3(buffsize_x,buffsize_y,buffsize_y);
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
        if (skills[index] is ActiveSkillSO active)
            MaxskillCooldownTimers[index] = active.Skill_Cooldown;
        else if (skills[index] is BuffSO buff)
            MaxskillCooldownTimers[index] = buff.Skill_Cooldown;

        currentSkillCooldownTimers[index] = MaxskillCooldownTimers[index]; // 수정: 해당 index만 초기화
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


    public void ResetPlayerStats()
    {
        Debug.Log("체력 회복 완료");
        resetHp = data.health;
        playerStat.health = resetHp;
        currentCharacterState = CharacterState.Run;
       
        DamgeEvent.OnTriggerPlayerDamageEvent(this);
    }

    public void initialSetPlayerStats(PlayerData initialData)
    {
        playerStat.health = initialData.health;
        playerStat.moveSpeed = initialData.moveSpeed;
        playerStat.character_ID = initialData.character_ID;
        playerStat.characterClass = initialData.characterClass;
        playerStat.characterName = initialData.characterName;
        playerStat.characterGrade = initialData.characterGrade;
        playerStat.attackDamage = initialData.attackDamage;
        playerStat.attackSpeed = initialData.attackSpeed;
        playerStat.attackRange = initialData.attackRange;
        playerStat.criticalProbability = initialData.criticalProbability;
        playerStat.detectionRange = detectionRange;
        playerStat.training_type = initialData.training_type;
        playerStat.equip_item = initialData.equip_item;
        playerStat.skill_possed = initialData.skill_possed;
        resetHp = playerStat.health;
        attackRange = playerStat.attackRange;
        playerStat.equip_level = initialData.equip_level;
        playerStat.training_level = initialData.training_level;
    }


}

public enum CharacterState
{
    Run,
    Attack,
    Die
}