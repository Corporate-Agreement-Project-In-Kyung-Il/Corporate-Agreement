using System.Collections.Generic;
using System.Linq;
using _03.Script.엄시형.Monster;

namespace _03.Script.엄시형.Stage.DTO
{
    public struct StageInfoDTO
    {
        public List<AreaPatternDTO> AreaPatternList;
        public List<MonsterType> SpawnMonsterTypeList;
        public AreaPatternDTO BossAreaPattern;
        public int MaxStageId;
        
        public StageInfoDTO(List<AreaPatternDTO> areaPatternList, List<MonsterType> spawnMonsterTypeList, AreaPatternDTO bossAreaPattern, int maxStageId)
        {
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