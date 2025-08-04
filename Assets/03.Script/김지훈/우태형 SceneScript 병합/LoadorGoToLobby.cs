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
    private Player[] players;

    private void Start()
    {
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
            TSVLoaderSample.OverwritePlayerData(players[i].data);
        }
        
        if (index.Equals(0))
        {
            //살아나
            for (int i = 0; i < players.Length; i++)
            {
                players[i].initialSetPlayerStats(players[i].data);
                players[i].gameObject.SetActive(true);
                players[i].ChangeState(CharacterState.Run);
            }
        }
        else
        {
            //돌아가
            GameManager.Instance.LoadScene(0, this.gameObject);
        }
    }
}
