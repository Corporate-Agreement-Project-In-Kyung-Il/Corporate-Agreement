using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpBarManager : MonoBehaviour
{
    public GameObject hpBarPrefab;
    public Canvas uiCanvas;

    private Camera mainCamera;
    private Dictionary<Transform, GameObject> hpBars = new Dictionary<Transform, GameObject>();
    private Plane[] cameraPlanes;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        cameraPlanes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); //이거 Monster리스트에 담기게 Instantiate 되면, 바꾸기

        foreach (GameObject enemy in enemies)
        {
            Renderer rend = enemy.GetComponentInChildren<Renderer>();
            if (rend == null) continue;

            bool isVisible = GeometryUtility.TestPlanesAABB(cameraPlanes, rend.bounds);

            if (isVisible)
            {
                // 없으면 생성
                if (!hpBars.ContainsKey(enemy.transform))
                {
                    GameObject bar = Instantiate(hpBarPrefab, uiCanvas.transform);
                    bar.GetComponent<EnemyHpBar>().target = enemy.transform;
                    hpBars[enemy.transform] = bar;
                }
            }
            else
            {
                // 있으면 삭제
                if (hpBars.ContainsKey(enemy.transform))
                {
                    Destroy(hpBars[enemy.transform]);
                    hpBars.Remove(enemy.transform);
                }
            }
        }
    }
}
