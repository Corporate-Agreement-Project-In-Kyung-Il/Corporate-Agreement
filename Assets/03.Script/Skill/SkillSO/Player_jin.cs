using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffEffectType
{
    Shield_Protection,
    Steel_Shield,
    Projectile_Hit,
    Archer_Strong_Mind,
    Wizard_Strong_Mind,
    // 나중에 쉽게 추가 가능
}

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
    public GameObject skillPrefab;
    public GameObject skillPrefab2;

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

    private void Start()
    {
        if (skills[0] is ActiveSkillSO skill)
        {
            skillPrefab = skill.SkillPrefab;
        }
        else if (skills[0] is BuffSO buff)
        {
            skillPrefab = buff.SkillPrefab;
        }

        if (skills[1] is ActiveSkillSO skill2)
        {
            skillPrefab2 = skill2.SkillPrefab;
        }
        else if (skills[1] is BuffSO buff2)
        {
            skillPrefab2 = buff2.SkillPrefab;
        }
    }

    private Vector2 enemyDetectionCenter;

    private void Update()
    {
        List<BuffEffectType> keys = new List<BuffEffectType>(buffCooldownTimers.Keys);

        foreach (var key in keys)
        {
            buffCooldownTimers[key] -= Time.deltaTime;
            Debug.Log(buffCooldownTimers[key]);
        }
        

        skillCooldownTimers[0] -= Time.deltaTime;
        skillCooldownTimers[1] -= Time.deltaTime;

        SkillCondition();
    }
    //-----------------------------버프--------------------------------------------------
    private Dictionary<BuffEffectType, bool> activeBuffs = new();
    private Dictionary<BuffEffectType, float> buffCooldownTimers = new();

    public float GetAttackSpeed()
    {
        return playerStat.attackSpeed;
    }

    public void SetAttackSpeed(float newSpeed)
    {
        playerStat.attackSpeed = newSpeed;
        Debug.Log($"공격 속도 변경됨: {newSpeed}");
    }
    public bool HasBuff(BuffEffectType buff)
    {
        return activeBuffs.TryGetValue(buff, out bool isActive) && isActive;
    }

    public void SetBuffState(BuffEffectType buff, bool isActive)
    {
        activeBuffs[buff] = isActive;
    }

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

    public void SetAttackDamage(float newDamage)
    {
        playerStat.attackDamage = newDamage;
        Debug.Log($"공격력 변경됨: {newDamage}");
    }

    //-----------------------------버프--------------------------------------------------
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
            Instantiate(skillPrefab2);
            
            // 공격/이펙트/범위 등 구현
        }
        else if (skills[index] is BuffSO buff)
        {
            Instantiate(skillPrefab);
            TriggerBuff(buff); // 쿨타임 체크 + 버프 적용 + 지속시간 관리
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