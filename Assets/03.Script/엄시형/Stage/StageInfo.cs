using System;
using System.Collections.Generic;
using System.Diagnostics;
using _03.Script.엄시형.Monster;
using _03.Script.엄시형.Stage.DTO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

namespace _03.Script.엄시형.Stage
{
    [Serializable]
    public sealed class StageInfo
    {
        // TODO : 엑셀이나 CSV부터 읽어오기 + 에디터에 올리기
        public List<AreaPattern> AreaPatternList => m_AreaPatternList;
        public List<MonsterType> SpawnMonsterTypeList => m_SpawnMonsterTypeList;
        public int MaxStage => m_MaxStageId;
        public AreaPattern BossAreaPattern => m_BossAreaPattern;
        
        [Header("보스 구역 정보")]
        [SerializeField] private AreaPattern m_BossAreaPattern;
        
        [Header("Max Stage ID 1~15까지면 15입력")]
        [SerializeField] private int m_MaxStageId;

        public List<MonsterType> m_SpawnMonsterTypeList = new List<MonsterType>();
        
        [Header("구역 정보 1구역 2구역 3구역 4구역(보스스테이지)")]
        [SerializeField] private List<AreaPattern> m_AreaPatternList = new List<AreaPattern>();
        
        [Conditional("UNITY_EDITOR")]
        private void OnValidate()
        {
            Debug.Assert(m_AreaPatternList.Count != 0, "mAreaInfoList 요소가 0 인스펙터 확인");
        }

        public StageInfo(List<AreaPattern> areaPatternList
            , List<MonsterType> spawnMonsterTypeList
            , AreaPattern bossAreaPattern
            , int mMaxStageId)
        {
            m_AreaPatternList = areaPatternList;
            m_SpawnMonsterTypeList = spawnMonsterTypeList;
            m_BossAreaPattern = bossAreaPattern;
            m_MaxStageId = mMaxStageId;
        }
    }
}