using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerList : MonoBehaviour
{
    private static PlayerList instance;
    public int currentStage;
    public static PlayerList Instance
    {
        get
        {
            return instance;
        }
    }

    public int[] CharacterIDs = new int[3];
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        // 씬 변경 감지
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        if (instance == this) instance = null;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬 index 0이면 파괴
        if (scene.buildIndex == 0)
        {
            Destroy(gameObject);
        }
    }
    public void InitializePutData(PlayerData Data, TSVLoaderSample.SampleData playerData, Sprite playerSprite)
    {
        currentStage = (int)playerData.CurrentStage;
        Data.character_ID = playerData.Character_ID;
        Data.characterClass = playerData.Character_Class;
        Data.characterName = playerData.Character_Name;
        Data.characterGrade = playerData.Character_Grade;

        Data.equip_item = playerData.Equip_Item;
        Data.equip_level = playerData.Equip_Level;
        
        Data.training_type = playerData.Training_type;
        Data.training_level = playerData.Training_Level;
        
        Data.skill_possed[0] = playerData.Skill_Possed1;
        Data.skill_possed[1] = playerData.Skill_Possed2;

        Data.attackDamage = playerData.Attack;
        Data.health = playerData.Health;
        Data.attackSpeed = playerData.Attack_Speed;
        Data.criticalProbability = playerData.Critical_Probability;
        Data.playerUISprite = playerSprite;
    }
}
