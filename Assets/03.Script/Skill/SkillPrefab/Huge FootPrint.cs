using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HugeFootPrint : ActiveSkillBase, ISkillID
{
    //광역기 한번때림
    public int SkillId;
    public int SkillID { get; set; }

    public void SetSkillID()
    {
        SkillID = SkillId;
    }

    public BoxCollider2D collider;

    public HugeFootPrint()
    {
        
    }

    private void Start()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Enemy")).Equals(false))
            return;

        Debug.Log("거대한 발자국 공격!");
        //데미지입힘
        Destroy(this);
    }

    public override void Initialize()
    {
        if (owner.skills[0].SkillID == SkillID && owner.skills[0] is ActiveSkillSO skill)
        {
            stat.Damage = skill.Skill_Damage;
            collider.size = new Vector2(stat.Range_width, stat.Range_height);
        }
        else if (owner.skills[1].SkillID == SkillID && owner.skills[1] is ActiveSkillSO skill2)
        {
            stat.Damage = skill2.Skill_Damage;
            collider.size = new Vector2(stat.Range_width, stat.Range_height);
        }
    }
}