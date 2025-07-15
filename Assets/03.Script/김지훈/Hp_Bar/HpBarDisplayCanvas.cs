using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBarManager : MonoBehaviour
{
    [Header("PlayerHpBar 표시")]
    public PlayerHpBar playerHpBar;
    [Header("EnemyHpBar 표시")]
    public EnemyHpBar enemyBar;
    public Canvas uiCanvas; //World Space로 설정된 Canvas
    [Header("기본적인 EnemyGameObject 기준 : HpBar 표시위치")]
    public float barDisplayPosition;
    
    private Camera mainCamera;
    private Dictionary<Transform, GameObject> enemyHpBars = new Dictionary<Transform, GameObject>();
    private Dictionary<Transform, GameObject> playerHpBars = new Dictionary<Transform, GameObject>();
    private Plane[] cameraPlanes;
    private List<Collider2D> monsterList = new List<Collider2D>();
    private List<Collider2D> playerList = new List<Collider2D>();
    
    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        monsterList = AliveExistSystem.Instance.monsterList;
        playerList = AliveExistSystem.Instance.playerList;
    }

    void Update()
    {
        cameraPlanes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
        EnemyHpBarSetting();
        PlayerHpBarSetting();

    }

    private void EnemyHpBarSetting()
    {
        for (int i = 0; i < monsterList.Count; i++)
        {
            if(monsterList.Count <= 0) continue;
            
            Transform enemyTransform = monsterList[i].transform;

            //if (rend == null) continue;
            bool isVisible = GeometryUtility.TestPlanesAABB(cameraPlanes, monsterList[i].bounds);

            if (isVisible)
            {
                // 없으면 생성
                if (enemyHpBars.ContainsKey(enemyTransform).Equals(false))
                {
                    EnemyHpBar EnemyHpDisplay = ObjectPoolSystem.Instance.GetObjectOrNull("HpBarDisplay") as EnemyHpBar;
     
                    EnemyHpDisplay.gameObject.SetActive(true);
                    if (enemyTransform.gameObject.TryGetComponent(out MonsterController monster))
                    {
                        EnemyHpDisplay.target = monster;
                        EnemyHpDisplay.SliderDown(monster);
                    }

                    enemyHpBars[enemyTransform] = EnemyHpDisplay.gameObject;

                }
            }
            else
            {
                // 있으면 삭제
                if (enemyHpBars.ContainsKey(enemyTransform))
                {
                    Destroy(enemyHpBars[enemyTransform]);
                    enemyHpBars.Remove(enemyTransform);
                }
            }
        }
    }
    
    private void PlayerHpBarSetting()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList.Count <= 0) continue;

            Transform playerTransform = playerList[i].transform;

            bool isVisible = GeometryUtility.TestPlanesAABB(cameraPlanes, playerList[i].bounds);

            if (isVisible)
            {
                if (playerHpBars.ContainsKey(playerTransform).Equals(false))
                {
                    PlayerHpBar playerHpDisplay = Instantiate(playerHpBar, playerTransform.position + Vector3.up * -0.4f, Quaternion.identity, this.transform) as PlayerHpBar;
                        //ObjectPoolSystem.Instance.GetObjectOrNull("PlayerHpBar") as PlayerHpBar;

//                    if (playerHpDisplay == null) continue;
//
                    playerHpDisplay.gameObject.SetActive(true);
//                    if (playerTransform.TryGetComponent(out Player player))
                    if(playerTransform.gameObject.TryGetComponent(out Player player)) 
                        playerHpDisplay.target = player;
                    
                    playerHpBars[playerTransform] = playerHpDisplay.gameObject;
                }
            }
            else
            {
                if (playerHpBars.ContainsKey(playerTransform))
                {
                    Destroy(playerHpBars[playerTransform]);
                    playerHpBars.Remove(playerTransform);
                }
            }
        }
    }
}
