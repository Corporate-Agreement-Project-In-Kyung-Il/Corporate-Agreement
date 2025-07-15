using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class GameManager : MonoBehaviour
{
    public OptionButton[] optionButtons; // UI 버튼 배열
    public int baseRerollCount = 3; // 기본 리롤 횟수
    
    

    Dictionary<Enum, ScriptableObject> m_Options = new Dictionary<Enum, ScriptableObject>();
    
    public OptionChoice_SkillOption skillOption;
    public OptionChoice_EquipOption equipOption;
    public OptionChoice_TrainingOption trainingOption;
    
    [SerializeField]
    private OptionChoice_SkillOption m_IngameSkillOption;
    public static GameManager Instance { get; private set; }
    public int[] characterSkillID = new int[6];
    public Canvas canvas;
 
    // Player에 InputGameManagerSkillID 메소드를 보면됨. 이때 0번째 = 전사, 1번째 = 궁수, 2번째 = 마법사

    public PlayerDataReceiverJiHun playerStatAdjust;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /*
    private void Start()
    {
        GameStart();
    }
    */
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateChoices(3);
        }
    }

    public void GameStart()
    {
        SetIngameDatabase();
    }

    void SetIngameDatabase()
    {
#if UNITY_EDITOR
        // 게임 시작할 때 InGame선택지 ScriptableObject 생성
        // 1. HashSet으로 ID 생성
        HashSet<int> allowedIds = new HashSet<int>(characterSkillID);
        
        // 2. 필터링 : Skill_ID를 기준으로 필터링
        var filtered = skillOption.data
            .Where(pair => allowedIds.Contains(((SkillOption)pair.val).Skill_ID))
            .ToList();
        
        //3. 새 ScriptableObject 생성 및 데이터 할당
        m_IngameSkillOption = ScriptableObject.CreateInstance<OptionChoice_SkillOption>();
        m_IngameSkillOption.data = filtered;
        
        //4. 저장 경로 및 저장 처리
        AssetDatabase.CreateAsset(m_IngameSkillOption, $"Assets/00.Resources/DataBase/OptionChoice/InGameSkillOptionChoice.asset");
        AssetDatabase.SaveAssets();
        
        m_Options.Add(EOptionType.Skill, m_IngameSkillOption);
        m_Options.Add(EOptionType.Equip, equipOption);
        m_Options.Add(EOptionType.Training, trainingOption);
#endif
    }
    
    #region 랜덤 선택지 뽑는 코드
    
    public void CreateChoices(int choiceCount)
    {
        canvas.gameObject.SetActive(true);
        for (int i = 0; i < choiceCount; i++)
        {
            SetRandomOptionToButton(optionButtons[i], baseRerollCount);
        }
    }

    public void RerollChoice(OptionButton optionButton)
    {
        if (optionButton.rerollCount == 0)
        {
            Debug.Log("리롤 횟수 없음!!");
            return;
        }

        optionButton.rerollCount--;
        SetRandomOptionToButton(optionButton, optionButton.rerollCount); // 남은 횟수 유지
    }

    public ScriptableObject type;
    public void UpgradeChoice(OptionButton optionButton)
    {
        GetMatchedOptionData(optionButton);
    }
    
    public void  GetMatchedOptionData(OptionButton optionButton)
    {
        type = GetOptionType(optionButton.optionType);

        // 필드 접근
        var dataField = type.GetType().GetField("data");

        switch (optionButton.optionType)
        {
            case EOptionType.Skill :
                var skillData = m_IngameSkillOption;
                var skillValue = skillData.GetValue(optionButton.selectID);
                HashSet<int> skillID = new HashSet<int>{ skillValue.Skill_ID };
                
                var skillFiltered = m_IngameSkillOption.data
                    .Where(pair => skillID.Contains(((SkillOption)pair.val).Skill_ID))
                    .ToList();
                var skillNextGrade = skillFiltered
                    .Where(v => v.val.Selection_Level > skillValue.Selection_Level)
                    .OrderBy(v => v.val.Selection_Level)
                    .FirstOrDefault();
                if (skillNextGrade != null)
                {
                    // 승급 가능
                    optionButton.selectID = skillNextGrade.Key_ID;
                    Debug.Log($"[Upgrade] {skillValue.Selection_Level} → {skillNextGrade.val.Selection_Level}");
                }
                else
                {
                    // 더 높은 등급이 없음
                    Debug.Log("이미 최고 등급입니다.");
                }
                break;
            case EOptionType.Equip : 
                var equipData = equipOption;
                var equipValue = equipData.GetValue(optionButton.selectID);
                HashSet<int> equipID = new HashSet<int>{ equipValue.Equipment_Type_ID };
                
                var equipFiltered = equipOption.data
                    .Where(pair => equipID.Contains(((EquipOption)pair.val).Equipment_Type_ID))
                    .ToList();
                var equipNextGrade = equipFiltered
                    .Where(v => v.val.Selection_Level > equipValue.Selection_Level)
                    .OrderBy(v => v.val.Selection_Level)
                    .FirstOrDefault();
                if (equipNextGrade != null)
                {
                    // 승급 가능
                    optionButton.selectID = equipNextGrade.Key_ID;
                    Debug.Log($"[Upgrade] {equipValue.Selection_Level} → {equipNextGrade.val.Selection_Level}");
                }
                else
                {
                    // 더 높은 등급이 없음
                    Debug.Log("이미 최고 등급입니다.");
                }
                break;
            case EOptionType.Training :
                var trainingData = trainingOption;
                var trainingValue = trainingData.GetValue(optionButton.selectID);
                HashSet<int> trainingID = new HashSet<int>{ trainingValue.Training_ID };
                
                // 2. 필터링 : Skill_ID를 기준으로 필터링
                var trainingFiltered = trainingOption.data
                    .Where(pair => trainingID.Contains(((TrainingOption)pair.val).Training_ID))
                    .ToList();
                var trainingNextGrade = trainingFiltered
                    .Where(v => v.val.Selection_Level > trainingValue.Selection_Level)
                    .OrderBy(v => v.val.Selection_Level)
                    .FirstOrDefault();
                if (trainingNextGrade != null)
                {
                    // 승급 가능
                    optionButton.selectID = trainingNextGrade.Key_ID;
                    Debug.Log($"[Upgrade] {trainingValue.Selection_Level} → {trainingNextGrade.val.Selection_Level}");
                }
                else
                {
                    // 더 높은 등급이 없음
                    Debug.Log("이미 최고 등급입니다.");
                }
                break;
        }
    }
    
    private ScriptableObject GetOptionType(EOptionType optionType)
    {
        return optionType switch
        {
            EOptionType.Skill => m_IngameSkillOption,
            EOptionType.Equip => equipOption,
            EOptionType.Training => trainingOption,
            _ => null
        };
    }

    /// <summary>
    /// 버튼에 랜덤 선택지 적용 (Skill / Equip / Training)
    /// </summary>
    private void SetRandomOptionToButton(OptionButton button, int rerollCount)
    {
        EOptionType choicedOption = GetRandomOptionType();

        if (!m_Options.TryGetValue(choicedOption, out ScriptableObject option))
        {
            Debug.LogError("No option found for type: " + choicedOption);
            return;
        }

        button.rerollCount = rerollCount;

        switch (choicedOption)
        {
            case EOptionType.Skill:
                var skill = option as OptionChoice_SkillOption;
                button.selectID = GetSelectionID(skill);
                button.optionType = EOptionType.Skill;
                button.selectedData = skill.GetValue();
                Debug.Log($"[Skill] 선택지: {button.selectID}");
                break;

            case EOptionType.Equip:
                var equip = option as OptionChoice_EquipOption;
                var equipData = GetSelectionData(equip);
                button.selectID = GetSelectionID(equip);
                button.optionType = EOptionType.Equip;
                button.selectedData = equipData.val;//equip.GetValue();
                Debug.Log($"[Equip] 선택지: {button.selectID}");
                break;

            case EOptionType.Training:
                var training = option as OptionChoice_TrainingOption;
                var tariningData = GetSelectionData(training);
                button.selectID = GetSelectionID(training);
                button.optionType = EOptionType.Training;
                button.selectedData = tariningData.val;
                Debug.Log($"[Training] 선택지: {button.selectID}");
                break;
        }
    }


    // 3종류 선택지 중에 어떤 선택지를 띄울지 랜덤 선택 (장비 , 스킬, 훈련)
    EOptionType GetRandomOptionType()
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
    
    IDValuePair<T> GetSelectionData<T>(ExelReaderBase<T> option) where T : BaseValue, new()
    {
        if (option == null || option.data == null || option.data.Count == 0)
        {
            Debug.LogWarning("옵션이 null이거나 비어있음");
            return null;
        }

        int randomIndex = UnityEngine.Random.Range(0, option.data.Count);
        return option.data[randomIndex];
    }
    #endregion
}

public enum EOptionType
{
    Skill,
    Equip,
    Training
}

