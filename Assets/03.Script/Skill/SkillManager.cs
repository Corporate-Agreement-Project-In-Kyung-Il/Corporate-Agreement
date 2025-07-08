using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public interface ISkillID
{
    public int SkillID { get; set; }

    public void SetSkillID();
}

public class SkillManager : MonoBehaviour
{
    [SerializeField] private Player_jin[] players;
    [SerializeField] private ScriptableObject[] skillObjects;
    private ISkillID[] skills;

    private void Awake()
    {
        FindPlayers();

        // skillObjects 안에 들어있는 ScriptableObject 중 ISkillID를 구현한 것만 필터링
        List<ISkillID> temp = new List<ISkillID>();
        foreach (var obj in skillObjects)
        {
            if (obj is ISkillID id)
            {
                temp.Add(id);
            }
        }
        skills = temp.ToArray();
        
        foreach (var skill in skills)
        {
            skill.SetSkillID();
        }
    }

    void Start()
    {
        ConnectSkills();
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
        Debug.Log("1");
        for (int i = 0; i < players.Length; i++)
        {
            Debug.Log("2");
            for (int j = 0; j < skills.Length; j++)
            {
                Debug.Log("3");
               
                if (players[i].data.skill_possed[0] == skills[j].SkillID)
                {
                    Debug.Log("0");
                    players[i].skills[0] = skills[j];
                   
                    Debug.Log(players[i].data.skill_possed[0]);
                    Debug.Log(skills[j].SkillID);
                }
                
                
                if (players[i].data.skill_possed[1] == skills[j].SkillID)
                {
                    Debug.Log(players[i].data.skill_possed[1]);
                    Debug.Log(skills[j].SkillID);
                    
                    players[i].skills[1] = skills[j];
                }
            }
        }

        Debug.Log("5");
        Debug.Log(players[0].SkillID);
    }
}