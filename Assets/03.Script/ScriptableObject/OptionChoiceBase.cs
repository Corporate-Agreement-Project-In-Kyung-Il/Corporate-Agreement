using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IDValuePair<T> where T : BaseValue
{
    public int Selection_ID;
    public T val;
}

[System.Serializable]
public abstract class BaseValue
{
    public MyEnum Selection_Level;
    public string Description;
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

[System.Serializable]
public class Equip : BaseValue
{
    public int Equipment_Type_ID;
    public float Attack_LV_UP_Effect;
    public float HP_LV_UP_Effect;
    public int Equipment_LvUP;
}

[System.Serializable]
public class Skill : BaseValue
{
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
    public float Critical_Damage_Increase;
    public float Critical_Rate_Increase;
    public float Attack_Speed_Increase;
    public int Training_LvUP;
}


public abstract class OptionChoiceBase<T> : ScriptableObject where T : BaseValue, new()
{
    public List<IDValuePair<T>> data = new List<IDValuePair<T>>();

    public T GetValue()
    {
        return new T(); // new() 제한자 덕분에 가능
    }

    public T GetValue(int key)
    {
        var pair = data.Find(x => x.Selection_ID == key);
        return pair != null ? pair.val : null;
    }
}