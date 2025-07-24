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
   private Button m_PopUpConfirmButton;
   
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
   public void PopUpPanel()
   {
      if (checkOptionPanel.activeSelf)
      {
         checkOptionPanel.SetActive(false);
      }
      else
      {
         checkOptionPanel.SetActive(true);
         Panel panel = checkOptionPanel.GetComponent<Panel>();
         switch (optionType)
         {
            case EOptionType.Skill:
               selectedData = GameManager.Instance.skillOption.GetValue(selectID);
               panel.contentText.text = (selectedData as SkillOption)?.Description;
               panel.confirmButton.onClick.AddListener(OnClick);
               break;
            case EOptionType.Equip:
               selectedData = GameManager.Instance.equipOption.GetValue(selectID);
               panel.contentText.text = (selectedData as EquipOption)?.Description;
               panel.confirmButton.onClick.AddListener(OnClick);
               break;
            case EOptionType.Training:
               selectedData = GameManager.Instance.trainingOption.GetValue(selectID);
               panel.contentText.text = (selectedData as TrainingOption)?.Description;
               panel.confirmButton.onClick.AddListener(OnClick);
               break;
         }
      }
   }
}
