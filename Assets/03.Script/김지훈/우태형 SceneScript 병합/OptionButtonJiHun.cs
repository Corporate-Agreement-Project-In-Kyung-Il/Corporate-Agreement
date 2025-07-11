using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OptionButtonJiHun : MonoBehaviour
{
    [SerializeField] private UnityEvent m_SkillOptionEvent;
    [SerializeField] private UnityEvent m_EquipOptionEvent;
    [SerializeField] private UnityEvent m_TrainingOptionEvent;

    public string optionType;
    public int selectID;

    public BaseValue selectedData; // ✅ 선택된 데이터 저장

    public void OnClick()
    {
        switch (optionType)
        {
            case "Skill" :
                m_SkillOptionEvent.Invoke();
                break;
            case "Equip" :
                m_EquipOptionEvent.Invoke();
                break;
            case "Training" :
                m_TrainingOptionEvent.Invoke();
                break;
        }
    }
}
