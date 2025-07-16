using System;
using System.Collections.Generic;
using System.Diagnostics;
using _03.Script.엄시형.Monster;
using _03.Script.엄시형.Stage.DTO;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace _03.Script.엄시형.Stage
{
    [Serializable]
    public class StageInfo
    {
        // TODO : 엑셀이나 CSV부터 읽어오기 + 에디터에 올리기
        public List<AreaPattern> AreaInfoList => m_AreaInfoList;
        public List<MonsterType> SpawnMonsterTypeList = new List<MonsterType>();
        public int MaxStage => m_MaxStageId;
        
        [Header("Max Stage ID 1~15까지면 15입력")]
        [SerializeField] private int m_MaxStageId;
        
        [Header("구역 정보 1구역 2구역 3구역 4구역(보스스테이지)")]
        [SerializeField] private List<AreaPattern> m_AreaInfoList = new List<AreaPattern>();
        
        [Header("보스 구역 정보")]
        [SerializeField] public AreaPattern BossAreaInfo;
        
        [Conditional("UNITY_EDITOR")]
        private void OnValidate()
        {
            Debug.Assert(m_AreaInfoList.Count != 0, "mAreaInfoList 요소가 0 인스펙터 확인");
        }
    }
}