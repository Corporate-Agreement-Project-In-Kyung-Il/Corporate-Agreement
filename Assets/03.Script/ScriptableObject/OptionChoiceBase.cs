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
    
}

[System.Serializable]
public class Training : BaseValue
{
    
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

[CreateAssetMenu(fileName = "OptionChoice_Equip", menuName = "LOH/OptionChoice_Equip")]
public class OptionChoice_Equip : OptionChoiceBase<Equip> { }

[CreateAssetMenu(fileName = "OptionChoice_Skill", menuName = "LOH/OptionChoice_Skill")]
public class OptionChoice_Skill : OptionChoiceBase<Skill> { }

[CreateAssetMenu(fileName = "OptionChoice_Training", menuName = "LOH/OptionChoice_Training")]
public class OptionChoice_Training : OptionChoiceBase<Training> { }