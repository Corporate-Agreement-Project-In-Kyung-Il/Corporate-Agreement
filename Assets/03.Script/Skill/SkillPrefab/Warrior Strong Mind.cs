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
    
    private void Start()
    {
        Debug.Log("start WarriorStrongMind");
        attackCount = 0;
        AttakcTarget();
    }

    private void Update()
    {
        transform.position=owner.target.transform.position;
    }

    public void AttakcTarget()
    {
        //owner.target에게 데미지를 입힘 (플레이어 합치고 추가)
        Debug.Log("전사의 강한의지 공격!");
        attackCount++;
        Destroy(gameObject);
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