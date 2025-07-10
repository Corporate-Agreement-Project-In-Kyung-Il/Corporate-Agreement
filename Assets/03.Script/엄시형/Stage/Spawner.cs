using System;
using System.Collections;
using System.Collections.Generic;
using _00.Resources.엄시형.PrefabTable;
using _03.Script.엄시형.Data.V2;
using _03.Script.엄시형.Monster;
using _03.Script.엄시형.Stage;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

// [CreateAssetMenu(fileName = "Spawner", menuName = "SO/Stage/Spawner", order = 1)]
public class Spawner : MonoBehaviour
{
    [SerializeField] private MonsterTableSO mMonsterTable;
    
    [Header("스테이지 정보")]
    [SerializeField] private StageInfoSo mStageInfo;
    [SerializeField] private List<GameObject> mAreaList = new List<GameObject>();
    // [SerializeField] private List<GameObject> mMonsterList = new List<GameObject>();

    private int mCurStageId = 1;
    
    private Dictionary<character_class, Vector2> mPlayerSpawnPointDic = new Dictionary<character_class, Vector2>
    {
        { character_class.궁수, new Vector2(-0.5f, -4.5f) },
        { character_class.전사 , new Vector2(0.5f, -3.5f) },
        { character_class.마법사 , new Vector2(1.5f, -4.5f)}
    };
    
    // TODO : Area 동적생성
    
    private void OnValidate()
    {
        Debug.Assert(mAreaList.Count != 0, "mAreaList 요소가 0 인스펙터 확인");
        Debug.Assert(mStageInfo != null, "mStageInfo 인스펙터에서 빠짐");
        Debug.Assert(mMonsterTable != null, "MonsterTableSO 인스펙터에서 빠짐");
    }

    void Awake()
    {
        SpawnAllMonstersInStage(mStageInfo);
    }
    
    private BaseMonster SpawnMonster(Vector2 position, MonsterType type)
    {
       return Instantiate(mMonsterTable.GetMonster(type), position, Quaternion.identity);
    }
    
    public void SpawnAllMonstersInStage(StageInfoSo stageInfo)
    {
        Debug.Assert(stageInfo != null, "널 들어옴");
        
        int monsterTypeLength = stageInfo.SpawnMonsterTypeList.Count;
        
        for (int i = 0; i < stageInfo.AreaInfoList.Count; i++)
        {
            AreaInfoSO areaInfo = stageInfo.AreaInfoList[i];
            
            for (int x = 0; x < areaInfo.MonsterCount; x++)
            {
                MonsterType type = stageInfo.SpawnMonsterTypeList[Random.Range(0, monsterTypeLength)];
                
                SpawnMonsterInRange(
                    areaInfo.SpawnInfoList[x]
                    , type
                    , mAreaList[i]);
            }
        }
    }
    
    // TODO : 추후 수정해야함
    private void SetPositionStartPoint(Player character)
    {
        // switch (character.type)
        // {
        //     
        // }
    }

    /// <summary>
    /// 구역(Area)의 자식으로 몬스터를 스폰.
    /// </summary>
    /// <param name="type"> 몬스터 종류(MonsterType) </param>
    /// <param name="parent"> 구역(Area) </param>
    /// <returns> 스폰 몬스터 </returns>
    private BaseMonster SpawnMonsterInRange(SpawnInfo spawnInfo, MonsterType type, GameObject parent)
    {
        Vector2 randomOffset = Random.insideUnitCircle * spawnInfo.Radius;
        Vector2 spawnPos = spawnInfo.Point + randomOffset;
        BaseMonster monster = Instantiate(mMonsterTable.GetMonster(type), parent.transform);
        monster.transform.localPosition = spawnPos; // 부모의 로컬 좌표로 스폰
        
        return monster;
    }
}
