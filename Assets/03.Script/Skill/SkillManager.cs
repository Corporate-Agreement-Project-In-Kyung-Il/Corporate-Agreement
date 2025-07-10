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

    [SerializeField]
    private OptionChoice_SkillOption skillOption;
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SkillEnchant();
        }
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

    public void SkillEnchant()
    {
        var a = skillOption.GetValue(Selection_ID);
    
        Debug.Log($"Skill_ID: {a.Skill_ID}, Selection_Level: {a.Selection_Level}, Description: {a.Description}, " +
                  $"Cooldown_Reduction: {a.Cooldown_Reduction}, Duration_Increase: {a.Duration_Increase}, " +
                  $"Activation_Rate_Increase: {a.Activation_Rate_Increase}, Damage_Increase: {a.Damage_Increase}, " +
                  $"Skill_LvUP: {a.Skill_LvUP}");
        
        foreach (var player in players)
        {
            for (int i = 0; i < player.skills.Length; i++)
            {
                if (player.data.skill_possed[i] == a.Skill_ID)
                {
                    if (skills[i] is ActiveSkillSO active)
                    {
                        active.Skill_current_LV+= a.Skill_LvUP;
                        Debug.Log($"▶ {player.name}의 Skill {a.Skill_ID} 레벨이 {a.Skill_LvUP} 만큼 증가 → 현재 레벨: {active.Skill_current_LV}");
                    }

                    if (skills[i] is BuffSO buff)
                    {
                        buff.Skill_current_LV += a.Skill_LvUP;
                        Debug.Log($"▶ {player.name}의 Skill {a.Skill_ID} 레벨이 {a.Skill_LvUP} 만큼 증가 → 현재 레벨: {buff.Skill_current_LV}");
                    }

                   
                }
            }
        }
        
    }

}