using System;
using System.Collections.Generic;
using _03.Script.엄시형.Monster;
using UnityEngine;
using UnityEngine.Serialization;

namespace _03.Script.엄시형.Data.V2
{
    [Serializable]
    public class AreaInfo
    {
        public bool bIsBossStage =>  mbIsBossStage;
        public int MonsterCount => mMonsterSpawnPointList.Count;
        public List<MonsterType> SpawnMonsterTypeList => mSpawnMonsterTypeList;
        public List<Vector2Int> MonsterSpawnPointList => mMonsterSpawnPointList;
        
        [Header("등장 몬스터!")]
        [SerializeField] private List<MonsterType> mSpawnMonsterTypeList;
        
        [Header("몬스터 스폰지점")]
        [SerializeField] private List<Vector2Int> mMonsterSpawnPointList;
        
        private bool mbIsBossStage;
    }
}