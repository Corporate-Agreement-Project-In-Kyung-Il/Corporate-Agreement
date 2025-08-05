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
   public bool isUpgradable;
   public GameObject brokenImage;
   
   public Text rerollCountText;
   public Canvas optionCanvas;
   public BaseValue selectedData;
   public GameObject checkOptionPanel;
   public Sprite[] gradeImages;
   public Image choiceImage;
   public Text choiceText;
   public Text gradeText;
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
            optionCanvas.gameObject.SetActive(false);
            GameManager.Instance.Resume();
            break;
         case EOptionType.Equip:
            m_EquipOptionEvent.Invoke();
            optionCanvas.gameObject.SetActive(false);
            GameManager.Instance.Resume();
            break;
         case EOptionType.Training:
            m_TrainingOptionEvent.Invoke();
            optionCanvas.gameObject.SetActive(false);
            GameManager.Instance.Resume();
            break;
      }
   }

   public void SetOptionGradeImage(EMyGrade grade, string selection_Name)
   {
      switch (grade)
      {
         case EMyGrade.노말 :
            choiceImage.sprite = gradeImages[0];
            choiceText.text = $"{grade.ToString()} {selection_Name}";
            gradeText.text = grade.ToString();
            break;
         case EMyGrade.레어 :
            choiceImage.sprite = gradeImages[1];
            choiceText.text = $"{grade.ToString()} {selection_Name}";
            gradeText.text = grade.ToString();
            break;
         case EMyGrade.에픽 :
            choiceImage.sprite = gradeImages[2];
            choiceText.text = $"{grade.ToString()} {selection_Name}";
            gradeText.text = grade.ToString();
            break;
         case EMyGrade.유니크 :
            choiceImage.sprite = gradeImages[3];
            choiceText.text = $"{grade.ToString()} {selection_Name}";
            gradeText.text = grade.ToString();
            break;
         case EMyGrade.레전드 :
            choiceImage.sprite = gradeImages[4];
            choiceText.text = $"{grade.ToString()} {selection_Name}";
            gradeText.text = grade.ToString();
            break;
         case EMyGrade.신화 :
            choiceImage.sprite = gradeImages[5];
            choiceText.text = $"{grade.ToString()} {selection_Name}";
            gradeText.text = grade.ToString();
            break;
      }
   }
   public void PopUpOptionChoicePanel()
   {
      if (isUpgradable == false)
      {
         return;
      }
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
