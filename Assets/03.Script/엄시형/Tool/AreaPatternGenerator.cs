using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using _03.Script.엄시형.Data;
using _03.Script.엄시형.Data.V2;
using _03.Script.엄시형.Stage;
using _03.Script.엄시형.Stage.DTO;
using CsvHelper;
using CsvHelper.Configuration;
using LitJson;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

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
        [SerializeField] private AreaPatternDTO mAreaPatternDTO;
        
        private readonly List<SpawnInfoDTO> mSpawnInfoList = new List<SpawnInfoDTO>();
        private readonly List<GameObject> mPointObjectList = new List<GameObject>();
        private readonly AreaPatternPersistenceManager mPersistenceManager = new AreaPatternPersistenceManager();
        
        private void Awake()
        {
            Debug.Assert(mTilemap != null, "Tilemap이 빠졌습니다");
            Debug.Assert(mPointPrefab != null, "mPointPrefab이 빠졌습니다");
            Debug.Assert(mMainCam != null, "mMainCam이 빠졌습니다");
            Debug.Assert(mAreaPatternDTO != null, "mAreaPattern이 빠졌습니다");
        }

        // Update is called once per frame
        void Update()
        {
            // TODO : CSV로 저장
            
            if (Input.GetMouseButtonDown(1))
            {
                Vector2 worldPos = mMainCam.ScreenToWorldPoint(Input.mousePosition);
            
                Vector2 localPos = mTilemap.transform.InverseTransformPoint(worldPos);
                float radius = mPointPrefab.transform.localScale.x * 0.5f;
                
                // Instantiate는 월드 좌표로 생성해 로컬좌표로 변경함
                GameObject point = Instantiate(mPointPrefab, parent: mTilemap.transform);
                point.transform.localPosition = localPos;

                SpawnInfoDTO spawnInfo = new SpawnInfoDTO(localPos, radius);
                
                mSpawnInfoList.Add(spawnInfo);
                mPointObjectList.Add(point.gameObject);
            }
        }
        
        private void OnGUI()
        {
            if (GUI.Button(new Rect(10, 10, 100, 40), "저장"))
            {
                mAreaPatternDTO.MonsterSpawnInfoList = mSpawnInfoList;
                mPersistenceManager.WriteAsJSON(mAreaPatternDTO);
                // mPersistenceManager.WriteAsCSV(mAreaPatternDTO);
            }
            
            if (GUI.Button(new Rect(110, 10, 100, 40), "초기화"))
            {
                foreach (var pointObj in mPointObjectList)
                {
                    Destroy(pointObj);
                }

                mAreaPatternDTO = default;
                mPointObjectList.Clear();
            }
        }
    }

    internal class AreaPatternPersistenceManager
    {
        public void WriteAsJSON(AreaPatternDTO pattern)
        {
            string fullPath = Path.Combine(Application.persistentDataPath, "AreaPattern.json");
            string json = JsonUtility.ToJson(pattern);

            Debug.Log(fullPath);
            File.WriteAllText(fullPath, json);
        }

        public void WriteAsCSV(AreaPatternDTO pattern)
        {
            string fullPath = Path.Combine(Application.persistentDataPath, "AreaPattern.tsv");
            var config = new CsvConfiguration(CultureInfo.CurrentCulture);
            
            using (StreamWriter sw = new StreamWriter(fullPath))
            {
                using (var cw = new CsvWriter(sw, config))
                {
                    cw.WriteRecords(pattern.MonsterSpawnInfoList);
                }
            }
        }

        public void ReadFromJson()
        {
            string fullPath = Path.Combine(Application.persistentDataPath, "AreaPattern.json");
            AreaPatternDTO patternDto = JsonUtility.FromJson<AreaPatternDTO>(File.ReadAllText(fullPath));
            
            
        }
    }
}
