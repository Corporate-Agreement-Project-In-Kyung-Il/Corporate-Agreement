using System;
using System.Collections;
using System.Collections.Generic;
using _00.Resources.엄시형.PrefabTable;
using _03.Script.엄시형.Data.V2;
using _03.Script.엄시형.Monster;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private MonsterTableSO mMonsterTable;
    
    [Header("스테이지 정보")]
    [SerializeField] private StageInfoSO 
        mStageInfo;
    [SerializeField] private List<Tilemap> mStageTilemapList;

    private void OnValidate()
    {
        Debug.Assert(mStageTilemapList != null, "mStageTilemapList 인스펙터에서 빠짐");
        Debug.Assert(mStageInfo != null, "mStageInfo 인스펙터에서 빠짐");
        Debug.Assert(mMonsterTable != null, "MonsterTableSO 인스펙터에서 빠짐");
        // var tilemap = mStageTilemapList[0];
        // var tileAnchor = tilemap.tileAnchor;
        // var bounds = tilemap.cellBounds;
        // var tiles = tilemap.GetTilesBlock(bounds);
        //
        // int width = bounds.size.x;
        // int height = bounds.size.y;
        //
        // for (int y = height - 1; y >= 0; y--)
        // {
        //     for (int x = 0; x < width; x++)
        //     {
        //         int index = x + y * width;
        //         var tile = tiles[index];
        //         
        //         if (tile != null)
        //         {
        //             float tileX = bounds.xMin + x + tileAnchor.x;
        //             float tileY = bounds.yMin + y + tileAnchor.y;
        //             
        //             Debug.Log($"타일 위치 {tileX}, {tileY})");
        //         }
        //     }
        // }
        //
        // Debug.Log(tiles.Length);
    }

    void Awake()
     {
         SpawnMonstersInArea(mStageInfo.AreaInfoList[0]);

         // foreach (var pos in mStageTilemapList[0].cellBounds.allPositionsWithin)
         // {
         //     TileBase tile = mStageTilemapList[0].GetTile(pos);
         //     
         //     if (tile != null)
         //     {
         //         Debug.Log($"✔️ 실제 타일 위치: {pos.x + 0.5f}, {pos.y + 0.5f}");
         //     }
         // }
     }

    // private void SpawnMonster(Vector2 position, MonsterType type)
    // {
    //     Instantiate(mMonsterPrefabList.Find(
    //         monster => monster.Type == type)
    //         , position, Quaternion.identity);
    // }
    
    private void SpawnMonstersInArea(AreaInfo areaInfo)
    {
        Debug.Assert(areaInfo != null, "널 들어옴");
        
        int monsterTypeLength = areaInfo.SpawnMonsterTypeList.Count;
        
        for (int i = 0; i < areaInfo.MonsterSpawnPointList.Count; i++)
        {
            MonsterType type = areaInfo.SpawnMonsterTypeList[Random.Range(0, monsterTypeLength)];
            BaseMonster monsterPrefab = mMonsterTable.GetMonster(type);
            Vector2Int spawnPointVec2 = areaInfo.MonsterSpawnPointList[i];
            Vector3 spawnPoint = new Vector3(spawnPointVec2.x, spawnPointVec2.y, 0);
            
            Instantiate(monsterPrefab, spawnPoint, Quaternion.identity);
        }
    }
}
