using System;
using System.Collections.Generic;
using System.Linq;
using _03.Script.엄시형.Data;
using _03.Script.엄시형.Monster;
using _03.Script.엄시형.Stage.DTO;
using _03.Script.엄시형.Tool;
using UnityEngine;

namespace _03.Script.엄시형.Stage
{
    [CreateAssetMenu(fileName = "StageInfoSO", menuName = "SO/Stage/StageInfoSO", order = 0)]
    public sealed class StagePatternTableSO : ScriptableObject
    {
        private AreaPatternPersistenceManager m_AreaPerstistenceMgr = new AreaPatternPersistenceManager();
        
        public Dictionary<int, StageInfo> m_StageInfoDic = new Dictionary<int, StageInfo>();
        
        [SerializeField] private List<AreaPattern> m_AreaPatternList = new List<AreaPattern>();
        [SerializeField] private List<StageInfo> m_StageInfo = new List<StageInfo>();
        
        private void Reset()
        {
            // Init();
        }
        
        public StageInfo GetStageInfo(int stageId)
        {
            // return m_StageInfoDic[stageId];
            // return m_StageInfoDic.First().Value;
            return m_StageInfo.FirstOrDefault();
        }

        public void Init()
        {
            // TODO : 안드로이드 경로 문제
            // Dic으로 변환못함 
            
            if (m_AreaPerstistenceMgr
                .TryReadFromJson(out List<AreaPatternDTO> areaPatterns))
            {
                
                foreach (var dto in areaPatterns)
                {
                    AreaPattern pattern = dto.ToAreaPattern();
                    int stageId = pattern.GetStageId();
                    
                    if (m_StageInfoDic.ContainsKey(stageId) == false)
                    {
                        var stageInfo = new StageInfo(
                            new List<AreaPattern>(),
                            // Slime 몬스터만 일단 넣어둠
                            new List<MonsterType>()
                            {
                                MonsterType.Slime
                            },
                            // 일단 BOSS 구역은 패턴 고정
                            new AreaPattern(110004
                                , new List<SpawnInfo>()
                                {
                                    new SpawnInfo(new Vector2(0f, 15f), 0f)
                                }),
                            15);
                        
                        m_StageInfoDic.Add(stageId, stageInfo);
                        m_StageInfo.Add(stageInfo);
                    }
                    
                    m_StageInfoDic[stageId].AreaPatternList.Add(pattern);
                    
                    m_AreaPatternList.Add(pattern);
                }
            }
        }
    }
}