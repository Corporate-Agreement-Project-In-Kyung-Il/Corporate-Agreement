using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using _03.Script.엄시형.Stage;
using _03.Script.엄시형.Stage.DTO;
using _03.Script.엄시형.Util;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace _03.Script.엄시형.Tool
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
        [SerializeField] private List<AreaPatternDTO> m_AreaPatternDTOList = new List<AreaPatternDTO>();
        
        [Header("스테이지 정보 리스트")]
        [SerializeField] private List<StageInfoDTO> m_StageInfoList = new List<StageInfoDTO>();
        
        // TODO : Id에 따라 변하게
        [FormerlySerializedAs("m_PatternId")] [SerializeField] private int m_CurIdx = 1;
        
        private int m_PrevIdx = 1;
        
        private readonly List<GameObject> m_PointObjectList = new List<GameObject>();
        // private readonly AreaPatternPersistenceManager m_PersistenceManager = new AreaPatternPersistenceManager();
        
        // 저장버튼
        // 초기화버튼
        // AreaPattern Id-- 버튼
        // AreaPattern Id++ 버튼
        // 폴더 열기 버튼
        
        [Conditional("UNITY_EDITOR")]
        private void OnValidate()
        {
            Debug.Assert(m_PointPrefab != null, "mPointPrefab이 빠졌습니다");
            Debug.Assert(m_MainCam != null, "mMainCam이 빠졌습니다");
            Debug.Assert(m_AreaTilemapTable != null, "m_AreaTilemapTable != null");
            
            if (m_CurIdx != m_PrevIdx)
            {
                Debug.Log("패턴 ID가 변경되었습니다: " + m_CurIdx);

                RepaintPoints();
            }
               
            m_PrevIdx = m_CurIdx;
        }
        
        
        private void Awake()
        {
            if (AreaPatternPersistenceManager.TryReadFromJson(out m_AreaPatternDTOList))
            {
                // patternId에 해당하는 타일맵 생성
                var patternId = m_AreaPatternDTOList[0].PatternId;
                m_Tilemap = m_AreaTilemapTable.GetTilemapOrNull(patternId);
                m_Tilemap = Instantiate(m_Tilemap, parent: m_Grid.transform);
                
                foreach (var spawnInfo in m_AreaPatternDTOList[m_CurIdx - 1].MonsterSpawnInfoList)
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
                m_AreaPatternDTOList.Add(new AreaPatternDTO(m_CurIdx));
            }
            
        }

        private void OnEnable()
        {
            m_SaveBtn.onClick.AddListener(Save);
            m_ResetBtn.onClick.AddListener(ResetCurPattern);
            m_DecreaseBtn.onClick.AddListener(DecreasePatternId);
            m_IncreaseBtn.onClick.AddListener(IncreasePatternId);
            m_OpenFolderBtn.onClick.AddListener(OpenFolder);
        }

        private void OnDisable()
        {
            m_SaveBtn.onClick.RemoveListener(Save);
            m_ResetBtn.onClick.RemoveListener(ResetCurPattern);
            m_DecreaseBtn.onClick.RemoveListener(DecreasePatternId);
            m_IncreaseBtn.onClick.RemoveListener(IncreasePatternId);
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

            // 포인트 오브젝트 배치 우클릭
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Vector2 worldPos = m_MainCam.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

                if (hit && hit.collider.TryGetComponent(out Tilemap tilemap))
                {
                    Vector2 localPos = m_Tilemap.transform.InverseTransformPoint(hit.point);
                    
                    float diameter = m_PointPrefab.transform.localScale.x;
                
                    // Instantiate는 월드 좌표로 생성해 로컬좌표로 변경함
                    GameObject point = Instantiate(m_PointPrefab, parent: m_Tilemap.transform);
                    point.transform.localPosition = localPos;

                    SpawnInfoDTO spawnInfo = new SpawnInfoDTO(localPos, diameter);
                
                    m_AreaPatternDTOList[m_CurIdx - 1].MonsterSpawnInfoList.Add(spawnInfo);
                    m_PointObjectList.Add(point.gameObject);
                }
            }
        }

        private void ResetCurPattern()
        {
            foreach (var pointObj in m_PointObjectList)
            {
                Destroy(pointObj); // OnValidate에서는 Destroy 대신 DestroyImmediate 권장
            }

            m_PointObjectList.Clear();
        }

        private void DecreasePatternId()
        {
            if (m_CurIdx > 1)
            {
                m_CurIdx--;
            }
            else if (m_AreaPatternDTOList.Count > 0)
            {
                m_CurIdx = m_AreaPatternDTOList.Count;
            }

            m_PrevIdx = m_CurIdx;
            RepaintPoints();
        }

        private void IncreasePatternId()
        {
            int maxIndex = m_AreaPatternDTOList.Count; // 0부터 시작하므로 -1

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
            
        // public void AddPattern()
        // {
        //     m_PatternId++;
        //         
        //     foreach (var pointObj in m_PointObjectList)
        //     {
        //         Destroy(pointObj);
        //     }
        //
        //     m_PointObjectList.Clear();
        //     m_PatternId = m_AreaPatternDTOList.Last().PatternId + 1;
        //     m_AreaPatternDTOList.Add(new AreaPatternDTO(m_PatternId));
        //     m_PrevId = m_PatternId;
        //     RepaintPoints();
        // }
        
        private void Save()
        {
            m_AreaPatternDTOList[m_CurIdx - 1].MonsterSpawnInfoList.Clear();

            foreach (var pointObj in m_PointObjectList)
            {
                Vector2 localPos = m_Tilemap.transform.InverseTransformPoint(pointObj.transform.position);
                SpawnInfoDTO spawnInfo = new SpawnInfoDTO(localPos, pointObj.transform.localScale.x);
                m_AreaPatternDTOList[m_CurIdx - 1].MonsterSpawnInfoList.Add(spawnInfo);
            }

            AreaPatternPersistenceManager.WriteAsJSON(m_AreaPatternDTOList);
        }

        private void OpenFolder()
        {
            EditorUtility.RevealInFinder(AreaPatternPersistenceManager.fullPath);
        }
        
        private void RepaintPoints()
        {
            if (m_AreaPatternDTOList.Count < m_CurIdx)
            {
                foreach (var pointObj in m_PointObjectList)
                {
                    Destroy(pointObj);
                }

                m_PointObjectList.Clear();
                m_AreaPatternDTOList.Add(new AreaPatternDTO(m_CurIdx));
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
                
                var patternId = m_AreaPatternDTOList[m_CurIdx - 1].PatternId;
                m_Tilemap = m_AreaTilemapTable.GetTilemapOrNull(patternId);
                m_Tilemap = Instantiate(m_Tilemap, parent: m_Grid.transform);
                
                foreach (var spawnInfo in m_AreaPatternDTOList[m_CurIdx - 1].MonsterSpawnInfoList)
                {
                    Vector2 worldPos = m_Tilemap.transform.TransformPoint(spawnInfo.Pos);
                    GameObject point = Instantiate(m_PointPrefab, parent: m_Tilemap.transform);
                    point.transform.position = worldPos;

                    point.transform.localScale = Vector3.one * spawnInfo.Diameter;
                    m_PointObjectList.Add(point.gameObject);
                }
            }
        }
    }
    
    
}
