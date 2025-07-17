using System;
using System.Collections.Generic;
using UnityEngine;

namespace _03.Script.엄시형.Stage.DTO
{
    /// <summary>
    /// 데이터 교환용 클래스
    /// 다른용도 사용금지
    /// </summary>
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