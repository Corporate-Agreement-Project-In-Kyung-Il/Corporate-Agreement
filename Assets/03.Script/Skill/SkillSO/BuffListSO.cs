using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffList", menuName = "ScriptableObjects/Skill/BuffList")]
public class BuffListSO : ScriptableObject
{
    public int Skill__Buff_Type_ID;
    public string SkillBuffTypeName;
    public string Buff_Description;
}
