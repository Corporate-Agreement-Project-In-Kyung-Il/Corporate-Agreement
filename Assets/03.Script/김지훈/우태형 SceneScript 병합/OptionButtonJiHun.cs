using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OptionButtonJiHun : MonoBehaviour
{
    private UnityEvent m_SkillOptionEvent;
    private UnityEvent m_EquipOptionEvent;
    private UnityEvent m_TrainingOptionEvent;

    public string optionType;
    public int selectID;
    public IBuffSelection[] buffSelections;
    private void Start()
    {
        
    }

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
