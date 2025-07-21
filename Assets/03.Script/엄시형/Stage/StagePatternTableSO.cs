using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using _03.Script.엄시형.Data;
using _03.Script.엄시형.Monster;
using _03.Script.엄시형.Stage.DTO;
using _03.Script.엄시형.Util;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace _03.Script.엄시형.Stage
{
    [CreateAssetMenu(fileName = "StageInfoSO", menuName = "SO/Stage/StageInfoSO", order = 0)]
    public sealed class StagePatternTableSO : ScriptableObject
    {
        // private AreaPatternPersistenceManager m_AreaPerstistenceMgr = new AreaPatternPersistenceManager();
        
        // private Dictionary<int, StageInfo> m_StageInfoDic = new Dictionary<int, StageInfo>();
        
        // [SerializeField] private List<AreaPattern> m_AreaPatternList = new List<AreaPattern>();
        
        private Dictionary<int, List<AreaPattern>> m_AreaPatternDic = new Dictionary<int, List<AreaPattern>>();
        
        [Conditional("UNITY_EDITOR")]
        private void Reset()
        {
            Init();
        }
        
        private List<AreaPattern> GetAllPattern(int count)
        {
            m_AreaPatternDic.TryGetValue(count, out List<AreaPattern> list);
            Debug.Assert(list != null, $"AreaPatternDic에 {count}키에 해당하는 스폰 정보가 없습니다.");
            
            return list;
        }
        
        public AreaPattern GetRandomSpawnPattern(int count)
        {
            List<AreaPattern> list = GetAllPattern(count);
            
            AreaPattern pattern = list[Random.Range(0, list.Count)];
            
            // 랜덤으로 AreaPattern 반환
            return pattern;
        }
        
        [Conditional("UNITY_EDITOR")]
        public void Init()
        {
            // TODO : 안드로이드 경로 문제
            // Dic으로 변환못함 
            
            if (AreaPatternPersistenceManager.TryReadFromJson(out List<AreaPatternDTO> areaPatterns))
            {
                foreach (var dto in areaPatterns)
                {
                    // 몬스터 카운트를 키로 저장
                    int key = dto.MonsterSpawnInfoList.Count;
                    
                    // 키가 없으면 리스트 생성
                    if (m_AreaPatternDic.ContainsKey(key) == false)
                    {
                        m_AreaPatternDic[key] = new List<AreaPattern>();
                    }
                    
                    m_AreaPatternDic[key].Add(dto.ToAreaPattern());
                }
            }
        }
    }
}