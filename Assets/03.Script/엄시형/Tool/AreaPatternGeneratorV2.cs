using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using _03.Script.엄시형.Stage;
using _03.Script.엄시형.Stage.DTO;
using _03.Script.엄시형.Stage.V2;
using _03.Script.엄시형.Util;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace _03.Script.엄시형.Tool.V2
{
#if UNITY_EDITOR
    // 플레이시 게임오브젝트 생성시 자동 삭제가 안됨
    // [ExecuteInEditMode]
#endif
    public sealed class AreaPatternGenerator : MonoBehaviour
    {
#if UNITY_EDITOR
        // TODO : AreaPattern말고 StageInfo로 변경해야함
        // [ReadOnly]
        [SerializeField] public GameObject m_PointPrefab;

        [SerializeField] public AreaTilemapTableSO m_AreaTilemapTable;
        [SerializeField] private StagePatternTableSO m_StagePatternTable;

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

        // [Header("스테이지 정보 리스트")] [SerializeField]
        private List<StageInfoDTO> m_StageInfoList = new List<StageInfoDTO>();
        // [SerializeField] private List<AreaPattern>
        
        
        // TODO : Id에 따라 변하게
        [SerializeField]
        private int m_CurIdx = 0;

        private int m_PrevIdx = 0;

        private readonly List<GameObject> m_PointObjectList = new List<GameObject>();
        // private readonly AreaPatternPersistenceManager m_PersistenceManager
        // = new AreaPatternPersistenceManager();
        // private readonly StageInfoPersistMgr m_StagePersistMgr = new StageInfoPersistMgr();
        
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
            m_AreaTilemapTable.Init();
            var themeAreaList = m_AreaTilemapTable.GetThemeAreaList(StageTheme.Grass);
            
            m_Tilemap = themeAreaList.First().Tilemap;
            m_Tilemap = Instantiate(m_Tilemap, parent: m_Grid.transform);
            List<AreaPattern> patternMonsterCount = m_StagePatternTable.GetAllPatternByCount(3);
            
            foreach (var spawnInfo in patternMonsterCount[0].MonsterSpawnInfoList)
            {
                Vector2 worldPos = m_Tilemap.transform.TransformPoint(spawnInfo.Point);
                GameObject point = Instantiate(m_PointPrefab, parent: m_Tilemap.transform);
                point.transform.position = worldPos;
                
                point.transform.localScale = Vector3.one * spawnInfo.Radius;
                m_PointObjectList.Add(point.gameObject);
            }
            
            // //TODO : 로딩때
            // if (AreaPatternPersistenceManager.TryReadFromJson(out m_StageInfoList))
            // {
            //     Debug.Log("성공");
            //     
            //     // patternId에 해당하는 타일맵 생성
            //     var patternId = m_StageInfoList[0].AreaPatternList[0].MonsterSpawnInfoList.Count;
            //     m_Tilemap = m_AreaTilemapTable.GetTilemap(StageTheme.Grass, patternId);
            //     m_Tilemap = Instantiate(m_Tilemap, parent: m_Grid.transform);
            //     
            //     foreach (var spawnInfo in 
            //              m_StageInfoList[0].AreaPatternList[m_CurIdx].MonsterSpawnInfoList)
            //     {
            //         Vector2 worldPos = m_Tilemap.transform.TransformPoint(spawnInfo.Pos);
            //         GameObject point = Instantiate(m_PointPrefab, parent: m_Tilemap.transform);
            //         point.transform.position = worldPos;
            //
            //         point.transform.localScale = Vector3.one * spawnInfo.Diameter;
            //         m_PointObjectList.Add(point.gameObject);
            //     }
            // }
            // else
            // {
            //     Debug.Log("실패");
            // }
        }

        private void OnEnable()
        {
            m_SaveBtn.onClick.AddListener(WriteAsJson);
            // m_IncreaseBtn.onClick.AddListener(Restart);
            m_IncreaseBtn.onClick.AddListener(IncreasePatternIdx);
            m_DecreaseBtn.onClick.AddListener(DecreasePatternIdx);
            m_OpenFolderBtn.onClick.AddListener(OpenFolder);

        }

        private void OnDisable()
        {
            m_SaveBtn.onClick.RemoveListener(WriteAsJson);
            // m_IncreaseBtn.onClick.AddListener(Restart);
            m_IncreaseBtn.onClick.RemoveListener(IncreasePatternIdx);
            m_DecreaseBtn.onClick.RemoveListener(DecreasePatternIdx);
            m_OpenFolderBtn.onClick.RemoveListener(OpenFolder);
        }

        void Update()
        {
            // 휠클릭 
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Vector2 worldPos = m_MainCam.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D[] hits = Physics2D.RaycastAll(worldPos, Vector2.zero);

                foreach (var hit in hits)
                {
                    if (hit.collider.gameObject.name == "PointCircle(Clone)")
                    {
                        m_PointObjectList.Remove(hit.collider.gameObject);
                        Destroy(hit.collider.gameObject);
                    }
                }
            }

            // // 포인트 오브젝트 배치 우클릭
            // if (Input.GetKeyDown(KeyCode.Mouse1))
            // {
            //     Vector2 worldPos = m_MainCam.ScreenToWorldPoint(Input.mousePosition);
            //     RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
            //
            //     if (hit && hit.collider.TryGetComponent(out Tilemap tilemap))
            //     {
            //         Vector2 localPos = m_Tilemap.transform.InverseTransformPoint(hit.point);
            //         
            //         float diameter = m_PointPrefab.transform.localScale.x;
            //     
            //         // Instantiate는 월드 좌표로 생성해 로컬좌표로 변경함
            //         GameObject point = Instantiate(m_PointPrefab, parent: m_Tilemap.transform);
            //         point.transform.localPosition = localPos;
            //
            //         SpawnInfoDTO spawnInfo = new SpawnInfoDTO(localPos, diameter);
            //     
            //         m_AreaPatternDTOList[m_CurIdx - 1].MonsterSpawnInfoList.Add(spawnInfo);
            //         m_PointObjectList.Add(point.gameObject);
            //     }
            // }
        }

        private void WriteAsJson()
        {
            // m_StagePersistMgr.WriteAsJSON(m_StageInfoList);
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
            // EditorUtility.RevealInFinder(m_StagePersistMgr.fullPath);
        }
        
        private void RepaintPoints()
        {
            if (m_StageInfoList[m_CurIdx].AreaPatternList.Count < m_CurIdx)
            {
                return;
                
                foreach (var pointObj in m_PointObjectList)
                {
                    Destroy(pointObj);
                }

                // m_PointObjectList.Clear();
                // m_StageInfoList[m_CurIdx].AreaPatternList.Add(new AreaPatternDTO(m_CurIdx));
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
                    = m_StageInfoList[m_CurIdx].AreaPatternList[m_CurIdx].MonsterSpawnInfoList.Count;
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
        
        // 저장버튼
        // 초기화버튼
        // AreaPattern Id-- 버튼
        // AreaPattern Id++ 버튼
        // 폴더 열기 버튼
    }
# endif
}
