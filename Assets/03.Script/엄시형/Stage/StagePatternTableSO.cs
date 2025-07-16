using System;
using System.Collections.Generic;
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
        
        private void Reset()
        {
            Init();
        }
        
        public StageInfo GetStageInfo(int stageId)
        {
            return m_StageInfoDic[stageId];
        }

        public void Init()
        {
            if (m_AreaPerstistenceMgr
                .TryReadFromJson(out List<AreaPatternDTO> areaPatterns))
            {
                
                foreach (var dto in areaPatterns)
                {
                    AreaPattern pattern = dto.ToAreaPattern();
                    int stageId = pattern.GetStageId();

                    // Debug.Log("stageId" + stageId);
                    
                    if (m_StageInfoDic.ContainsKey(stageId) == false)
                    {
                        var stageInfo = new StageInfo();
                        
                        stageInfo.SpawnMonsterTypeList = new List<MonsterType>()
                        {
                            MonsterType.Slime
                        };
                        
                        stageInfo.BossAreaInfo = new AreaPattern(
                            110004
                            , new List<SpawnInfo>()
                            {
                                new SpawnInfo(
                                    new Vector2(0f, 15f)
                                    , 0f)
                            });
                        
                        m_StageInfoDic.Add(stageId, stageInfo);
                    }
                    
                    m_StageInfoDic[stageId].AreaInfoList.Add(pattern);
                    
                    m_AreaPatternList.Add(pattern);
                }
            }
        }
    }
}