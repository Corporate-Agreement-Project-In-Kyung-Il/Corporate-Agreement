using System;
using _03.Script.엄시형.Stage.DTO;
using Unity.Collections;
using UnityEngine;
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
        [SerializeField] private AreaPattern mAreaPattern;
        
        private void Awake()
        {
            Debug.Assert(mTilemap != null, "Tilemap이 빠졌습니다");
            Debug.Assert(mPointPrefab != null, "mPointPrefab이 빠졌습니다");
            Debug.Assert(mMainCam != null, "mMainCam이 빠졌습니다");
            Debug.Assert(mAreaPattern != null, "mAreaPattern이 빠졌습니다");
            
            mAreaPattern.MonsterSpawnPoints.Clear();
        }

        // Update is called once per frame
        void Update()
        {
            // TODO : CSV로 저장
            
            if (Input.GetMouseButtonDown(1))
            {
                Vector3 worldPos = mMainCam.ScreenToWorldPoint(Input.mousePosition);
                worldPos.z = 0;
            
                Vector3 localPos = mTilemap.transform.InverseTransformPoint(worldPos);
            
                // Instantiate는 월드 좌표로 생성해 로컬좌표로 변경함
                GameObject point = Instantiate(mPointPrefab, parent: mTilemap.transform);
                point.transform.localPosition = localPos;
            }
        }

        private void OnGUI()
        {
            if (GUI.Button(new Rect(10, 10, 100, 40), "저장"))
            {
                Debug.Log("버튼 클릭됨!");
            }
        }
    }
}
