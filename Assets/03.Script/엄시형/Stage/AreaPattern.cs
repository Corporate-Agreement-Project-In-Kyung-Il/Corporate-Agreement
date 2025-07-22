using System;
using System.Collections.Generic;
using UnityEngine;

namespace _03.Script.엄시형.Stage.DTO
{
    [Serializable]
    public sealed class AreaPattern
    {
        
        [field: SerializeField]
        public int PatternId { get; private set; }
        
        [field: SerializeField]
        public List<SpawnInfo> MonsterSpawnInfoList { get; set; } = new List<SpawnInfo>();
        
        public int SpawnMonsterCount => MonsterSpawnInfoList.Count; 
        
        public AreaPattern() {}
        
        public AreaPattern(int id, List<SpawnInfoDTO> SpawnInfoDtoList)
        {
            PatternId = id;
            
            MonsterSpawnInfoList = new List<SpawnInfo>(SpawnInfoDtoList.Count);

            foreach (var dto in SpawnInfoDtoList)
            {
                MonsterSpawnInfoList.Add(dto.ToSpawnInfo());
            }
        }
        
        public AreaPattern(int id, List<SpawnInfo> SpawnInfoList)
        {
            PatternId = id;

            MonsterSpawnInfoList = new List<SpawnInfo>(SpawnInfoList);
        }
    }
}