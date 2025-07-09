using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AquaBall : MonoBehaviour,ISkillID
{//광역기 투사체, 적 추적후 터짐
    public int SkillId;
    public int SkillID { get; set; }
    public void SetSkillID()
    {
        SkillID = SkillId;
    }
    
    void Start()
    {
        Debug.Log("start AquaBall");
        coll = GetComponent<Collider2D>();
        coll.enabled = false;
    }

    public Player owner;
    public float moveSpeed;
    private Collider2D coll;
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
        if (other.CompareTag("Enemy"))
        {
            //데미지입힘
        }
    }
    
}