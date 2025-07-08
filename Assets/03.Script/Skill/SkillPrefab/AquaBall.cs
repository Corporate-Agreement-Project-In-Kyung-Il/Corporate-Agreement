using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AquaBall : SkillBase
{
    void Start()
    {
        
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
