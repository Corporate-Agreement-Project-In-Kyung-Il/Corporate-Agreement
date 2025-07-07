using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class IDValuePair
{
    public int Selection_ID;
    public Value val;
}

[System.Serializable]
public class Value
{
    public int Equipment_Type_ID;
    public MyEnum Selection_Level;
    public string Description;
    public float Attack_LV_UP_Effect;
    public float HP_LV_UP_Effect;
    public int Equipment_LvUP;
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
[CreateAssetMenu(fileName = "OptionChoice", menuName = "LOH/OptionChoice_Equip")]
public class OptionChoice_Equip : ScriptableObject
{
    public List<IDValuePair> data = new List<IDValuePair>();

    public Value GetValue()
    {
        return new Value();
    }

    // 키로 Value 가져오기
    public Value GetValue(int key)
    {
        var pair = data.Find(x => x.Selection_ID == key);
        return pair != null ? pair.val : null;
    }
}
