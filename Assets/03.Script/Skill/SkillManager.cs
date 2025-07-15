using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public interface ISkillID
{
    public int SkillID { get; set; }

    public void SetSkillID();
}

public class SkillManager : MonoBehaviour
{
    [SerializeField] 
    private Player[] m_Players;
    
    [SerializeField]
    private ScriptableObject[] m_OriginSkillObjects;
    
    [SerializeField] 
    private ScriptableObject[] m_SkillObjects;

    
    [SerializeField]
    private OptionChoice_SkillOption m_SkillOption;

    [Tooltip("현재 선택지에서 선택된 Skill ID")]
    public int selectionID;// 여기다 스킬 선택했을때 넣어주시면 됩니다. 
    
    private ISkillID[] m_Skills;

    public void SetSelectionID(int id)
    {
        selectionID = GameManager.Instance.optionButtons[id].selectID;
        SkillEnchant();
    }
    private void Awake()
    {
        FindPlayers();
        List<ScriptableObject> clonedList = new();
        foreach (var origin in m_OriginSkillObjects)
        {
            var clone = Instantiate(origin); // ScriptableObject 복사본 생성
            clonedList.Add(clone);
        }
        m_SkillObjects = clonedList.ToArray();
        // skillObjects 안에 들어있는 ScriptableObject 중 ISkillID를 구현한 것만 필터링
        List<ISkillID> temp = new List<ISkillID>();
        foreach (var obj in m_SkillObjects)
        {
            if (obj is ISkillID id)
            {
                temp.Add(id);
            }
        }

        m_Skills = temp.ToArray();

        foreach (var skill in m_Skills)
        {
            skill.SetSkillID();
        }
        ConnectSkills();
    }

    

    void Update()
    {
        
    }

    public void FindPlayers()
    {
        m_Players = FindObjectsOfType<Player>();
    }

    public void SetSkillSo(OptionChoice_SkillOption skillOption)
    {
        m_SkillOption = skillOption;
    }

    public void ConnectSkills()
    {
        for (int i = 0; i < m_Players.Length; i++)
        {
            for (int j = 0; j < m_Skills.Length; j++)
            {
                if (m_Players[i].data.skill_possed[0] == m_Skills[j].SkillID)
                {
                    m_Players[i].skills[0] = m_Skills[j];
                }
    
                if (m_Players[i].data.skill_possed[1] == m_Skills[j].SkillID)
                {
                    m_Players[i].skills[1] = m_Skills[j];
                }
            }
        }
    }
    public void SkillEnchant()
    {
        var a = m_SkillOption.GetValue(selectionID);
    
        Debug.Log($"Skill_ID: {a.Skill_ID}, Selection_Level: {a.Selection_Level}, Description: {a.Description}, " +
                  $"Cooldown_Reduction: {a.Cooldown_Reduction}, Duration_Increase: {a.Duration_Increase}, " +
                  $"Activation_Rate_Increase: {a.Activation_Rate_Increase}, Damage_Increase: {a.Damage_Increase}, " +
                  $"Skill_LvUP: {a.Skill_LvUP}");
        
        foreach (var player in m_Players)
        {
            for (int i = 0; i < player.skills.Length; i++)
            {
                if (player.data.skill_possed[i] == a.Skill_ID)
                {
                    if (player.skills[i] is ActiveSkillSO active)
                    {
                        active.Skill_current_LV+= a.Skill_LvUP;
                        Debug.Log($"▶ {player.name}의 Skill {active.SkillID} 레벨이 {a.Skill_LvUP} 만큼 증가 → 현재 레벨: {active.Skill_current_LV}");
                     
                        active.Skill_Cooldown-= a.Cooldown_Reduction;
                        Debug.Log($"▶ {player.name}의 Skill {active.SkillID} 쿨타임이 {a.Cooldown_Reduction} 만큼 감소 → 현재 쿨타임: {active.Skill_Cooldown}");
                        
                        active.Skill_Damage+= a.Damage_Increase;
                        Debug.Log($"▶ {player.name}의 Skill {active.SkillID} 데미지가 {a.Damage_Increase} 만큼 증가 → 현재 데미지: {active.Skill_Damage}");

                    }

                    if (player.skills[i] is BuffSO buff)
                    {
                        buff.Skill_current_LV += a.Skill_LvUP;
                        Debug.Log($"▶ {player.name}의 Skill {buff.Skill_ID} 레벨이 {a.Skill_LvUP} 만큼 증가 → 현재 레벨: {buff.Skill_current_LV}");
                        buff.Skill_Cooldown -= a.Cooldown_Reduction;
                        Debug.Log($"▶ {player.name}의 Skill {buff.Skill_ID} 쿨타임이 {a.Cooldown_Reduction} 만큼 감소 → 현재 쿨타임: {buff.Skill_Cooldown}");
                        buff.Skill_Duration += a.Duration_Increase;
                        Debug.Log($"▶ {player.name}의 Skill {buff.Skill_ID} 지속시간이 {a.Duration_Increase} 만큼 증가 → 현재 지속시간: {buff.Skill_Duration}");
                        buff.Skill_Activation_Rate+= a.Activation_Rate_Increase;
                        Debug.Log($"▶ {player.name}의 Skill {buff.Skill_ID} 발동확률이 {a.Activation_Rate_Increase} 만큼 증가 → 현재 발동확률: {buff.Skill_Activation_Rate}");
                    }

                   
                }
            }
        }
        
    }


}