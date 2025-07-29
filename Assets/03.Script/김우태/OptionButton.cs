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
   
   [SerializeField]
   private Sprite[] m_GradeImages;
   [SerializeField]
   private Image m_ChoiceImage;
   private Button m_PopUpConfirmButton;
   
   public void OnClick()
   {
      var panel = checkOptionPanel.GetComponent<Panel>();
      panel.confirmButton.onClick.RemoveAllListeners();
      panel.contentText.text = null;
      checkOptionPanel.SetActive(false);
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

   public void SetOptionGradeImage(MyGrade grade)
   {
      switch (grade)
      {
         case MyGrade.노말 :
            m_ChoiceImage.sprite = m_GradeImages[0];
            break;
         case MyGrade.레어 :
            m_ChoiceImage.sprite = m_GradeImages[1];
            break;
         case MyGrade.에픽 :
            m_ChoiceImage.sprite = m_GradeImages[2];
            break;
         case MyGrade.유니크 :
            m_ChoiceImage.sprite = m_GradeImages[3];
            break;
         case MyGrade.레전드 :
            m_ChoiceImage.sprite = m_GradeImages[4];
            break;
         case MyGrade.신화 :
            m_ChoiceImage.sprite = m_GradeImages[5];
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
