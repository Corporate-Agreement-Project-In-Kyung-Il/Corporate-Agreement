using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRevival : MonoBehaviour
{
    private Player[] players;
    private void Awake()
    {
        players = GetComponentsInChildren<Player>();
    }

    private void OnEnable()
    {
        StageClearEvent.stageClearEvent += ResetPlayerStats;
    }

    private void OnDisable()
    {
        StageClearEvent.stageClearEvent -= ResetPlayerStats;
    }

    private void ResetPlayerStats()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].gameObject.activeSelf.Equals(false))
            {
                players[i].gameObject.SetActive(true);
            }
            players[i].ResetPlayerStats();
        }
    }

}

