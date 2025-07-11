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
   public string optionType;
   public int selectID;
   
   public void OnClick()
   {
      switch (optionType)
      {
         case "Skill":
            m_SkillOptionEvent.Invoke();
            break;
         case "Equip":
            m_EquipOptionEvent.Invoke();
            break;
         case "Training":
            m_TrainingOptionEvent.Invoke();
            break;
      }
   }
}
