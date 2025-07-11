using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using _03.Script.엄시형.Stage.DTO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Debug = UnityEngine.Debug;

namespace _03.Script.엄시형.Tool
{
    public class AreaPatternGenerator : MonoBehaviour
    {
        // [ReadOnly]
        [SerializeField] public GameObject mPointPrefab;
        
        // [ReadOnly]
        [SerializeField] private Tilemap mTilemap;
        [SerializeField] private Camera mMainCam;
        
        // TODO : ID읽어와서 중복 못하게
        [SerializeField] private List<AreaPatternDTO> mAreaPatternDTOList = new List<AreaPatternDTO>();
        
        // TODO : Id에 따라 변하게
        [SerializeField] private int mPatternId = 1;
        private int mPrevId = 1;
        
        private readonly List<GameObject> mPointObjectList = new List<GameObject>();
        private readonly AreaPatternPersistenceManager mPersistenceManager = new AreaPatternPersistenceManager();

        
        
        [Conditional("UNITY_EDITOR")]
        private void OnValidate()
        {
            Debug.Assert(mTilemap != null, "Tilemap이 빠졌습니다");
            Debug.Assert(mPointPrefab != null, "mPointPrefab이 빠졌습니다");
            Debug.Assert(mMainCam != null, "mMainCam이 빠졌습니다");
            
            if (mPatternId != mPrevId)
            {
                Debug.Log("패턴 ID가 변경되었습니다: " + mPatternId);

                if (mAreaPatternDTOList.Count < mPatternId)
                {
                    foreach (var pointObj in mPointObjectList)
                    {
                        Destroy(pointObj);
                    }

                    mPointObjectList.Clear();
                    mAreaPatternDTOList.Add(new AreaPatternDTO(mPatternId));
                }
                else
                {
                    foreach (var pointObj in mPointObjectList)
                    {
                        Destroy(pointObj);
                    }

                    mPointObjectList.Clear();

                    foreach (var spawnInfo in mAreaPatternDTOList[mPatternId - 1].MonsterSpawnInfoList)
                    {
                        Vector2 worldPos = mTilemap.transform.TransformPoint(spawnInfo.Pos);
                        GameObject point = Instantiate(mPointPrefab, parent: mTilemap.transform);
                        point.transform.position = worldPos;

                        point.transform.localScale = Vector3.one * spawnInfo.Diameter;
                        mPointObjectList.Add(point.gameObject);
                    }
                }
            }
               
            mPrevId = mPatternId;
        }
        private void Awake()
        {
            if (mPersistenceManager.TryReadFromJson(out mAreaPatternDTOList))
            {
                foreach (var spawnInfo in mAreaPatternDTOList[mPatternId - 1].MonsterSpawnInfoList)
                {
                    Vector2 worldPos = mTilemap.transform.TransformPoint(spawnInfo.Pos);
                    GameObject point = Instantiate(mPointPrefab, parent: mTilemap.transform);
                    point.transform.position = worldPos;

                    point.transform.localScale = Vector3.one * spawnInfo.Diameter;
                    mPointObjectList.Add(point.gameObject);
                }
            }
            else
            {
                mAreaPatternDTOList.Add(new AreaPatternDTO(mPatternId));
            }
        }
        
        void Update()
        {
            // 포인트 오브젝트 배치
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Vector2 worldPos = mMainCam.ScreenToWorldPoint(Input.mousePosition);
                Vector2 localPos = mTilemap.transform.InverseTransformPoint(worldPos);
                
                float diameter = mPointPrefab.transform.localScale.x;
                
                // Instantiate는 월드 좌표로 생성해 로컬좌표로 변경함
                GameObject point = Instantiate(mPointPrefab, parent: mTilemap.transform);
                point.transform.localPosition = localPos;

                SpawnInfoDTO spawnInfo = new SpawnInfoDTO(localPos, diameter);
                
                mAreaPatternDTOList[mPatternId - 1].MonsterSpawnInfoList.Add(spawnInfo);
                mPointObjectList.Add(point.gameObject);
            }
        }
        
        private void OnGUI()
        {
            int btnWidth = 60;
            int btnHeight = 30;
            int margin = 10;
            
            if (GUI.Button(new Rect(10, 10, btnWidth, btnHeight), "저장"))
            {
                mAreaPatternDTOList[mPatternId - 1].MonsterSpawnInfoList.Clear();
                
                foreach (var pointObj in mPointObjectList)
                {
                    Vector2 localPos = mTilemap.transform.InverseTransformPoint(pointObj.transform.position);
                    
                    SpawnInfoDTO spawnInfo = new SpawnInfoDTO(localPos, pointObj.transform.localScale.x);
                    mAreaPatternDTOList[mPatternId - 1].MonsterSpawnInfoList.Add(spawnInfo);
                }
                
                mPersistenceManager.WriteAsJSON(mAreaPatternDTOList);
            }
            
            if (GUI.Button(new Rect(btnWidth + margin, 10, btnWidth, btnHeight), "현재 패턴 초기화"))
            {
                foreach (var pointObj in mPointObjectList)
                {
                    Destroy(pointObj);
                }
                
                mPointObjectList.Clear();
            }
            
            if (GUI.Button(new Rect(btnWidth * 2 + margin, 10, btnWidth, btnHeight), "<<"))
            {
                
            }
            
            if (GUI.Button(new Rect(btnWidth * 3 + margin, 10, btnWidth, btnHeight), ">>"))
            {
                
            }

            GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.fontSize = 15;
            labelStyle.normal.textColor = Color.white;
            
            GUI.Label(new Rect(10, 50, 150, 40), $"현재 Pattern ID : {mPatternId}", labelStyle);
            GUI.Label(new Rect(10, 80, 150, 40), $"마지막 ID : {mAreaPatternDTOList.Last().PatternId}", labelStyle);
        }
    }

    internal class AreaPatternPersistenceManager
    {
        public void WriteAsJSON(List<AreaPatternDTO> patterns)
        {
            AllAreaPatternDTO allPatterns = new AllAreaPatternDTO(patterns);
            
            string fullPath = Path.Combine(Application.persistentDataPath, "AreaPattern.json");
            string json = JsonUtility.ToJson(allPatterns);

            Debug.Log(fullPath);
            File.WriteAllText(fullPath, json);
        }

        public bool TryReadFromJson(out List<AreaPatternDTO> areaPatternList)
        {
            areaPatternList = new List<AreaPatternDTO>();
            string fullPath = Path.Combine(Application.persistentDataPath, "AreaPattern.json");

            if (File.Exists(fullPath) == false) return false;
            
            areaPatternList = JsonUtility.FromJson<AllAreaPatternDTO>(
                    File.ReadAllText(fullPath)).AreaPatternList;
            
            return true;
        }
    }
}
