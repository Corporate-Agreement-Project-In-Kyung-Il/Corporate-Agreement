using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
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
   public Text rerollCountText;
   public Canvas optionCanvas;
   public BaseValue selectedData;
   public GameObject checkOptionPanel;
   public Button popUpYesButton;
   
   public void OnClick()
   {
      switch (optionType)
      {
         case EOptionType.Skill:
            m_SkillOptionEvent.Invoke();
            GameManager.Instance.Resume();
            optionCanvas.gameObject.SetActive(false);
            break;
         case EOptionType.Equip:
            m_EquipOptionEvent.Invoke();
            GameManager.Instance.Resume();
            optionCanvas.gameObject.SetActive(false);
            break;
         case EOptionType.Training:
            m_TrainingOptionEvent.Invoke();
            GameManager.Instance.Resume();
            optionCanvas.gameObject.SetActive(false);
            break;
         
      }
   }
   /*public void PopUpPanel()
   {
      if (checkOptionPanel.activeSelf)
      {
         checkOptionPanel.SetActive(false);
      }
      else
      {
         checkOptionPanel.SetActive(true);
         popUpYesButton.onClick.AddListener();
      }
   }*/
}
