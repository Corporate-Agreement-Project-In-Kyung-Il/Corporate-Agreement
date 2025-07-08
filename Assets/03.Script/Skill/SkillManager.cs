using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public interface ISkillID
{
    public int SkillID { get; }
}

public class SkillManager : MonoBehaviour
{
    [SerializeField] private Player_jin[] players;
    [SerializeField] private SkillSO skill;

    private void Awake()
    {
    }

    void Start()
    {
    }


    void Update()
    {
    }

    public void FindPlayers()
    {
        players = FindObjectsOfType<Player_jin>();
    }

    public void ConnectSkills()
    {
        for (int i = 0; i < players.Length; i++)
        {
            for (int j = 0; j < skill.SkillID.Length; j++)
            {
                if (players[i].SkillID[0] == skill.SkillID[j].SkillID)
                {
                    
                }
                if (players[i].SkillID[1] == skill.SkillID[j].SkillID)
                {
                    
                }
            }
        }
    }
}