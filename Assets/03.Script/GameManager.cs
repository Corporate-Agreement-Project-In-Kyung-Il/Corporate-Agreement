using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.Rendering.Universal;


public class GameManager : MonoBehaviour
{
    [Tooltip("선택지 1, 2, 3번")]
    public OptionButton[] optionButtons; // UI 버튼 배열
    
    [Tooltip("선택지가 생성 될 때 기본 리롤 횟수가 설정 됩니다.")]
    public int baseRerollCount = 3; // 기본 리롤 횟수
    
    

    Dictionary<Enum, ScriptableObject> m_Options = new Dictionary<Enum, ScriptableObject>();
    
    [Tooltip("Skill SckiptableObject가 들어가야 합니다.")]
    public OptionChoice_SkillOption skillOption;
    [Tooltip("Equip SckiptableObject가 들어가야 합니다.")]
    public OptionChoice_EquipOption equipOption;
    [Tooltip("Training SckiptableObject가 들어가야 합니다.")]
    public OptionChoice_TrainingOption trainingOption;
    
    [SerializeField]
    private OptionChoice_SkillOption m_IngameSkillOption;
    public static GameManager Instance { get; private set; }
    public int[] characterSkillID = new int[6];
    [Tooltip("선택지 옵션 Canvas 입니다. 선택지가 등장할 때 활성화 되고, 버튼이 눌리면 비활성화 됩니다.")]
    public Canvas canvas;
 
    // Player에 InputGameManagerSkillID 메소드를 보면됨. 이때 0번째 = 전사, 1번째 = 궁수, 2번째 = 마법사

    public PlayerDataReceiverJiHun playerStatAdjust;
    
    [Tooltip("Skill Manager가 들어가야 합니다.")]
    public SkillManager skillManager;

    public static bool IsPaused = false;
    
    
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

        if (Input.GetKeyDown(KeyCode.R))
        {
            TestProbability();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        IsPaused = false;
    }
    
    public void Pause()
    {
        Time.timeScale = 0f;
        IsPaused = true;
    }

    public void TestProbability()
    {
        Debug.Log("확률 테스트 시작");
        int 노말 = 0;
        int 레어 = 0;
        int 에픽 = 0;
        int 유니크 = 0;
        int 레전드 = 0;
        int 신화 = 0;

        for (int i = 0; i < 1000000; i++)
        {
            int Q = GetRandomSelectionID(equipOption, EOptionType.Equip);
            switch (equipOption.GetValue(Q).Selection_Level)
            {
                case MyEnum.노말 :
                    노말++;
                    break;
                case MyEnum.레어 :
                    레어++;
                    break;
                case MyEnum.에픽 :
                    에픽++;
                    break;
                case MyEnum.유니크 :
                    유니크++;
                    break;
                case MyEnum.레전드 :
                    레전드++;
                    break;
                case MyEnum.신화 :
                    신화++;
                    break;
            }
        }
        Debug.Log($"노말 : {노말}");
        Debug.Log($"레어 : {레어}");
        Debug.Log($"에픽 : {에픽}");
        Debug.Log($"유니크 : {유니크}");
        Debug.Log($"레전드 : {레전드}");
        Debug.Log($"신화 : {신화}");
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
        skillManager.SetSkillOption(m_IngameSkillOption);
        CreateChoices(3);
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
        Pause();
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
                button.selectID = GetRandomSelectionID(skill, EOptionType.Skill);
                button.optionType = EOptionType.Skill;
                button.selectedData = skill.GetValue();
                Debug.Log($"[Skill] 선택지: {button.selectID}");
                break;

            case EOptionType.Equip:
                var equip = option as OptionChoice_EquipOption;
                var equipData = GetSelectionData(equip);
                button.selectID = GetRandomSelectionID(equip, EOptionType.Equip);
                button.optionType = EOptionType.Equip;
                button.selectedData = equipData.val;//equip.GetValue();
                Debug.Log($"[Equip] 선택지: {button.selectID}");
                break;

            case EOptionType.Training:
                var training = option as OptionChoice_TrainingOption;
                var tariningData = GetSelectionData(training);
                button.selectID = GetRandomSelectionID(training, EOptionType.Training);
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
    int GetRandomSelectionID<T>(ExelReaderBase<T> option, EOptionType optionType) where T : BaseValue, new()
    {
        if (option == null || option.data == null || option.data.Count == 0)
        {
            Debug.LogWarning("옵션이 null이거나 비어있음");
            return -1;
        }

        /*int randomIndex = UnityEngine.Random.Range(0, option.data.Count);
        return option.data[randomIndex].Key_ID; // IDValuePair<T> 에 id가 있다고 가정*/
        
        // 1. 등급별 가중치 설정
        Dictionary<MyEnum, int> levelWeights = new Dictionary<MyEnum, int>()
        {
            { MyEnum.노말, 500 },
            { MyEnum.레어, 257 },
            { MyEnum.에픽, 140 },
            { MyEnum.유니크, 80 },
            { MyEnum.레전드, 20 },
            { MyEnum.신화, 3 }
        };
        
        List<(int id, int weight)> weightedList = new List<(int, int)>();

        if (option is OptionChoice_SkillOption)
        {
            foreach (var pair in m_IngameSkillOption.data)
            {
                if (levelWeights.TryGetValue(((SkillOption)pair.val).Selection_Level, out int weight))
                {
                    weightedList.Add((pair.Key_ID, weight));
                }
            }
        }
        else if (option is OptionChoice_EquipOption)
        {
            foreach (var pair in equipOption.data)
            {
                if (levelWeights.TryGetValue(((EquipOption)pair.val).Selection_Level, out int weight))
                {
                    weightedList.Add((pair.Key_ID, weight));
                }
            }
        }
        else if (option is OptionChoice_TrainingOption)
        {
            foreach (var pair in trainingOption.data)
            {
                if (levelWeights.TryGetValue(((TrainingOption)pair.val).Selection_Level, out int weight))
                {
                    weightedList.Add((pair.Key_ID, weight));
                }
            }
        }
        else
        {
            Debug.LogError("지원하지 않는 타입입니다.");
            return -1;
        }

        if (weightedList.Count == 0)
        {
            Debug.LogWarning("가중치 목록이 비어 있음");
            return -1;
        }

        int totalWeight = weightedList.Sum(w => w.weight);
        int rand = UnityEngine.Random.Range(0, totalWeight);
        int cumulative = 0;

        foreach (var item in weightedList)
        {
            cumulative += item.weight;
            if (rand <= cumulative)
                return item.id;
        }

        Debug.LogWarning("Fallback으로 마지막 항목 선택됨");
        return weightedList.Last().id;
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



