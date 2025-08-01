using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using System.Linq;
#endif
using UnityEngine;
using UnityEngine.Serialization;

public interface ISkillID
{
    public int SkillID { get; set; }

    public void SetSkillID();
}

public class SkillManager : MonoBehaviour
{
    [SerializeField] private Player[] players;
    [SerializeField] private ScriptableObject[] Origin_skillObjects;
    [SerializeField] private ScriptableObject[] skillObjects;
    [SerializeField] private OptionChoice_SkillOption skillOption;


    public int Selection_ID; // 여기다 스킬 선택했을때 넣어주시면 됩니다. 

    private ISkillID[] skills;

    public void SetSkillOption(OptionChoice_SkillOption ingameSkillOption)
    {
        skillOption = ingameSkillOption;
    }
    public void SetSelectionID(int id)
    {
        Selection_ID = GameManager.Instance.optionButtons[id].selectID;
        SkillEnchant();
    }

    public void SetPlayers(Player[] playerFromStageManager)
    {
        players = playerFromStageManager;
        SetPlayersWhenStart();
    }
    public void SetPlayersWhenStart()
    {
        //AutoAssignSkillObjects();
        FindPlayers();
        List<ScriptableObject> clonedList = new();
        foreach (var origin in Origin_skillObjects)
        {
            var clone = Instantiate(origin); // ScriptableObject 복사본 생성
            clonedList.Add(clone);
        }

        skillObjects = clonedList.ToArray();
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


    private void Awake()
    {
        AutoAssignSkillObjects();
    }

    public void FindPlayers()
    {
        players = FindObjectsOfType<Player>();
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

    private void AutoAssignSkillObjects()
    {
        // 1) 검색할 폴더 경로 지정
        string[] skillFolders = new[]
        {
            "Assets/00.Resources/DataBase/Skills/Active",
            "Assets/00.Resources/DataBase/Skills/Buff"
        };
    
        // 2) 해당 폴더 내의 ScriptableObject 에셋 GUID만 검색
        string[] guids = AssetDatabase.FindAssets(
            "t:ScriptableObject",
            skillFolders
        );
    
        // 3) GUID → 에셋 로드 → 필터링(null 제거) → 배열 변환
        var assets = guids
            .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
            .Select(path => AssetDatabase.LoadAssetAtPath<ScriptableObject>(path))
            .Where(obj => obj != null)
            .ToArray();
    
        // 4) 배열에 할당
        Origin_skillObjects = assets;
    
        // 5) 에디터에 Dirty 표시해서 저장하도록 함
        EditorUtility.SetDirty(this);
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
                    if (player.skills[i] is ActiveSkillSO active)
                    {
                        active.Skill_current_LV += a.Skill_LvUP;
                        Debug.Log(
                            $"▶ {player.name}의 Skill {active.SkillID} 레벨이 {a.Skill_LvUP} 만큼 증가 → 현재 레벨: {active.Skill_current_LV}");

                        active.Skill_Cooldown -= a.Cooldown_Reduction;
                        Debug.Log(
                            $"▶ {player.name}의 Skill {active.SkillID} 쿨타임이 {a.Cooldown_Reduction} 만큼 감소 → 현재 쿨타임: {active.Skill_Cooldown}");

                        active.Skill_Damage += a.Damage_Increase;
                        Debug.Log(
                            $"▶ {player.name}의 Skill {active.SkillID} 데미지가 {a.Damage_Increase} 만큼 증가 → 현재 데미지: {active.Skill_Damage}");
                    }

                    if (player.skills[i] is BuffSO buff)
                    {
                        buff.Skill_current_LV += a.Skill_LvUP;
                        Debug.Log(
                            $"▶ {player.name}의 Skill {buff.Skill_ID} 레벨이 {a.Skill_LvUP} 만큼 증가 → 현재 레벨: {buff.Skill_current_LV}");
                        buff.Skill_Cooldown -= a.Cooldown_Reduction;
                        Debug.Log(
                            $"▶ {player.name}의 Skill {buff.Skill_ID} 쿨타임이 {a.Cooldown_Reduction} 만큼 감소 → 현재 쿨타임: {buff.Skill_Cooldown}");
                        buff.Skill_Duration += a.Duration_Increase;
                        Debug.Log(
                            $"▶ {player.name}의 Skill {buff.Skill_ID} 지속시간이 {a.Duration_Increase} 만큼 증가 → 현재 지속시간: {buff.Skill_Duration}");
                        buff.Skill_Activation_Rate += a.Activation_Rate_Increase;
                        Debug.Log(
                            $"▶ {player.name}의 Skill {buff.Skill_ID} 발동확률이 {a.Activation_Rate_Increase} 만큼 증가 → 현재 발동확률: {buff.Skill_Activation_Rate}");
                    }
                }
            }
        }
    }
}