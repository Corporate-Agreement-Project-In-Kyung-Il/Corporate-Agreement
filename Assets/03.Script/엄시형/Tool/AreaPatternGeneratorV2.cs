using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using _03.Script.엄시형.Stage;
using _03.Script.엄시형.Stage.DTO;
using _03.Script.엄시형.Stage.V2;
using _03.Script.엄시형.Util;
using _03.Script.엄시형.Util.V2;
using Unity.Collections;
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
    public sealed class AreaPatternGenerator : MonoBehaviour
    {
        [SerializeField] public GameObject m_PointPrefab;

        [SerializeField] private StagePatternTableSO m_StagePatternTable;

        [SerializeField] private Grid m_Grid;
        [SerializeField] private Camera m_MainCam;
        
        [SerializeField] private Button m_SaveBtn;
        [SerializeField] private Button m_AddBtn;
        [SerializeField] private Button m_DecreaseBtn;
        [SerializeField] private Button m_IncreaseBtn;
        [SerializeField] private Button m_OpenFolderBtn;

        [SerializeField] private Tilemap m_Tilemap;
        
        // TODO : Id에 따라 변하게
        // [SerializeField]
        private int m_CurIdx = 0;

        private int m_PrevIdx = 0;

        private readonly List<GameObject> m_PointObjectList = new List<GameObject>();
        private AreaPattern m_AreaPattern;
        
        [Conditional("UNITY_EDITOR")]
        private void OnValidate()
        {
            Debug.Assert(m_PointPrefab != null, "mPointPrefab이 빠졌습니다");
            Debug.Assert(m_MainCam != null, "mMainCam이 빠졌습니다");
            // Debug.Assert(m_AreaTilemapTable != null, "m_AreaTilemapTable != null");
            
            if (m_CurIdx != m_PrevIdx)
            {
                Debug.Log("패턴 ID가 변경되었습니다: " + m_CurIdx);

                // RepaintPoints();
            }
            
            m_PrevIdx = m_CurIdx;
        }

        private void Awake()
        {
            PaintPoints(m_CurIdx);
        }

        private void OnEnable()
        {
            m_SaveBtn.onClick.AddListener(Save);
            m_AddBtn.onClick.AddListener(Add);
            m_IncreaseBtn.onClick.AddListener(IncreasePatternIdx);
            m_DecreaseBtn.onClick.AddListener(DecreasePatternIdx);
            m_OpenFolderBtn.onClick.AddListener(OpenFolder);

        }

        private void OnDisable()
        {
            m_SaveBtn.onClick.RemoveListener(Save);
            m_AddBtn.onClick.RemoveListener(Add);
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
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Vector2 worldPos = m_MainCam.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
            
                if (hit && hit.collider.TryGetComponent(out Tilemap tilemap))
                {
                    Vector2 localPos = m_Tilemap.transform.InverseTransformPoint(hit.point);
                    
                    // Instantiate는 월드 좌표로 생성해 로컬좌표로 변경함
                    GameObject point = Instantiate(m_PointPrefab, parent: m_Tilemap.transform);
                    point.transform.localPosition = localPos;
                    
                    m_PointObjectList.Add(point.gameObject);
                }
            }
        }

        private void Save()
        {
            m_AreaPattern = new AreaPattern(0, new List<SpawnInfo>());
            
            foreach (var go in m_PointObjectList)
            {
                Vector2 localPos = m_Tilemap.transform.InverseTransformPoint(go.transform.position);
                m_AreaPattern.MonsterSpawnInfoList.Add(new SpawnInfo(localPos, go.transform.localScale.x));
                m_StagePatternTable.AreaPatternList[m_CurIdx] = m_AreaPattern;
            }
            
            // 몬스터 수로 정렬
            m_StagePatternTable.AreaPatternList.Sort((pattern1, pattern2) =>
            {
                return pattern1.SpawnMonsterCount.CompareTo(pattern2.SpawnMonsterCount);
            });
            
            m_StagePatternTable.Save();
            m_AreaPattern = null;
        }
        
        private void IncreasePatternIdx()
        {
            int maxIndex = m_StagePatternTable.AreaPatternList.Count - 1;
            
            if (m_CurIdx < maxIndex)
            {
                m_CurIdx++;
            }
            else
            {
                m_CurIdx = 0;
            }

            m_PrevIdx = m_CurIdx;
            ClearPoints();
            PaintPoints(m_CurIdx);
        }

        private void Add()
        {
            ClearPoints();
            m_StagePatternTable.AreaPatternList.Add(new AreaPattern(0, new List<SpawnInfo>()));
            m_CurIdx = m_StagePatternTable.AreaPatternList.Count - 1;
            PaintPoints(m_CurIdx);
        }
        
        private void DecreasePatternIdx()
        {
            if (m_CurIdx > 0)
            {
                m_CurIdx--;
            }
            else
            {
                m_CurIdx = m_StagePatternTable.AreaPatternList.Count - 1;
            }
        
            m_PrevIdx = m_CurIdx;
            ClearPoints();
            PaintPoints(m_CurIdx);
        }
        
        private void OpenFolder()
        {
            string fullPath = Path.Combine(Application.dataPath
                , "05.DataTable"
                , "AreaPattern.json");
            
            EditorUtility.RevealInFinder(fullPath);
        }
        
        /// <summary>
        /// 패턴인덱스에 해당하는 스폰 포인트 그림
        /// </summary>
        /// <param name="index"></param>
        private void PaintPoints(int index)
        {
            foreach (var spawnInfo in m_StagePatternTable.AreaPatternList[index].MonsterSpawnInfoList)
            {
                Vector2 worldPos = m_Tilemap.transform.TransformPoint(spawnInfo.Point);
                GameObject point = Instantiate(m_PointPrefab, parent: m_Tilemap.transform);
                
                point.transform.position = worldPos;
                point.transform.localScale = Vector3.one * spawnInfo.Radius;
                
                m_PointObjectList.Add(point.gameObject);
            }
        }
        
        /// <summary>
        /// 빨간색 점 제거
        /// </summary>
        private void ClearPoints()
        {
            foreach (var point in m_PointObjectList)
            {
                Destroy(point);
            }
            
            m_PointObjectList.Clear();
        }
    }
# endif
}
