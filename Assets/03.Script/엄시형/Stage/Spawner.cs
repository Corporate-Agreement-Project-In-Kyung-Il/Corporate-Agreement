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
    [SerializeField] private List<Tilemap> mStageTilemapList;

    private void OnValidate()
    {
        Debug.Assert(mStageTilemapList != null, "mStageTilemapList 인스펙터에서 빠짐");
        Debug.Assert(mStageInfo != null, "mStageInfo 인스펙터에서 빠짐");
        Debug.Assert(mMonsterTable != null, "MonsterTableSO 인스펙터에서 빠짐");
    }

    void Awake()
    {
        SpawnMonstersInArea(mStageInfo.AreaInfoList[0]);
    }
    
    // private void SpawnMonster(Vector2 position, MonsterType type)
    // {
    //     Instantiate(mMonsterPrefabList.Find(
    //         monster => monster.Type == type)
    //         , position, Quaternion.identity);
    // }
    
    private void SpawnMonstersInArea(AreaInfoSO areaInfoSo)
    {
        Debug.Assert(areaInfoSo != null, "널 들어옴");
        
        int monsterTypeLength = mStageInfo.SpawnMonsterTypeList.Count;
        
        for (int i = 0; i < areaInfoSo.MonsterSpawnPointList.Count; i++)
        {
            MonsterType type = mStageInfo.SpawnMonsterTypeList[Random.Range(0, monsterTypeLength)];
            BaseMonster monsterPrefab = mMonsterTable.GetMonster(type);
            Vector2Int spawnPointVec2 = areaInfoSo.MonsterSpawnPointList[i];
            Vector3 spawnPoint = new Vector3(spawnPointVec2.x, spawnPointVec2.y, 0);
            
            Instantiate(monsterPrefab, spawnPoint, Quaternion.identity);
        }
    }
}
