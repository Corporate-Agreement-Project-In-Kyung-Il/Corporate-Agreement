using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Random = System.Random;

public class OptionSet : MonoBehaviour
{
    public OptionChoice_SkillOption skillOption;
    public OptionChoice_EquipOption equipOption;
    public OptionChoice_TrainingOption trainingOption;
    public int choiceCount = 3;
    public OptionButton[] optionButtons; // UI 버튼 배열
    
    public enum EOptionType
    {
        Skill,
        Equip,
        Training
    }
    Dictionary<Enum, ScriptableObject> m_Options = new Dictionary<Enum, ScriptableObject>();
    private void Awake()
    {
        m_Options.Add(EOptionType.Skill, skillOption);
        m_Options.Add(EOptionType.Equip, equipOption);
        m_Options.Add(EOptionType.Training, trainingOption);
    }

    private void Start()
    {
        optionButtons = new OptionButton[3];
        optionButtons[0] = new OptionButton { selectionButton = GameObject.Find("SelectionButton1").GetComponent<Button>(), 
            enchantButton = GameObject.Find("EnchantButton1").GetComponent<Button>() };
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateChoices();
        }
    }

    private void CreateChoices()
    {
        for (int i = 0; i < choiceCount; i++)
        {
            EOptionType choicedOption = GetOptionType();
            if (m_Options.TryGetValue(choicedOption, out ScriptableObject option))
            {
                switch (choicedOption)
                {
                    case EOptionType.Skill:
                        var skill = option as OptionChoice_SkillOption;
                        int skillID = GetSelectionID(skill);
                        Debug.Log("Skill 선택지 띄움 SkillOption 번호 : " + skillID);
                        break;
                    case EOptionType.Equip:
                        var equip = option as OptionChoice_EquipOption;
                        int equipID = GetSelectionID(equip);
                        Debug.Log("Equip 선택지 선택됨 EquipOption 번호: " + equipID);
                        break;
                    case EOptionType.Training:
                        var training = option as OptionChoice_TrainingOption;
                        int trainingID = GetSelectionID(training);
                        Debug.Log("Training 선택지 선택됨 Training 번호: " + trainingID);
                        break;
                }
            }
            else
            {
                Debug.LogError("No option found for type: " + choicedOption);
            }
        }
    }

    // 3종류 선택지 중에 어떤 선택지를 띄울지 랜덤 선택 (장비 , 스킬, 훈련)
    EOptionType GetOptionType()
    {
        EOptionType[] values = (EOptionType[])System.Enum.GetValues(typeof(EOptionType));
        int randomIndex = UnityEngine.Random.Range(0, values.Length);
        return values[randomIndex];
    }
    
    // 선택지 번호를 가져오는 코드
    int GetSelectionID<T>(ExelReaderBase<T> option) where T : BaseValue, new()
    {
        if (option == null || option.data == null || option.data.Count == 0)
        {
            Debug.LogWarning("옵션이 null이거나 비어있음");
            return -1;
        }

        int randomIndex = UnityEngine.Random.Range(0, option.data.Count);
        return option.data[randomIndex].Key_ID; // IDValuePair<T> 에 id가 있다고 가정
    }
}
