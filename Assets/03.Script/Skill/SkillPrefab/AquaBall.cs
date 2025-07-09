using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AquaBall : MonoBehaviour,ISkillID
{
    public int SkillId;
    public int SkillID { get; set; }
    public void SetSkillID()
    {
        SkillID = SkillId;
    }
    void Start()
    {
        Debug.Log("start AquaBall");
    }

    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            //데미지입힘
        }
    }
    
}