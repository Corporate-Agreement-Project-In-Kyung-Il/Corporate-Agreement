using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicExplosion : ActiveSkillBase, ISkillID
{
    
    //광역기 투사체
    public int SkillId;
    public int SkillID { get; set; }
    public void SetSkillID()
    {
        SkillID = SkillId;
    }

    public float moveSpeed;
    private BoxCollider2D coll;

    private void Awake()
    {
        Initialize();
    }
    void Start()
    {
        Debug.Log("start MagicExplosion");
        coll.enabled = false;
    }


    void Update()
    {
        Vector2 dir = (owner.target.transform.position - transform.position).normalized;
        float dis = Vector2.Distance(owner.target.transform.position, transform.position);

        transform.position += (Vector3)(dir * (moveSpeed * Time.deltaTime));

        if (dis < 0.2f)
        {
            coll.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Enemy")).Equals(false))
            return;


        Debug.Log("마법폭발 공격!");
        Destroy(gameObject);
        //데미지입힘
    }

    public override void Initialize()
    {
        coll = GetComponent<BoxCollider2D>();
        SetSkillID();
        if (owner.skills[0].SkillID == SkillID && owner.skills[0] is ActiveSkillSO skill)
        {
            stat.Damage = skill.Skill_Damage;
            coll.size = new Vector2(stat.Range_width, stat.Range_height);
        }
        else if (owner.skills[1].SkillID == SkillID && owner.skills[1] is ActiveSkillSO skill2)
        {
            stat.Damage = skill2.Skill_Damage;
            coll.size = new Vector2(stat.Range_width, stat.Range_height);
        }
    }
}