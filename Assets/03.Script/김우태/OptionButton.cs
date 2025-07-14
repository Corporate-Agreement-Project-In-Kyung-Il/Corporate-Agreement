using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class OptionButton : MonoBehaviour
{
   [SerializeField] 
   UnityEvent m_SkillOptionEvent;
   
   [SerializeField] 
   UnityEvent m_EquipOptionEvent;
   
   [SerializeField] 
   UnityEvent m_TrainingOptionEvent;

   public int rerollCount;
   public EOptionType optionType;
   public int selectID;
   public Canvas optionCanvas;
   public BaseValue selectedData;

   public void OnClick()
   {
      optionCanvas.gameObject.SetActive(false);
      switch (optionType)
      {
         case EOptionType.Skill:
            m_SkillOptionEvent.Invoke();
            break;
         case EOptionType.Equip:
            m_EquipOptionEvent.Invoke();
            break;
         case EOptionType.Training:
            m_TrainingOptionEvent.Invoke();
            break;
      }
   }
}
