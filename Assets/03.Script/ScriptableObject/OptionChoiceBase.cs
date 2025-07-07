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


[CreateAssetMenu(fileName = "OptionChoice", menuName = "LOH/OptionChoiceBase")]
public class OptionChoiceBase : ScriptableObject
{
    public List<IDValuePair<Equip>> data = new List<IDValuePair<Equip>>();

    public Equip GetValue()
    {
        return new Equip();
    }

    public Equip GetValue(int key)
    {
        var pair = data.Find(x => x.Selection_ID == key);
        return pair != null ? pair.val : null;
    }
}