using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpBarManager : MonoBehaviour
{
    public GameObject hpBarPrefab;
    public Canvas uiCanvas; //World Space로 설정된 Canvas
    public float barDisplayPosition;
    
    private Camera mainCamera;
    private Dictionary<Transform, GameObject> hpBars = new Dictionary<Transform, GameObject>();
    private Plane[] cameraPlanes;
    private List<Collider2D> monsterList = new List<Collider2D>();
    
    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        monsterList = MonsterExistSystem.Instance.monsterList;
    }

    void Update()
    {
        cameraPlanes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
        //GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); //이거 Monster리스트에 담기게 Instantiate 되면, 바꾸기

        for (int i = 0; i < monsterList.Count; i++)//GameObject enemy in enemies)
        {
            if(monsterList.Count <= 0) continue;
            
            Transform enemyTransform = monsterList[i].transform;

            //if (rend == null) continue;
            bool isVisible = GeometryUtility.TestPlanesAABB(cameraPlanes, monsterList[i].bounds);

            if (isVisible)
            {
                // 없으면 생성
                if (hpBars.ContainsKey(enemyTransform).Equals(false))
                {
                    GameObject bar = Instantiate(hpBarPrefab);
                    bar.transform.SetParent(uiCanvas.transform, worldPositionStays: false);
                    
                    bar.transform.position = enemyTransform.position + Vector3.up * barDisplayPosition;
                    bar.transform.localRotation = Quaternion.identity;
                    
                    bar.GetComponent<EnemyHpBar>().target = enemyTransform;
                    
                    hpBars[enemyTransform] = bar;
                }
            }
            else
            {
                // 있으면 삭제
                if (hpBars.ContainsKey(enemyTransform))
                {
                    Destroy(hpBars[enemyTransform]);
                    hpBars.Remove(enemyTransform);
                }
            }
        }
    }
}
