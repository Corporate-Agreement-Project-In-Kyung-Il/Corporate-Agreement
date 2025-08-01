using System.Collections;
using System.Collections.Generic;
using BackEnd.Quobject.SocketIoClientDotNet.Client;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    [SerializeField] private GameObject EsekaiSkillPanel;
    public void LoadScene()
    {
        SceneManager.LoadScene(1);
    }

    public void OnOffEsekaiSkillPanel()
    {
        if (EsekaiSkillPanel.activeSelf)
        {
            EsekaiSkillPanel.SetActive(false);
        }
        else 
        {
            EsekaiSkillPanel.SetActive(true);
        }
    }
}
