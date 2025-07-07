using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    Active,
    Passive,
}

public enum ActiveName
{
    전사의강한의지,
    거대한발자국,
    야수의발톱,
    마법폭발,
    아쿠아볼
}

public enum PassiveName
{
}

[Serializable]
public class ActiveSkillData
{
    public string Skill_ID;
    public string Skill_Name; // enum으로 사용할 경우 SkillName enum 정의 필요
    public SkillType Skill_Type; // enum으로 사용할 경우 SkillType enum 정의 필요
    public int Skill_Minimum_LV;
    public int Skill_Maximum_LV;
    public float Skill_Cooldown;
    public float Skill_Damage;
    public int Skill_Attack_Count;
    public bool Wide_Area;
    public int Skill_Range_width;
    public int Skill_Range_height;
    public float Cooldown_Reduction; // (초 단위)
    public float Damage_Increase; // (%) 단위
}

[Serializable]
public class PassiveSkillData
{
    public string Skill_ID; // 스킬 고유 ID
    public string Skill_Name; // 스킬 이름
    public SkillType Skill_Type; // 예: "buff"
    public int Skill_Minimum_LV; // 스킬 사용 최소 레벨
    public character_class Skill_Class; // 직업 (전사, 궁수, 마법사 등)
    public character_name Skill_Character; // 캐릭터 이름 혹은 전용 여부
    public bool Skill_Range; // 범위 여부 (FALSE = 단일)
    public int Skill_Maximum_LV; // 스킬 최대 레벨
    public float Skill_Cooldown; // 쿨타임
    public float Skill_Duration; // 지속시간
    public string Skill_Buff_Type; // 버프 타입 ID (예: 300001)
    public float Skill_Activation_Rate; // 발동 확률 (0.4 = 40%)
    public float Cooldown_Reduction; // 쿨타임 감소량
    public float Duration_Increase; // 지속시간 증가량
    public float Activation_Rate_Increase; // 발동 확률 증가량
}

public interface ISkillAbleCheck
{
    public bool SkillConditionChecking();
    public bool SkillCoolChecking(float cooldown);
}