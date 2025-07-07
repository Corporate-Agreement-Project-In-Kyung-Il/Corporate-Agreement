using System.Collections.Generic;
using UnityEngine;

namespace _03.Script.엄시형.Data
{
    public class StageInfo
    {
        public bool bIsBossStage =>  mbIsBossStage;
        public int MonsterCount => mMonsterSpawnPointList.Count;
        public List<Vector2Int> MonsterSpawnPointList => mMonsterSpawnPointList;
    
        [Header("Reference")]
        [SerializeField] private List<Vector2Int> mMonsterSpawnPointList;
        
        private bool mbIsBossStage;
    }
}