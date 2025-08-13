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
        A,
        B,
        C,
    }
    
    [Serializable]
    public sealed class StageInfo
    {
        public int AreaCount => m_SpawnMonsterCounts.Length;
        public int MaxStage => m_MaxStageId;
        public int[] SpawnMonsterCounts => m_SpawnMonsterCounts;
        
        [Header("Max Stage ID 1~15까지면 15입력")]
        [SerializeField] private int m_MaxStageId;
        [SerializeField] private int[] m_SpawnMonsterCounts;
        
        public StageInfo(int[] spawnMonsterCounts
            , StageTheme theme
            , MonsterType monsterType
            , int mMaxStageId)
        {
            m_SpawnMonsterCounts = spawnMonsterCounts;
            m_MaxStageId = mMaxStageId;
        }
    }
}