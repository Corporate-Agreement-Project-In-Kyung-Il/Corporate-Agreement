using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRevival : MonoBehaviour
{
    public static Player[] players;
    private  void Start()
    {
        players = GetComponentsInChildren<Player>();
    }

    private void OnEnable()
    {
        StageEvent.stageClearEvent += ResetPlayerStats;
    }

    private void OnDisable()
    {
        StageEvent.stageClearEvent -= ResetPlayerStats;
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

