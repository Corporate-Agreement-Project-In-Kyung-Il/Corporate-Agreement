using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WarriorStrongMind : ActiveSkillBase, ISkillID
{
    //단일 공격 3번때림 
    public int SkillId;
    public int SkillID { get; set; }

    public void SetSkillID()
    {
        SkillID = SkillId;
    }

    public int attackCount;

    public WarriorStrongMind()
    {
        AttakcTarget();
    }

    private void Start()
    {
        Debug.Log("start WarriorStrongMind");
        attackCount = 0;
    }

    private void Update()
    {
        if (owner.target == null) return;
        transform.position = owner.target.transform.position; //타겟 포지션추적
    }

    private void AttakcTarget()
    {
        if (attackCount >= stat.Attack_Count || owner.target == null)
        {
            Destroy(this);
            return;
        }
        
        //owner.target에게 데미지를 입힘 (플레이어 합치고 추가)
        Debug.Log("전사의 강한의지 공격!");
        attackCount++;
        AttakcTarget();
    }

    public override void Initialize()
    {
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