using System;
using System.Collections.Generic;
using _03.Script.엄시형.Stage;
using UnityEngine;

namespace _03.Script.엄시형.Data.V2
{
    [CreateAssetMenu(fileName = "AreaInfoSO", menuName = "SO/Stage/AreaInfoSO", order = 1)]
    public class AreaInfoSO : ScriptableObject
    {
        // public bool bIsBossStage =>  mbIsBossStage;
        public int MonsterCount => SpawnInfoList.Count;
        // public List<MonsterType> SpawnMonsterTypeList => mSpawnMonsterTypeList;
        public List<SpawnInfo> SpawnInfoList = new List<SpawnInfo>();
        
        // [Header("등장 몬스터!")]
        // [SerializeField] private List<MonsterType> mSpawnMonsterTypeList;
        
        // [Header("몬스터 스폰지점")]
        // [SerializeField] private List<Vector2> mMonsterSpawnPointList;
        
        // private bool mbIsBossStage;

        // private void OnValidate()
        // {
        //     Debug.Assert(SpawnInfoList.Count != 0, "SpawnInfoList 요소가 0 인스펙터 확인");
        // }
    }
}