using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkill 
{
    private ActiveSkillData data;
    private float coolTimer;

    public ActiveSkill(ActiveSkillData data)
    {
        this.data = data;
    }

    public void Initialize()
    {
        coolTimer = 0f;
    }

    public void Use()
    {
        if (coolTimer > 0) return;
        SpawnPrefab(data.Skill_Damage, data.Skill_Range_width, data.Skill_Range_height, data.Wide_Area);
        coolTimer = data.Skill_Cooldown - data.Cooldown_Reduction;
    }

    public void SpawnPrefab(float damage, int width, int height, bool wide)
    {
        
    }

    public void CooldownCheck(float delta)
    {
        coolTimer -= delta;
    }
}