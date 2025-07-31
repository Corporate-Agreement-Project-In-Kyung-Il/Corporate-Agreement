using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WarriorStrongMind : ActiveSkillBase, ISkillID
{
    public Sprite SkillSprite { get; set; }
    public Sprite skillSprite;
    public void SetSkillSprite()
    {
        SkillSprite = skillSprite;
    }
    //단일 공격 3번때림 
    public int SkillId;
    public int SkillID { get; set; }

    public float destroyTime = 1f;
    float timer = 0;

    public void SetSkillID()
    {
        SkillID = SkillId;
    }

    public int attackCount;

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        attackCount = 0;
        SFXManager.Instance.Play(skillSound);
        AttackTarget();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > destroyTime)
        {
            Destroy(gameObject);
        }

        if (owner.target == null) return;

        transform.position = owner.target.transform.position;
    }

    public void AttackTarget()
    {
        if (attackCount >= stat.Attack_Count||owner.target==null)
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

        if (owner.target.gameObject.TryGetComponent(out IDamageAble enemyDamage) && owner.target != null)
        {
            attackCount++;
            CombatEvent combatEvent = new CombatEvent();
            combatEvent.Receiver = enemyDamage;
            combatEvent.Sender = owner;
            combatEvent.Damage = stat.Damage;
            combatEvent.collider = owner.target;

            CombatSystem.instance.AddCombatEvent(combatEvent);
        }

        yield return new WaitForSeconds(0.3f);
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