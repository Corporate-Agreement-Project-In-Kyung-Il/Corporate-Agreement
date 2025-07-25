using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayStartButton : MonoBehaviour
{
    // 버튼이 눌렸을 때 호출되는 함수
    public void OnClickStartButton()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex; 
        SceneManager.LoadScene(currentSceneIndex + 1); // 다음 씬 로드
    }
    public void OnClickBackButton()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex; 
        SceneManager.LoadScene(currentSceneIndex -1); // 다음 씬 로드
    }
}