using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using _03.Script.엄시형.Data;
using _03.Script.엄시형.Monster;
using _03.Script.엄시형.Stage.DTO;
using _03.Script.엄시형.Stage.V2;
using _03.Script.엄시형.Util.V2;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace _03.Script.엄시형.Stage
{
    [CreateAssetMenu(fileName = "StagePatternTableSO", menuName = "SO/Stage/StagePatternTableSO", order = 0)]
    public sealed class StagePatternTableSO : ScriptableObject
    {
        // private AreaPatternPersistenceManager m_AreaPerstistenceMgr = new AreaPatternPersistenceManager();
        
        // private Dictionary<int, StageInfo> m_StageInfoDic = new Dictionary<int, StageInfo>();
        
        // private Dictionary<int, List<AreaPattern>> m_AreaPatternDic = new Dictionary<int, List<AreaPattern>>();
        
        public List<AreaPattern> AreaPatternList => m_AreaPatternList;
        [SerializeField] private List<AreaPattern> m_AreaPatternList = new List<AreaPattern>();
        
        public AreaPattern GetRandomSpawnPattern(int count)
        {
            List<AreaPattern> list = GetAllPatternByCount(count);
            
            Debug.Log("AreaPattern Count : " + list.Count);
            
            AreaPattern pattern = list[Random.Range(0, list.Count)];
            
            // 랜덤으로 AreaPattern 반환
            return pattern;
        }
        
        public List<AreaPattern> GetAllPatternByCount(int count)
        {
            var list = m_AreaPatternList.FindAll(pattern =>
            {
                return pattern.MonsterSpawnInfoList.Count == count;
            });
            Debug.Log($"{list.Count} 카운트");
            // m_AreaPatternDic.TryGetValue(count, out List<AreaPattern> list);
            // Debug.Assert(list != null, $"AreaPatternDic에 {count}키에 해당하는 스폰 정보가 없습니다.");
            
            return list;
        }

        // [Conditional("UNITY_EDITOR")]
        // private void Awake()
        // {
        //     Load();
        // }

        [Conditional("UNITY_EDITOR")]
        internal void Load()
        {
            m_AreaPatternList.Clear();
            // TODO : 안드로이드 경로 문제
            // Dic으로 변환못함 
            
            string fullPath = Path.Combine(
                Application.dataPath
                , "05.DataTable"
                , "AreaPattern.json");
            
            
            if (PersistManager.TryReadFromJson(out AllAreaPatternDTO allAreaPatternDTO, fullPath))
            {
                foreach (var dto in allAreaPatternDTO.AreaPatternList)
                {
                    // 몬스터 카운트를 키로 저장
                    // int key = dto.MonsterSpawnInfoList.Count;
                    
                    // 키가 없으면 리스트 생성
                    // if (m_AreaPatternDic.ContainsKey(key) == false)
                    // {
                    //     m_AreaPatternDic[key] = new List<AreaPattern>();
                    // }

                    var areaPattern = dto.ToAreaPattern();
                    m_AreaPatternList.Add(areaPattern);
                    // m_AreaPatternDic[key].Add(areaPattern);
                }
            }
            else
            {
                Debug.LogWarning("못읽음");
            }
        }

        [Conditional("UNITY_EDITOR")]
        internal void Save()
        {
            string fullPath = Path.Combine(
                Application.dataPath
                , "05.DataTable"
                , "AreaPattern.json");

            List<AreaPatternDTO> patternDtos = new List<AreaPatternDTO>(m_AreaPatternList.Count);
            
            foreach (var pattern in m_AreaPatternList)
            {
                patternDtos.Add(pattern.ToAreaPatternDTO());
            }
            
            AllAreaPatternDTO allAreaPatternDto = new AllAreaPatternDTO(patternDtos);

            PersistManager.WriteAsJson(allAreaPatternDto, fullPath);
            EditorUtility.SetDirty(this);
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(StagePatternTableSO))]
    public sealed class StagePatternTableSOEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            StagePatternTableSO stagePatternTable = (StagePatternTableSO) target;
            
            if (GUILayout.Button("Load"))
            {
                stagePatternTable.Load();
                EditorUtility.SetDirty(stagePatternTable);
            }
            
            if (GUILayout.Button("Save"))
            {
                stagePatternTable.Save();
                EditorUtility.SetDirty(stagePatternTable);
            }
            
            if (GUILayout.Button("OpenFolder"))
            {
                string fullPath = Path.Combine(
                    Application.dataPath
                    , "05.DataTable"
                    , "AreaPattern.json");
                
                EditorUtility.RevealInFinder(fullPath);
            }
        }
    }
#endif
}