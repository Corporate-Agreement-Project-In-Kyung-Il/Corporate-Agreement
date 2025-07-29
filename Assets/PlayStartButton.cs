using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayStartButton : MonoBehaviour
{
    public PlayerList PlayerList;
    public CharacterChoosing characterChoosing;

    private void Start()
    {
        characterChoosing=GetComponent<CharacterChoosing>();
    }

    // 버튼이 눌렸을 때 호출되는 함수
    public void OnClickStartButton()
    {
        if (PlayerList.CharacterIDs[0] == 0 && PlayerList.CharacterIDs[1] == 0 && PlayerList.CharacterIDs[2] == 0)
        {
            StartCoroutine(ShowNo3ClassPanel());
            return;
        }

        
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
    
}