using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Buff", menuName = "ScriptableObjects/Skill/Buff")]
public class BuffSO : ScriptableObject
{
    public int Skill_ID;
    public string Skill_Name;
    public SkillType Skill_Type;
    public int Skill_Minimum_LV;

    public  character_class Skill_Class;
    public character_name Skill_Character;
    public bool Skill_Range;

    public int Skill_Maximum_LV;
    public float Skill_Cooldown;
    public float Skill_Duration;

    public string Skill_Buff_Type;
    public float Skill_Activation_Rate;

    public float Cooldown_Reduction;
    public float Duration_Increase;
    public float Activation_Rate_Increase;
}
