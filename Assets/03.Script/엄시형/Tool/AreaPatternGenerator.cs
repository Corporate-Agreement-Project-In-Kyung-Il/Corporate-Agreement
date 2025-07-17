using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using _03.Script.엄시형.Stage;
using _03.Script.엄시형.Stage.DTO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Debug = UnityEngine.Debug;

namespace _03.Script.엄시형.Tool
{
    public sealed class AreaPatternGenerator : MonoBehaviour
    {
        // [ReadOnly]
        [SerializeField] public GameObject m_PointPrefab;
        
        // [ReadOnly]
        [SerializeField] private Tilemap m_Tilemap;
        [SerializeField] private Camera m_MainCam;
        
        // TODO : ID읽어와서 중복 못하게
        [SerializeField] private List<AreaPatternDTO> m_AreaPatternDTOList = new List<AreaPatternDTO>();
        
        [Header("스테이지 정보 리스트")]
        [SerializeField] private List<StageInfoDTO> m_StageInfoList = new List<StageInfoDTO>();
        
        // TODO : Id에 따라 변하게
        [SerializeField] private int m_PatternId = 1;
        
        private int m_PrevId = 1;
        
        private readonly List<GameObject> m_PointObjectList = new List<GameObject>();
        private readonly AreaPatternPersistenceManager m_PersistenceManager = new AreaPatternPersistenceManager();
        
        // 저장버튼
        // 초기화버튼
        // AreaPattern Id-- 버튼
        // AreaPattern Id++ 버튼
        // 폴더 열기 버튼
        
        [Conditional("UNITY_EDITOR")]
        private void OnValidate()
        {
            Debug.Assert(m_Tilemap != null, "Tilemap이 빠졌습니다");
            Debug.Assert(m_PointPrefab != null, "mPointPrefab이 빠졌습니다");
            Debug.Assert(m_MainCam != null, "mMainCam이 빠졌습니다");
            
            // if (string.IsNullOrEmpty(mFolderName))
            // {
            //     Debug.LogError("폴더명을 입력해주세요");
            // }
            
            if (m_PatternId != m_PrevId)
            {
                Debug.Log("패턴 ID가 변경되었습니다: " + m_PatternId);

                RepaintPoints();
            }
               
            m_PrevId = m_PatternId;
        }
        
        
        private void Awake()
        {
            if (m_PersistenceManager.TryReadFromJson(out m_AreaPatternDTOList))
            {
                foreach (var spawnInfo in m_AreaPatternDTOList[m_PatternId - 1].MonsterSpawnInfoList)
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
                m_AreaPatternDTOList.Add(new AreaPatternDTO(m_PatternId));
            }
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

                if (hit && hit.collider.name == "Area")
                {
                    Vector2 localPos = m_Tilemap.transform.InverseTransformPoint(hit.point);
                    
                    float diameter = m_PointPrefab.transform.localScale.x;
                
                    // Instantiate는 월드 좌표로 생성해 로컬좌표로 변경함
                    GameObject point = Instantiate(m_PointPrefab, parent: m_Tilemap.transform);
                    point.transform.localPosition = localPos;

                    SpawnInfoDTO spawnInfo = new SpawnInfoDTO(localPos, diameter);
                
                    m_AreaPatternDTOList[m_PatternId - 1].MonsterSpawnInfoList.Add(spawnInfo);
                    m_PointObjectList.Add(point.gameObject);
                }
            }
        }
        
        private void OnGUI()
        {
            int btnWidth = 400;
            int btnHeight = 20;
            int margin = 5;

            GUILayout.BeginArea(new Rect(10, 10, btnWidth, btnHeight + 10)); // 전체 버튼 영역 지정
            GUILayout.BeginHorizontal();

            // 저장 버튼
            if (GUILayout.Button("저장", GUILayout.Height(btnHeight)))
            {
                m_AreaPatternDTOList[m_PatternId - 1].MonsterSpawnInfoList.Clear();

                foreach (var pointObj in m_PointObjectList)
                {
                    Vector2 localPos = m_Tilemap.transform.InverseTransformPoint(pointObj.transform.position);
                    SpawnInfoDTO spawnInfo = new SpawnInfoDTO(localPos, pointObj.transform.localScale.x);
                    m_AreaPatternDTOList[m_PatternId - 1].MonsterSpawnInfoList.Add(spawnInfo);
                }

                m_PersistenceManager.WriteAsJSON(m_AreaPatternDTOList);
            }

            // 패턴 초기화 버튼
            if (GUILayout.Button("패턴 초기화", GUILayout.Height(btnHeight)))
            {
                foreach (var pointObj in m_PointObjectList)
                {
                    DestroyImmediate(pointObj); // OnValidate에서는 Destroy 대신 DestroyImmediate 권장
                }

                m_PointObjectList.Clear();
            }

            // <<
            if (GUILayout.Button("<<", GUILayout.Height(btnHeight), GUILayout.Width(40)))
            {
                if (m_PatternId > 1)
                {
                    
                    m_PatternId--;
                }
                else if (m_AreaPatternDTOList.Count > 0)
                {
                    m_PatternId = m_AreaPatternDTOList.Last().PatternId;
                }

                m_PrevId = m_PatternId;
                RepaintPoints();
            }

            // >>
            if (GUILayout.Button(">>", GUILayout.Height(btnHeight), GUILayout.Width(40)))
            {
                int maxPatternId = m_AreaPatternDTOList.Last().PatternId;

                if (m_PatternId < maxPatternId)
                {
                    m_PatternId++;
                }
                else
                {
                    m_PatternId = 1;
                }
                
                m_PrevId = m_PatternId;
                RepaintPoints();
            }
            
            // >>
            if (GUILayout.Button("추가", GUILayout.Height(btnHeight), GUILayout.Width(60)))
            {
                m_PatternId++;
                
                foreach (var pointObj in m_PointObjectList)
                {
                    Destroy(pointObj);
                }

                m_PointObjectList.Clear();
                m_PatternId = m_AreaPatternDTOList.Last().PatternId + 1;
                m_AreaPatternDTOList.Add(new AreaPatternDTO(m_PatternId));
                m_PrevId = m_PatternId;
                RepaintPoints();
            }
            
            if (GUILayout.Button("폴더 열기", GUILayout.Height(btnHeight), GUILayout.Width(60)))
            {
                EditorUtility.RevealInFinder(m_PersistenceManager.fullPath);
            }
            
            GUILayout.EndHorizontal();
            GUILayout.EndArea();

            GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 15,
                normal = { textColor = Color.white }
            };
            
            GUI.Label(new Rect(10, 50, 150, 40), $"현재 Pattern ID : {m_PatternId}", labelStyle);
            GUI.Label(new Rect(10, 80, 150, 40), $"마지막 ID : {m_AreaPatternDTOList.Last().PatternId}", labelStyle);
            
        }
        
        private void RepaintPoints()
        {
            if (m_AreaPatternDTOList.Count < m_PatternId)
            {
                foreach (var pointObj in m_PointObjectList)
                {
                    Destroy(pointObj);
                }

                m_PointObjectList.Clear();
                m_AreaPatternDTOList.Add(new AreaPatternDTO(m_PatternId));
            }
            else
            {
                foreach (var pointObj in m_PointObjectList)
                {
                    Destroy(pointObj);
                }

                m_PointObjectList.Clear();

                foreach (var spawnInfo in m_AreaPatternDTOList[m_PatternId - 1].MonsterSpawnInfoList)
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

    public class AreaPatternPersistenceManager
    {
        public readonly string fullPath;
        
        public AreaPatternPersistenceManager()
        {
            fullPath = Path.Combine(Application.dataPath, "05.DataTable", "AreaPattern.json");
        }
            
        public void WriteAsJSON(List<AreaPatternDTO> patterns)
        {
            AllAreaPatternDTO allPatterns = new AllAreaPatternDTO(patterns);
            
            string json = JsonUtility.ToJson(allPatterns);

            Debug.Log(fullPath);
            File.WriteAllText(fullPath, json);
        }

        public bool TryReadFromJson(out List<AreaPatternDTO> areaPatternList)
        {
            areaPatternList = new List<AreaPatternDTO>();

            if (File.Exists(fullPath) == false) return false;
            
            areaPatternList = JsonUtility.FromJson<AllAreaPatternDTO>(
                    File.ReadAllText(fullPath)).AreaPatternList;
            
            return true;
        }
    }
}
