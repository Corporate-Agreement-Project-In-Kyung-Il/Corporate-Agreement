using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class SkillBase : MonoBehaviour
{
    [SerializeField] private ScriptableObject Skill;

    public int SkillID { get; private set; }

    private void Start()
    {
        if (Skill is ISkillID skillIDObject)
        {
            SkillID = skillIDObject.SkillID;
            Debug.Log($"Skill ID: {SkillID}");
        }
        else
        {
            Debug.LogWarning("Skill이 ISkillID를 구현하지 않았습니다.");
        }
    }
}