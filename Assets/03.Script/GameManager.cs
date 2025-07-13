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
    public int[] characterSkillID;

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

    private void Start()
    {
        GameStart();
    }
    
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
    
    private void CreateChoices(int choiceCount)
    {
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
    
    private int GradeToInt(MyEnum grade)
    {
        return grade switch
        {
            MyEnum.노말 => 1, 
            MyEnum.레어=> 2,
            MyEnum.에픽 => 3,
            MyEnum.유니크 => 4,
            MyEnum.레전드 => 5,
            MyEnum.신화 => 6,
            _ => 0
        };
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
                var data = m_IngameSkillOption;
                var value = data.GetValue(optionButton.selectID);
                var s = value.Selection_Level;
                HashSet<int> skillID = new HashSet<int>{ value.Skill_ID };
        
                // 2. 필터링 : Skill_ID를 기준으로 필터링
                var filtered = m_IngameSkillOption.data
                    .Where(pair => skillID.Contains(((SkillOption)pair.val).Skill_ID))
                    .ToList();
                var nextGrade = filtered
                    .Where(v => v.val.Selection_Level > value.Selection_Level)
                    .OrderBy(v => v.val.Selection_Level)
                    .FirstOrDefault();
                if (nextGrade != null)
                {
                    // 승급 가능
                    optionButton.selectID = nextGrade.Key_ID;
                    Debug.Log($"[Upgrade] {value.Selection_Level} → {nextGrade.val.Selection_Level}");
                }
                else
                {
                    // 더 높은 등급이 없음
                    Debug.Log("이미 최고 등급입니다.");
                }
                break;
            case EOptionType.Equip :
                break;
            case EOptionType.Training :
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
                Debug.Log($"[Skill] 선택지: {button.selectID}");
                break;

            case EOptionType.Equip:
                var equip = option as OptionChoice_EquipOption;
                button.selectID = GetSelectionID(equip);
                button.optionType = EOptionType.Equip;
                Debug.Log($"[Equip] 선택지: {button.selectID}");
                break;

            case EOptionType.Training:
                var training = option as OptionChoice_TrainingOption;
                button.selectID = GetSelectionID(training);
                button.optionType = EOptionType.Training;
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
    #endregion
}

public enum EOptionType
{
    Skill,
    Equip,
    Training
}

