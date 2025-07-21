using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[Obsolete("이제 안 씀, GameManager를 사용하도록", true)]
public class GameManagerJiHun : MonoBehaviour
{
    public Canvas canvas;
    public int choiceCount = 3;
    public OptionButtonJiHun[] optionButtons; // UI 버튼 배열
    
    Dictionary<Enum, ScriptableObject> m_Options = new Dictionary<Enum, ScriptableObject>();
    
    public OptionChoice_SkillOption skillOption;
    public OptionChoice_EquipOption equipOption;
    public OptionChoice_TrainingOption trainingOption;
    
    [SerializeField]
    private OptionChoice_SkillOption m_IngameSkillOption;
    public static GameManagerJiHun Instance { get; private set; }
    
    //Player에 InputGameManagerSkillID 메소드를 보면됨. 이때 0번째 = 전사, 1번째 = 궁수, 2번째 = 마법사
    public int[] characterID = new int[3];

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
        
        playerStatAdjust = GetComponentInChildren<PlayerDataReceiverJiHun>();
    }

    private void Start()
    {
        m_Options.Add(EOptionType.Skill, skillOption);
        m_Options.Add(EOptionType.Equip, equipOption);
        m_Options.Add(EOptionType.Training, trainingOption);
        // 오류 저장용 
        SetDatabase();
    }

    public bool isButtonActivePrepare = false;
    private void Update()
    {
        if (isButtonActivePrepare.Equals(false))
        {
            for (int i = 0; i < choiceCount; i++)
            {
                // 선택지 생성 전, 버튼 비활성화
                optionButtons[i].gameObject.SetActive(false);
                optionButtons[i].selectedData = null;
                canvas.gameObject.SetActive(false);
            }

            isButtonActivePrepare = true;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isButtonActivePrepare)
        {
            for (int i = 0; i < choiceCount; i++)
            {
                // 선택지 생성, 버튼 활성화
                optionButtons[i].gameObject.SetActive(true);
            }
            
            canvas.gameObject.SetActive(true);
            

            CreateChoices();
        }
    }

    void SetDatabase()
    {
#if UNITY_EDITOR
        // 게임 시작할 때 InGame선택지 ScriptableObject 생성
        // 1. HashSet으로 ID 생성
        HashSet<int> allowedIds = new HashSet<int>(characterID);
        
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
#endif
    }
    
    #region 랜덤 선택지 뽑는 코드
    
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
                        optionButtons[i].selectID = skillID;
                        optionButtons[i].optionType = "Skill";
                        Debug.Log("Skill 선택지 띄움 SkillOption 번호 : " + skillID + "/" + i);
                        break;
                    case EOptionType.Equip:
                        var equip = option as OptionChoice_EquipOption;
                        var equipData = GetSelectionData(equip);
                        //int equipID = GetSelectionID(equip);
                        
                        optionButtons[i].selectID = equipData.Key_ID;
                        //optionButtons[i].selectID = equipID;
                        optionButtons[i].optionType = "Equip";
                        
                        optionButtons[i].selectedData = equipData.val;
                        Debug.Log("Equip 선택지 선택됨 EquipOption 번호: " + equipData.Key_ID + "/" + i);
                        break;
                    case EOptionType.Training:
                        var training = option as OptionChoice_TrainingOption;
                        var trainingData = GetSelectionData(training);
                        
                        //int trainingID = GetSelectionID(training);
                        optionButtons[i].selectID = trainingData.Key_ID;
                        optionButtons[i].optionType = "Training";
                        
                        optionButtons[i].selectedData = trainingData.val; // ✅ 여기에 저장
                        Debug.Log("Training 선택지 선택됨 Training 번호: " + trainingData.Key_ID + "/" + i);
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
    
    // 선택된 데이터 자체를 반환하는 함수
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
