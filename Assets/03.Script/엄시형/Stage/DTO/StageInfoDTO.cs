using System;
using System.Collections.Generic;
using System.Linq;
using _03.Script.엄시형.Monster;
using _03.Script.엄시형.Stage.V2;

namespace _03.Script.엄시형.Stage.DTO
{
    [Serializable]
    public struct StageInfoDTO
    {
        public StageTheme Theme;
        public List<AreaPatternDTO> AreaPatternList;
        public List<MonsterType> SpawnMonsterTypeList;
        public AreaPatternDTO BossAreaPattern;
        public int MaxStageId;
        
        public StageInfoDTO(StageTheme theme
            , List<AreaPatternDTO> areaPatternList
            , List<MonsterType> spawnMonsterTypeList
            , AreaPatternDTO bossAreaPattern
            , int maxStageId)
        {
            Theme = theme;
            AreaPatternList = areaPatternList;
            SpawnMonsterTypeList = spawnMonsterTypeList;
            BossAreaPattern = bossAreaPattern;
            MaxStageId = maxStageId;
        }
        
        public StageInfo ToStageInfo()
        {
            var areaPatternList = AreaPatternList
                .Select(dto => dto.ToAreaPattern())
                .ToList();

            return new StageInfo(
                areaPatternList,
                SpawnMonsterTypeList,
                BossAreaPattern.ToAreaPattern(),
                MaxStageId);
        }
    }
}