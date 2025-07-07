using System;
using System.Collections;
using System.Collections.Generic;
using _03.Script.엄시형.Data;
using _03.Script.엄시형.Monster;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [Header("레벨 정보")]
    [SerializeField] private LevelInfoSO mLevelInfo;
    [SerializeField] private List<Tilemap> mStageTilemapList;
    
    [Header("몬스터 프리팹")]
    [SerializeField] private List<BaseMonster> mMonsterPrefabList;
    
    private void Awake()
    {
        Debug.Assert(mStageTilemapList != null, "mLevelTilemap 인스펙터에서 빠짐");
        Debug.Assert(mLevelInfo != null, "mLevelInfo 인스펙터에서 빠짐");
        Debug.Assert(mMonsterPrefabList != null, "mMonsterPrefabList 인스펙터에서 빠짐");
        
        foreach (var stage in mLevelInfo.StageInfoList)
        {
            for (int i = 0; i < stage.MonsterCount; i++)
            {
                Vector2Int spawnPoint = stage.MonsterSpawnPointList[i];
                int rndIndex = Random.Range(1, Enum.GetValues(typeof(MonsterType)).Length);
                
                SpawnMonster(spawnPoint, (MonsterType) rndIndex);
            }
        }
    }

    private void SpawnMonster(Vector2 position, MonsterType type)
    {
        Instantiate(mMonsterPrefabList.Find(
            monster => monster.Type == type)
            , position, Quaternion.identity);
    }
}
