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

public class Spawner : MonoBehaviour
{
    [SerializeField] private MonsterTableSO mMonsterTable;
    
    [Header("스테이지 정보")]
    [SerializeField] private StageInfoSo mStageInfo;
    [SerializeField] private List<GameObject> mAreaList;

    // TODO : Area 동적생성
    
    private void OnValidate()
    {
        Debug.Assert(mAreaList != null, "mStageTilemapList 인스펙터에서 빠짐");
        Debug.Assert(mStageInfo != null, "mStageInfo 인스펙터에서 빠짐");
        Debug.Assert(mMonsterTable != null, "MonsterTableSO 인스펙터에서 빠짐");
    }

    void Awake()
    {
        SpawnAllMonstersInStage(mStageInfo);
    }
    
    // private BaseMonster SpawnMonster(Vector2 position, MonsterType type)
    // {
    //    return Instantiate(mMonsterTable.GetMonster(type), position, Quaternion.identity);
    // }
    
    /// <summary>
    /// 구역(Area) 자식으로 몬스터가 들어갑니다
    /// </summary>
    /// <param name="type"> 몬스터 종류(MonsterType) </param>
    /// <param name="parent"> 구역(Area) </param>
    /// <returns> 스폰 몬스터 </returns>
    private BaseMonster SpawnMonster(Vector2 position, MonsterType type, GameObject parent)
    {
        BaseMonster monster = Instantiate(mMonsterTable.GetMonster(type), parent.transform);
        monster.transform.localPosition = position; // 부모의 로컬 좌표로 스폰
        
        return monster;
    }
    
    private void SpawnAllMonstersInStage(StageInfoSo stageInfo)
    {
        Debug.Assert(stageInfo != null, "널 들어옴");
        
        int monsterTypeLength = stageInfo.SpawnMonsterTypeList.Count;
        
        for (int i = 0; i < stageInfo.AreaInfoList.Count; i++)
        {
            for (int x = 0; x < stageInfo.AreaInfoList[i].MonsterCount; x++)
            {
                MonsterType type = stageInfo.SpawnMonsterTypeList[Random.Range(0, monsterTypeLength)];
                
                SpawnMonster(stageInfo.AreaInfoList[i].MonsterSpawnPointList[x]
                    , type
                    , mAreaList[i]);
            }
        }
    }
}
