using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class HpBarDisplayCanvas : MonoBehaviour
{
    [Header("PlayerHpBar 표시")]
    public PlayerHpBar playerHpBar;
    
    [Header("SkillBar 표시")]
    public SkillParentBar skillBar;

    //public Canvas uiCanvas; //World Space로 설정된 Canvas
    //[Header("기본적인 EnemyGameObject 기준 : HpBar 표시위치")]
    //public float barDisplayPosition;
    
    private Camera mainCamera;
    private Dictionary<Transform, GameObject> enemyHpBars = new Dictionary<Transform, GameObject>();
    private Dictionary<Transform, GameObject> playerHpBars = new Dictionary<Transform, GameObject>();
    private Plane[] cameraPlanes;
    private List<Collider2D> monsterList = new List<Collider2D>();
    private List<Collider2D> playerList = new List<Collider2D>();
    private Collider2D[] colliderEnemy = new Collider2D[30];
    
    public Vector2 enemyHpDetectionSize = new Vector2(5.5f, 25f);
    private void Start()
    {
        mainCamera = Camera.main;
    }



    void Update()
    {
        EnemyHpBarSetting();
        PlayerHpBarSetting();
    }

    private void EnemyHpBarSetting()
    {
         int count =Physics2D.OverlapBoxNonAlloc(new Vector2(mainCamera.transform.position.x,
            mainCamera.transform.position.y), enemyHpDetectionSize, 0f, colliderEnemy, LayerMask.GetMask("Enemy"));
        
        //colliderEnemy = Physics2D.OverlapBoxAll
        //        (Vector2.right * mainCamera.transform.position.x + 
        //         Vector2.up * mainCamera.transform.position.y, enemyHpDetectionSize, 0f,
        //            LayerMask.GetMask("Enemy"));
            
        //bool isVisible = GeometryUtility.TestPlanesAABB(cameraPlanes, monsterList[i].bounds);
        
        // 현재 존재하는 모든 몬스터들에 대해 검사
        
        for (int i = 0; i < count; i++)//colliderEnemy.Length; i++)
        {
            Transform enemyTransform = colliderEnemy[i].transform;
        
            // 이미 생성된 HpBar가 없으면 생성
            if (enemyHpBars.ContainsKey(enemyTransform) == false)
            {
                EnemyHpBar EnemyHpDisplay = ObjectPoolSystem.Instance.GetObjectOrNull("HpBarDisplay") as EnemyHpBar;
                if (EnemyHpDisplay == null) continue;
        
                EnemyHpDisplay.gameObject.SetActive(true);
        
                if (enemyTransform.TryGetComponent(out MonsterController monster))
                {
                    EnemyHpDisplay.target = monster;
                    EnemyHpDisplay.SliderDown(monster);
                }
        
                enemyHpBars[enemyTransform] = EnemyHpDisplay.gameObject;
            }
        }
    }
    
    
    private void PlayerHpBarSetting()
    {
        playerList = AliveExistSystem.Instance.playerList;
        
        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList.Count <= 0) continue;

            Transform playerTransform = playerList[i].transform;
            
            if (playerHpBars.ContainsKey(playerTransform).Equals(false))
            {
                PlayerHpBar playerHpDisplay = Instantiate(playerHpBar, playerTransform.position + Vector3.up * -0.4f, Quaternion.identity, this.transform) 
                    as PlayerHpBar;
                SkillParentBar skillTimeDisplay = Instantiate(skillBar, playerTransform.position + Vector3.up * -0.4f, Quaternion.identity, this.transform) 
                    as SkillParentBar;
                
                playerHpDisplay.gameObject.SetActive(true);
                skillTimeDisplay.gameObject.SetActive(true);

                if (playerTransform.gameObject.TryGetComponent(out Player player))
                {
                    playerHpDisplay.target = player;
                    skillTimeDisplay.target = player;
                }

                playerHpBars[playerTransform] = playerHpDisplay.gameObject;
                playerHpBars[playerTransform] = skillTimeDisplay.gameObject;
            }
        }
    }
}
