using System;
using System.Collections.Generic;
using _03.Script.엄시형.Monster;
using _03.Script.엄시형.Util;
using UnityEngine;
using UnityEngine.Serialization;

namespace _03.Script.엄시형.Data.V2
{
    [CreateAssetMenu(fileName = "AreaInfoSO", menuName = "SO/Stage/AreaInfoSO", order = 1)]
    public class AreaInfoSO : ScriptableObject
    {
        // public bool bIsBossStage =>  mbIsBossStage;
        public int MonsterCount => MonsterSpawnPointList.Count;
        // public List<MonsterType> SpawnMonsterTypeList => mSpawnMonsterTypeList;
        public List<Vector2> MonsterSpawnPointList;
        
        // [Header("등장 몬스터!")]
        // [SerializeField] private List<MonsterType> mSpawnMonsterTypeList;
        
        // [Header("몬스터 스폰지점")]
        // [SerializeField] private List<Vector2> mMonsterSpawnPointList;
        
        // private bool mbIsBossStage;
    }
}