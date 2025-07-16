using System;
using System.Collections.Generic;
using UnityEngine;

namespace _03.Script.엄시형.Stage.DTO
{
    [Serializable]
    public class AreaPattern
    {
        [field: SerializeField]
        public int PatternId { get; private set; }
        
        [field: SerializeField]
        public List<SpawnInfo> MonsterSpawnInfoList { get; private set; } = new List<SpawnInfo>();
        
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
        
        public int GetStageId()
        {
            return PatternId / 100000;
        }
    }
}