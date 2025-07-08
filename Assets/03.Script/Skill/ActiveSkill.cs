using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkill : ActiveSkillData
{
    
    public override void UseSkill()
    {
        Debug.Log(Skill_Name);
    }
}