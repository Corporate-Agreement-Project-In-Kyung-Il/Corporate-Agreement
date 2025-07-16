using System;
using System.Collections.Generic;
using _03.Script.엄시형.Monster;
using _03.Script.엄시형.Stage.DTO;
using _03.Script.엄시형.Tool;
using UnityEngine;

namespace _03.Script.엄시형.Stage
{
    [CreateAssetMenu(fileName = "StageInfoSO", menuName = "SO/Stage/StageInfoSO", order = 0)]
    public class StagePatternTableSO : ScriptableObject
    {
        private AreaPatternPersistenceManager m_AreaPerstistenceMgr = new AreaPatternPersistenceManager();
        
        public Dictionary<int, StageInfo> m_StageInfoDic = new Dictionary<int, StageInfo>(){};
        [SerializeField] private List<AreaPattern> m_AreaPatternList = new List<AreaPattern>();
        
        private void Reset()
        {
            if (m_AreaPerstistenceMgr
                .TryReadFromJson(out List<AreaPatternDTO> areaPatterns))
            {
                // m_StageInfoList = new List<StageInfo>(areaPatterns.Count);
                foreach (var dto in areaPatterns)
                {
                    AreaPattern pattern = dto.ToAreaPattern();
                    int stageId = pattern.GetStageId();
                    
                    if (m_StageInfoDic.ContainsKey(stageId) == false)
                    {
                        var stageInfo = new StageInfo();
                        stageInfo.SpawnMonsterTypeList = new List<MonsterType>()
                        {
                            MonsterType.Slime
                        };
                        
                        m_StageInfoDic.Add(stageId, stageInfo);
                    }
                    
                    m_StageInfoDic[stageId].AreaInfoList.Add(pattern);
                    
                    m_AreaPatternList.Add(pattern);
                }

                foreach (var kvp in m_StageInfoDic)
                {
                    Debug.Log(kvp.Key + " : " + kvp.Value);
                }
            }
        }
        
        public StageInfo GetStageInfo(int stageId)
        {
            return m_StageInfoDic[stageId];
        }

        private void Awake()
        {
            if (m_AreaPerstistenceMgr
                .TryReadFromJson(out List<AreaPatternDTO> areaPatterns))
            {
                // m_StageInfoList = new List<StageInfo>(areaPatterns.Count);
                foreach (var dto in areaPatterns)
                {
                    AreaPattern pattern = dto.ToAreaPattern();
                    int stageId = pattern.GetStageId();
                    
                    if (m_StageInfoDic.ContainsKey(stageId) == false)
                    {
                        var stageInfo = new StageInfo();
                        stageInfo.SpawnMonsterTypeList = new List<MonsterType>()
                        {
                            MonsterType.Slime
                        };
                        
                        m_StageInfoDic.Add(stageId, stageInfo);
                    }
                    
                    m_StageInfoDic[stageId].AreaInfoList.Add(pattern);
                    
                    m_AreaPatternList.Add(pattern);
                }
            }
        }
    }
}