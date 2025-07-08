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
    public static GameManager Instance { get; private set; }
    public int[] characterIDs;

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
        SetDatabase();
    }

    void SetDatabase()
    {
#if UNITY_EDITOR
        // 1. HashSet으로 ID 생성
        HashSet<int> allowedIds = new HashSet<int>(characterIDs);
        
        // 2. 필터링 : Skill_ID를 기준으로 필터링
        var filtered = skillOption.data
            .Where(pair => allowedIds.Contains(((SkillOption)pair.val).Skill_ID))
            .ToList();
        
        //3. 새 ScriptableObject 생성 및 데이터 할당
        var inGameSkillOption = ScriptableObject.CreateInstance<OptionChoice_SkillOption>();
        inGameSkillOption.data = filtered;
        
        //4. 저장 경로 및 저장 처리
        AssetDatabase.CreateAsset(inGameSkillOption, $"Assets/00.Resources/OptionChoice/InGameSkillOptionChoice.asset");
        AssetDatabase.SaveAssets();
#endif
    }
}
