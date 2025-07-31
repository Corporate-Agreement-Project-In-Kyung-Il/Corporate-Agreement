using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeastClaw : ActiveSkillBase, ISkillID
{
    public Sprite SkillSprite { get; set; }
    public Sprite skillSprite;
    public void SetSkillSprite()
    {
        SkillSprite = skillSprite;
    }
    //단일기 3번공격
    public int SkillId;
    public int SkillID { get; set; }

    public int attackCount;
    public float moveSpeed;
    private bool attacking;
    public float destroyTime = 1f;
    float timer = 0;

    public void SetSkillID()
    {
        SkillID = SkillId;
    }

    void Awake()
    {
        Initialize();
        attacking = false;
    }

    void Start()
    {
        SFXManager.Instance.Play(skillSound);
        attackCount = 0;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > destroyTime)
        {
            Destroy(gameObject);
        }

        if (owner.target == null) return;


        Vector2 dir = (owner.target.transform.position - transform.position).normalized;
        float dis = Vector2.Distance(owner.target.transform.position, transform.position);

        transform.position += (Vector3)(dir * (moveSpeed * Time.deltaTime));

        if (dis < 0.2f && attacking == false)
        {
            attacking = true;
            AttackTarget();
        }
    }

    public void AttackTarget()
    {
        if (attackCount >= stat.Attack_Count)
        {
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(DamageDelay());
        }
    }

    IEnumerator DamageDelay()
    {
        if (owner.target.gameObject.TryGetComponent(out IDamageAble enemyDamage) && attackCount < stat.Attack_Count)
        {
            attackCount++;
            CombatEvent combatEvent = new CombatEvent();
            combatEvent.Receiver = enemyDamage;
            combatEvent.Sender = owner;
            combatEvent.Damage = stat.Damage;
            combatEvent.collider = owner.target;

            CombatSystem.instance.AddCombatEvent(combatEvent);
        }

        yield return new WaitForSeconds(0.2f);
        AttackTarget();
    }

    public override void Initialize()
    {
        SetSkillID();
        SetSkillSprite();
        if (owner.skills[0].SkillID == SkillID && owner.skills[0] is ActiveSkillSO skill)
        {
            stat.Damage = skill.Skill_Damage;
            stat.Attack_Count = skill.Skill_Attack_Count;
        }
        else if (owner.skills[1].SkillID == SkillID && owner.skills[1] is ActiveSkillSO skill2)
        {
            stat.Damage = skill2.Skill_Damage;
            stat.Attack_Count = skill2.Skill_Attack_Count;
        }
    }
}