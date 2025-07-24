using System;
using System.Collections.Generic;
using System.Diagnostics;
using _03.Script.엄시형.Monster;
using _03.Script.엄시형.Stage.DTO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

namespace _03.Script.엄시형.Stage.V2
{
    public enum StageTheme
    {
        Grass,
        Perple,
    }
    
    [Serializable]
    public sealed class StageInfo
    {
        // TODO : 엑셀이나 CSV부터 읽어오기 + 에디터에 올리기
        public int AreaCount => m_SpawnMonsterCounts.Length;
        public MonsterType MonsterType => m_MonsterType;
        public int MaxStage => m_MaxStageId;
        public StageTheme Theme => m_Theme;
        public int[] SpawnMonsterCounts => m_SpawnMonsterCounts;
        
        // public AreaPattern BossAreaPattern => m_BossAreaPattern;
        
        // [Header("보스 구역 정보")]
        // [SerializeField] private AreaPattern m_BossAreaPattern;
        
        [Header("Max Stage ID 1~15까지면 15입력")]
        [SerializeField] private int m_MaxStageId;

        private MonsterType m_MonsterType;
        private int[] m_SpawnMonsterCounts;
        private StageTheme m_Theme;

        [Conditional("UNITY_EDITOR")]
        private void OnValidate()
        {
            // Debug.Assert(m_AreaPatternList.Count != 0, "mAreaInfoList 요소가 0 인스펙터 확인");
        }

        public StageInfo(int[] spawnMonsterCounts
            , StageTheme theme
            , MonsterType monsterType
            , int mMaxStageId)
        {
            m_Theme = theme;
            m_SpawnMonsterCounts = spawnMonsterCounts;
            // m_AreaPatternList = areaPatternList;
            m_MonsterType = monsterType;
            // m_BossAreaPattern = bossAreaPattern;
            m_MaxStageId = mMaxStageId;
        }
    }
}