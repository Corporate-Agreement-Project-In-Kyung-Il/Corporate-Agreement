using System;
using System.Collections.Generic;
using UnityEngine;

public class SkllManager_GPT : MonoBehaviour
{
    [SerializeField] private Player_jin[] players;
    [SerializeField] private ScriptableObject[] skillObjects;
    [SerializeField] private SkillBase[] skillPrefabs;

    private Dictionary<int, ISkillID> _skillById;
    private Dictionary<int, SkillBase> _prefabById;

    private void Awake()
    {
        FindPlayers();
        InitializeSkillMappings();
    }

    private void Start()
    {
        ConnectSkills();
    }

    private void FindPlayers()
    {
        players = FindObjectsOfType<Player_jin>();
    }

    /// <summary>
    /// skills 배열과 skillPrefabs 배열을 ID → 객체 사전으로 초기화
    /// </summary>
    private void InitializeSkillMappings()
    {
        // ISkillID 매핑
        _skillById = new Dictionary<int, ISkillID>();
        foreach (var obj in skillObjects)
        {
            if (obj is ISkillID skill)
            {
                skill.SetSkillID();
                _skillById[skill.SkillID] = skill;
            }
        }

        // SkillBase 프리팹 매핑
        _prefabById = new Dictionary<int, SkillBase>();
        foreach (var prefab in skillPrefabs)
        {
            _prefabById[prefab.SkillID] = prefab;
        }
    }

    /// <summary>
    /// 각 플레이어의 skill_possed 배열에 맞춰 skills, skillPrefab을 할당
    /// </summary>
    private void ConnectSkills()
    {
        foreach (var player in players)
        {
            // List<int>로 받기
            List<int> possessedIds = player.data.skill_possed;
        
            for (int slot = 0; slot < possessedIds.Count; slot++)
            {
                int skillId = possessedIds[slot];

                // ISkillID 할당
                if (_skillById.TryGetValue(skillId, out var skill))
                {
                    player.skills[slot] = skill;
                }

                // SkillBase 프리팹 할당
                if (_prefabById.TryGetValue(skillId, out var prefab))
                {
                    player.skillPrefab[slot] = prefab;
                }
            }
        }
    }
}
