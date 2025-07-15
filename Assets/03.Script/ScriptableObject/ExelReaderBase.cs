using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IDValuePair<T> where T : BaseValue
{
    public int Key_ID;
    public T val;
}

[System.Serializable]
public abstract class BaseValue
{
    
}

public enum MyEnum
{
    노말,
    레어,
    에픽,
    유니크,
    레전드,
    신화
}

public enum ECharacterGrade
{
    S,
    A
}

[System.Serializable]
public class EquipOption : BaseValue
{
    public MyEnum Selection_Level;
    public string Description;
    public int Equipment_Type_ID;
    public float Attack_LV_UP_Effect;
    public float HP_LV_UP_Effect;
    public int Equipment_LvUP;
}

[System.Serializable]
public class Equip : BaseValue
{
    public int Equipment_Type_ID;
    public string Equipment_Type_Name;
    public float Equipment_Attack;
    public float Equipment_HP;
    public int Equipment_Minimum_LV;
    public int Equipment_Maximum_LV;
    public float Attack_LV_UP_Effect;
    public float HP_LV_UP_Effect;
}

[System.Serializable]
public class Monster : BaseValue
{
    
}

[System.Serializable]
public class SkillOption : BaseValue
{
    public MyEnum Selection_Level;
    public string Description;
    public int Skill_ID;
    public float Cooldown_Reduction;
    public float Duration_Increase;
    public float Activation_Rate_Increase;
    public float Damage_Increase;
    public int Skill_LvUP;
}
[System.Serializable]
public class Training : BaseValue
{
    public int Training_ID;
    public string Training_Name;
    public int Critical_Rate;
    public float Critical_Damage;
    public float Attack_Speed;
    public int Training_Minimum_LV;
    public int Training_Maximum_LV;
    public float Critical_Damage_Increase;
    public float Critical_Rate_Increase;
    public float Attack_Speed_Increase;
}

[System.Serializable]
public class TrainingOption : BaseValue
{
    public MyEnum Selection_Level;
    public string Description;
    public int Training_ID;
    public float Critical_Damage_Increase;
    public float Critical_Rate_Increase;
    public float Attack_Speed_Increase;
    public int Training_LvUP;
}

[System.Serializable]
public class Character : BaseValue
{
    public int Character_ID;
    public string Character_Class;
    public string Character_Name;
    public string Character_Grade;
    public float Attack;
    public float Health;
    public float Attack_Speed;
    public float Critical_Probability;
    public int Training_type;
    public int equip_item;
    public int skill_possed1;
    public int skill_possed2;
}

public abstract class ExelReaderBase<T> : ScriptableObject where T : BaseValue, new()
{
    public List<IDValuePair<T>> data = new List<IDValuePair<T>>();
    
    public IList GetRawData() => data;
    public T GetValue()
    {
        return new T(); // new() 제한자 덕분에 가능
    }

    public T GetValue(int key)
    {
        var pair = data.Find(x => x.Key_ID == key);
        return pair != null ? pair.val : null;
    }
}