using System;
using System.Collections.Generic;
using UnityEngine;

namespace _03.Script.엄시형.Stage.DTO
{
    [Serializable]
    public struct AreaPatternDTO
    {
        public int PatternId;
        public List<SpawnInfoDTO> MonsterSpawnInfoList;

        public AreaPatternDTO(int patternId)
        {
            PatternId = patternId;
            MonsterSpawnInfoList = new List<SpawnInfoDTO>();
        }

        public AreaPattern ToAreaPattern()
        {
            return new AreaPattern(PatternId, MonsterSpawnInfoList);
        }
    }
}