using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // 중복 생성을 방지
        }
    }
}
