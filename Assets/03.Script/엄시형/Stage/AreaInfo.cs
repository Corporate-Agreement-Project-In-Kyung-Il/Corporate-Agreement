using System;
using System.Collections.Generic;
using UnityEngine;

namespace _03.Script.엄시형.Data
{
    [Serializable]
    public class AreaInfo
    {
        public bool bIsBossStage =>  mbIsBossStage;
        public int MonsterCount => mMonsterSpawnPointList.Count;
        public List<Vector2Int> MonsterSpawnPointList => mMonsterSpawnPointList;
    
        [Header("몬스터 스폰지점")]
        [SerializeField] private List<Vector2Int> mMonsterSpawnPointList;
        
        private bool mbIsBossStage;
    }
}