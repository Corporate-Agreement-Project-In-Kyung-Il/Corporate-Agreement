using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public OptionChoice_SkillOption skillOption;
    public OptionChoice_EquipOption equipOption;
    public OptionChoice_TrainingOption trainingOption;
    
    [SerializeField]
    private OptionChoice_SkillOption m_IngameSkillOption;
    public static GameManager Instance { get; private set; }
    public int[] characterID;

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
        int[] playerSkillID = { 100012, 100016, 100020 };
        GameStart(playerSkillID);
    }

    public void GameStart(int[] playerSkillID)
    {
        SetPlayerSkillID(playerSkillID);
        SetDatabase();
    }

    void SetPlayerSkillID(int[] playerSkillID)
    {
        characterID = playerSkillID;
    }
    // 게임 시작할 때 InGame선택지 ScriptableObject 생성
    void SetDatabase()
    {
#if UNITY_EDITOR
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
}
