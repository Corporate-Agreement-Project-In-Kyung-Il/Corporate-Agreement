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
//

    public int Selection_ID;// 여기다 스킬 선택했을때 넣어주시면 됩니다. 
    
    private ISkillID[] skills;

    public void SetSelectionID(int id)
    {
        Selection_ID = id;
    }
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
        ConnectSkills();
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
            for (int j = 0; j < skills.Length; j++)
            {
                if (players[i].data.skill_possed[0] == skills[j].SkillID)
                {
                    players[i].skills[0] = skills[j];
                }

                if (players[i].data.skill_possed[1] == skills[j].SkillID)
                {
                    players[i].skills[1] = skills[j];
                }
            }
        }
    }
}