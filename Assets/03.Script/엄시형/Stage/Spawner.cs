using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using _00.Resources.엄시형.PrefabTable;
using _03.Script.엄시형.Data.V2;
using _03.Script.엄시형.Monster;
using _03.Script.엄시형.Stage;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

// [CreateAssetMenu(fileName = "Spawner", menuName = "SO/Stage/Spawner", order = 1)]
public class Spawner : MonoBehaviour
{
    public static Spawner Instance { get; private set; }
    
    [SerializeField] private MonsterTableSO mMonsterTable;
    
    [Header("스테이지 정보")]
    [SerializeField] private StageInfoSo mStageInfo;
    [SerializeField] private List<GameObject> mAreaList = new List<GameObject>();
    // [SerializeField] private List<GameObject> mMonsterList = new List<GameObject>();
    
    [SerializeField] 
    private int m_CurStageId = 0;
    
    private Dictionary<character_class, Vector2> mPlayerSpawnPointDic = new Dictionary<character_class, Vector2>
    {
        { character_class.궁수, new Vector2(-0.5f, -4.5f) },
        { character_class.전사 , new Vector2(0.5f, -3.5f) },
        { character_class.마법사 , new Vector2(1.5f, -4.5f)}
    };
    
    // TODO : Area 동적생성
    
    [Conditional("UNITY_EDITOR")]
    private void OnValidate()
    {
        // Debug.Assert(mAreaList.Count != 0, "mAreaList 요소가 0 인스펙터 확인");
        Debug.Assert(mStageInfo != null, "mStageInfo 인스펙터에서 빠짐");
        Debug.Assert(mMonsterTable != null, "MonsterTableSO 인스펙터에서 빠짐");
    }

    void Awake()
    {
        Instance = this;
        
        SpawnAllMonstersInStage();
        GameManager.Instance.GameStart();
    }
    
    // private Player SpawnPlayer(Vector2 position, character_class characterClass)
    // {
    //     return Instantiate(mMonsterTable.GetMonster(type)
    //         , position
    //         , Quaternion.identity);
    // }
    
    private BaseMonster SpawnMonster(Vector2 position, MonsterType type)
    {
       return Instantiate(mMonsterTable.GetMonster(type), position, Quaternion.identity);
    }
    
    public void SpawnAllMonstersInStage()
    {
        if(m_CurStageId %3 == 0)
        {
            GameManager.Instance.CreateChoices(3);
        }
        m_CurStageId++;
        Debug.Assert(mStageInfo != null, "널 들어옴");
        
        int monsterTypeLength = mStageInfo.SpawnMonsterTypeList.Count;
        
        for (int i = 0; i < mStageInfo.AreaInfoList.Count; i++)
        {
            AreaInfoSO areaInfo = mStageInfo.AreaInfoList[i];
            
            for (int x = 0; x < areaInfo.MonsterCount; x++)
            {
                MonsterType type = mStageInfo.SpawnMonsterTypeList[Random.Range(0, monsterTypeLength)];
                
                SpawnMonsterInRange(
                    areaInfo.SpawnInfoList[x]
                    , type
                    , mAreaList[i]);
            }
        }
    }
    
    /// <summary>
    /// 플레이어 캐릭터의 시작 위치를 설정합니다.
    /// **Instantiate하는게 아님**.
    /// </summary>
    /// <param name="character"></param>
    public void SetPositionStartPoint(Player character)
    {
        if (mPlayerSpawnPointDic.TryGetValue(character.playerStat.characterClass
                , out var spawnPos))
        {
            character.transform.position = spawnPos;
        }
        else
        {
            Debug.Log("캐릭터 클래스에 맞는 스폰 위치가 없습니다.");
        }
    }

    /// <summary>
    /// 구역(Area)의 자식으로 몬스터를 스폰.
    /// </summary>
    /// <param name="type"> 몬스터 종류(MonsterType) </param>
    /// <param name="parent"> 구역(Area) </param>
    /// <returns> 스폰 몬스터 </returns>
    public BaseMonster SpawnMonsterInRange(SpawnInfo spawnInfo, MonsterType type, GameObject parent)
    {
        Vector2 randomOffset = Random.insideUnitCircle * spawnInfo.Radius;
        Vector2 spawnPos = spawnInfo.Point + randomOffset;
        BaseMonster monster = Instantiate(mMonsterTable.GetMonster(type), parent.transform);
        monster.transform.localPosition = spawnPos; // 부모의 로컬 좌표로 스폰
        
        return monster;
    }
}
