using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeastClaw : ActiveSkillBase, ISkillID
{
    //단일기 3번공격
    public int SkillId;
    public int SkillID { get; set; }

    public int attackCount;

    public void SetSkillID()
    {
        SkillID = SkillId;
    }

    void Awake()
    {
        Initialize();
    }

    void Start()
    {
        attackCount = 0;
        
        AttackTarget();
    }

    public void AttackTarget()
    {
        if (attackCount >= stat.Attack_Count)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            if (owner.target.gameObject.TryGetComponent(out IDamageAble enemyDamage))
            {
                attackCount++;
                CombatEvent combatEvent = new CombatEvent();
                combatEvent.Receiver = enemyDamage;
                combatEvent.Sender = owner;
                combatEvent.Damage = stat.Damage;
                combatEvent.collider = owner.target;

                CombatSystem.instance.AddCombatEvent(combatEvent);

                Debug.Log("야수의 발톱 공격!");
            }
            AttackTarget();
        }
    }

    public override void Initialize()
    {
        SetSkillID();
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