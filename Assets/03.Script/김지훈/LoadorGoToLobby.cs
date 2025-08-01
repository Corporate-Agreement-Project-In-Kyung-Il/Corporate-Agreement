using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadorGoToLobby : MonoBehaviour
{
    //0번은 이어하기, 1번은  로비로 돌아가기
    private Button[] buttons;
    private PlayerData[] players;

    private void Start()
    {
        players = GameManager.Instance.playerStatAdjust.Data;
        buttons = GetComponentsInChildren<Button>();
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i; 
            buttons[i].onClick.AddListener(() => OnButtonClicked(index));
        }
    }

    private void OnButtonClicked(int index)
    {
        for (int i = 0; i < players.Length; i++)
        {
            TSVLoaderSample.OverwritePlayerData(players[i]);
        }
        
        if (index.Equals(0))
        {
            //살아나
            StageEvent.OnTriggerStageClearEvent();
            gameObject.SetActive(false);
        }
        else
        {
            //돌아가
            SceneManager.LoadScene(0);
            Destroy(AliveExistSystem.Instance.gameObject);
            Destroy(PlayerList.Instance.gameObject);
            Destroy(GameManager.Instance.gameObject);
        }
    }
}
