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

    void Start()
    {
        attackCount = 0;
        Debug.Log("start BeastClaw");
        Initialize();
        AttackTarget();
    }

    private void AttackTarget()
    {
        if (attackCount >= stat.Attack_Count) return;

        //owner.target에게 데미지를 입힘 (플레이어 합치고 추가)
        Debug.Log("발톱 공격!");
        attackCount++;
        AttackTarget();
    }

    public override void Initialize()
    {
        if (owner.skills[0].SkillID == SkillID && owner.skills[0] is ActiveSkillSO skill)
        {
            stat.Damage = skill.Skill_Damage;
            stat.Attack_Count = skill.Skill_Attack_Count;
        }
        else if (owner.skills[1].SkillID == SkillID && owner.skills[0] is ActiveSkillSO skill2)
        {
            stat.Damage = skill2.Skill_Damage;
            stat.Attack_Count = skill2.Skill_Attack_Count;
        }
    }
}