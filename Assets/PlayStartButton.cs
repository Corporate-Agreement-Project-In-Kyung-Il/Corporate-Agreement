using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayStartButton : MonoBehaviour
{
    public PlayerList PlayerList;
    public PlayerData[] PlayerData;
    public CharacterChoosing characterChoosing;
    public Sprite[] characterSprites;
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
                    PlayerList.Instance.InitializePutData(PlayerData[i], sampleList[j], characterSprites[j]);
                }
            }
        }
        //(int)sampleList[0].CurrentStage;
    }
    
    
    
}