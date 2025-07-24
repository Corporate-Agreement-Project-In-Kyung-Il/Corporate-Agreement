using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using _03.Script.엄시형.Stage;
using _03.Script.엄시형.Stage.DTO;
using _03.Script.엄시형.Stage.V2;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace _03.Script.엄시형.Tool.V2
{
    public sealed class AreaPatternGenerator : MonoBehaviour
    {
        // TODO : AreaPattern말고 StageInfo로 변경해야함
        // [ReadOnly]
        [SerializeField] public GameObject m_PointPrefab;

        [SerializeField] public AreaTilemapTableSO m_AreaTilemapTable;

        // [ReadOnly]
        [SerializeField] private Grid m_Grid;
        [SerializeField] private Camera m_MainCam;

        [SerializeField] private Button m_SaveBtn;
        [SerializeField] private Button m_ResetBtn;
        [SerializeField] private Button m_DecreaseBtn;
        [SerializeField] private Button m_IncreaseBtn;
        [SerializeField] private Button m_OpenFolderBtn;

        private Tilemap m_Tilemap;

        // TODO : ID읽어와서 중복 못하게
        // [SerializeField] private List<AreaPatternDTO> m_AreaPatternDTOList
        // = new List<AreaPatternDTO>();

        [Header("스테이지 정보 리스트")] [SerializeField]
        private List<StageInfoDTO> m_StageInfoList = new List<StageInfoDTO>();

        // TODO : Id에 따라 변하게
        [SerializeField]
        private int m_CurIdx = 0;

        private int m_PrevIdx = 0;

        private readonly List<GameObject> m_PointObjectList = new List<GameObject>();
        // private readonly AreaPatternPersistenceManager m_PersistenceManager
        // = new AreaPatternPersistenceManager();
        private readonly StageInfoPersistMgr m_StagePersistMgr = new StageInfoPersistMgr();
        
        [Conditional("UNITY_EDITOR")]
        private void OnValidate()
        {
            Debug.Assert(m_PointPrefab != null, "mPointPrefab이 빠졌습니다");
            Debug.Assert(m_MainCam != null, "mMainCam이 빠졌습니다");
            Debug.Assert(m_AreaTilemapTable != null, "m_AreaTilemapTable != null");
            
            if (m_CurIdx != m_PrevIdx)
            {
                Debug.Log("패턴 ID가 변경되었습니다: " + m_CurIdx);

                // RepaintPoints();
            }
            
            m_PrevIdx = m_CurIdx;
        }

        private void Awake()
        {
            if (m_StagePersistMgr.TryReadFromJson(out m_StageInfoList))
            {
                Debug.Log("성공");
                
                // patternId에 해당하는 타일맵 생성
                var patternId = m_StageInfoList[0].AreaPatternList[0].MonsterSpawnInfoList.Count;
                m_Tilemap = m_AreaTilemapTable.GetTilemap(StageTheme.Grass, patternId);
                m_Tilemap = Instantiate(m_Tilemap, parent: m_Grid.transform);
                
                foreach (var spawnInfo in 
                         m_StageInfoList[0].AreaPatternList[m_CurIdx].MonsterSpawnInfoList)
                {
                    Vector2 worldPos = m_Tilemap.transform.TransformPoint(spawnInfo.Pos);
                    GameObject point = Instantiate(m_PointPrefab, parent: m_Tilemap.transform);
                    point.transform.position = worldPos;

                    point.transform.localScale = Vector3.one * spawnInfo.Diameter;
                    m_PointObjectList.Add(point.gameObject);
                }
            }
            else
            {
                Debug.Log("실패");
            }
        }

        private void OnEnable()
        {
            m_SaveBtn.onClick.AddListener(WriteAsJson);
            // m_IncreaseBtn.onClick.AddListener(Restart);
            m_IncreaseBtn.onClick.AddListener(IncreasePatternIdx);
            m_DecreaseBtn.onClick.AddListener(DecreasePatternIdx);
            m_OpenFolderBtn.onClick.AddListener(OpenFolder);

        }

        private void WriteAsJson()
        {
            m_StagePersistMgr.WriteAsJSON(m_StageInfoList);
        }
        
        private void IncreasePatternIdx()
        {
            int maxIndex = m_StageInfoList[m_CurIdx].AreaPatternList.Count; // 0부터 시작하므로 -1

            if (m_CurIdx < maxIndex)
            {
                m_CurIdx++;
            }
            else
            {
                m_CurIdx = 1;
            }
                
            m_PrevIdx = m_CurIdx;
            RepaintPoints();
        }
        
        private void DecreasePatternIdx()
        {
            if (m_CurIdx > 1)
            {
                m_CurIdx--;
            }
            else if (m_StageInfoList[m_CurIdx].AreaPatternList.Count > 0)
            {
                m_CurIdx = m_StageInfoList[m_CurIdx].AreaPatternList.Count;
            }

            m_PrevIdx = m_CurIdx;
            RepaintPoints();
        }
        
        private void OpenFolder()
        {
            EditorUtility.RevealInFinder(m_StagePersistMgr.fullPath);
        }
        
        private void RepaintPoints()
        {
            if (m_StageInfoList[m_CurIdx].AreaPatternList.Count < m_CurIdx)
            {
                foreach (var pointObj in m_PointObjectList)
                {
                    Destroy(pointObj);
                }

                m_PointObjectList.Clear();
                m_StageInfoList[m_CurIdx].AreaPatternList.Add(new AreaPatternDTO(m_CurIdx));
            }
            else
            {
                foreach (var pointObj in m_PointObjectList)
                {
                    Destroy(pointObj);
                }

                m_PointObjectList.Clear();
                
                // 패턴 ID에 해당하는 타일맵을 생성
                // patternId에 해당하는 타일맵 생성

                var area = m_Grid.GetComponentInChildren<Tilemap>();
                Destroy(area.gameObject);
                
                var patternId 
                    = m_StageInfoList[m_CurIdx].AreaPatternList[m_CurIdx].PatternId;
                m_Tilemap = m_AreaTilemapTable.GetTilemap(StageTheme.Grass, patternId);
                m_Tilemap = Instantiate(m_Tilemap, parent: m_Grid.transform);
                
                foreach (var spawnInfo 
                         in m_StageInfoList[m_CurIdx].AreaPatternList[m_CurIdx].MonsterSpawnInfoList)
                {
                    Vector2 worldPos = m_Tilemap.transform.TransformPoint(spawnInfo.Pos);
                    GameObject point = Instantiate(m_PointPrefab, parent: m_Tilemap.transform);
                    point.transform.position = worldPos;

                    point.transform.localScale = Vector3.one * spawnInfo.Diameter;
                    m_PointObjectList.Add(point.gameObject);
                }
            }
        }
        
        public class StageInfoPersistMgr
        {
            public readonly string fullPath;
            
            public StageInfoPersistMgr()
            {
                fullPath = Path.Combine(Application.dataPath, "05.DataTable", "StageInfo.json");
            }
            
            public void WriteAsJSON(List<StageInfoDTO> stages)
            {
                AllStageInfoDTO allStageDto = new AllStageInfoDTO(stages);
            
                string json = JsonUtility.ToJson(allStageDto);

                Debug.Log(fullPath);
                File.WriteAllText(fullPath, json);
            }
            
            public bool TryReadFromJson(out List<StageInfoDTO> areaPatternList)
            {
                areaPatternList = new List<StageInfoDTO>();

                if (File.Exists(fullPath) == false) return false;
            
                areaPatternList = JsonUtility.FromJson<AllStageInfoDTO>(
                    File.ReadAllText(fullPath)).StageInfoList;
            
                return true;
            }
        }
        
        // 저장버튼
        // 초기화버튼
        // AreaPattern Id-- 버튼
        // AreaPattern Id++ 버튼
        // 폴더 열기 버튼
    }
}
