using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayStartButton : MonoBehaviour
{
    public PlayerList PlayerList;
    public PlayerData[] PlayerData;
    public CharacterChoosing characterChoosing;

    private void Start()
    {
        characterChoosing=GetComponent<CharacterChoosing>();
    }

    // 버튼이 눌렸을 때 호출되는 함수
    public void OnClickStartButton()
    {
        if (PlayerList.CharacterIDs[0] == 0 || PlayerList.CharacterIDs[1] == 0 || PlayerList.CharacterIDs[2] == 0)
        {
            Debug.Log("ww");
            StartCoroutine(ShowNo3ClassPanel());
            return;
        }

        PutThePlayerData();

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex; 
        SceneManager.LoadScene(currentSceneIndex + 1); // 다음 씬 로드
    }
    public void OnClickBackButton()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex; 
        SceneManager.LoadScene(currentSceneIndex -1); // 다음 씬 로드
    }
    private IEnumerator ShowNo3ClassPanel()
    {
        characterChoosing.No3ClassPanel.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        characterChoosing.No3ClassPanel.SetActive(false);
    }

    private void PutThePlayerData()
    {
        var sampleList = TSVLoaderSample.SampleDataList;
        var putListID = PlayerList.Instance.CharacterIDs;

        for (int i = 0; i < putListID.Length; i++)
        {
            for (int j = 0; j < sampleList.Count; j++)
            {
                if (sampleList[j].Character_ID.Equals(putListID[i]))
                {
                    InitializePutData(PlayerData[i], sampleList[j]);
                }
            }
        }
        //(int)sampleList[0].CurrentStage;
    }

    private void InitializePutData(PlayerData Data, TSVLoaderSample.SampleData playerData)
    {
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
        
    }
    
}