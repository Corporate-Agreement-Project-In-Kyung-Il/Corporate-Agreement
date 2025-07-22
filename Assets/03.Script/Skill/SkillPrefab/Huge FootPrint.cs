using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HugeFootPrint : ActiveSkillBase, ISkillID
{
    //광역기 한번때림
    public int SkillId;
    public int SkillID { get; set; }
    public GameObject effect;

    public void SetSkillID()
    {
        SkillID = SkillId;
    }

    public BoxCollider2D coll;

    private void Awake()
    {
        effect.SetActive(false);
        Initialize();
    }

    private void Start()
    {
    }

    void Update()
    {
        if (owner.target == null) return;

        transform.position = owner.target.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Enemy")).Equals(false))
            return;

        effect.SetActive(true);
        //데미지입힘
        if (other.gameObject.TryGetComponent(out IDamageAble enemyDamage))
        {
            CombatEvent combatEvent = new CombatEvent();
            combatEvent.Receiver = enemyDamage;
            combatEvent.Sender = owner;
            combatEvent.Damage = stat.Damage;
            combatEvent.collider = other;

            CombatSystem.instance.AddCombatEvent(combatEvent);

            Debug.Log("거대한 발자국 공격!");
            //coll.enabled = false;

            StartCoroutine(effectDelay());
        }
    }

    IEnumerator effectDelay()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    public override void Initialize()
    {
        coll = GetComponent<BoxCollider2D>();
        SetSkillID();
        if (owner.skills[0].SkillID == SkillID && owner.skills[0] is ActiveSkillSO skill)
        {
            stat.Damage = skill.Skill_Damage;
            stat.Range_height = skill.Skill_Range_height;
            stat.Range_width = skill.Skill_Range_width;

            coll.size = new Vector2(stat.Range_width, stat.Range_height);
        }
        else if (owner.skills[1].SkillID == SkillID && owner.skills[1] is ActiveSkillSO skill2)
        {
            stat.Damage = skill2.Skill_Damage;
            stat.Range_height = skill2.Skill_Range_height;
            stat.Range_width = skill2.Skill_Range_width;

            coll.size = new Vector2(stat.Range_width, stat.Range_height);
        }

        // effect.transform.localScale = new Vector3(stat.Range_width, stat.Range_height, 1);
    }
}