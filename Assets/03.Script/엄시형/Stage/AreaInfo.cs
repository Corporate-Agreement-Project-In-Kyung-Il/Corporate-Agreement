using System;
using System.Collections.Generic;
using _03.Script.엄시형.Stage.DTO;
using UnityEngine;

namespace _03.Script.엄시형.Data
{
    [Serializable]
    public class AreaInfo
    {
        public bool bIsBossStage =>  mbIsBossStage;
        
        /// <summary>
        /// 지금은 Point하나당 한 몬스터라 상관없지만 한Point에 여러 마리 나온다면 변경 필요
        /// </summary>
        public int MonsterCount => mAreaPattern.MonsterSpawnInfoList.Count;
        public AreaPattern AreaPattern => mAreaPattern;
    
        [Header("몬스터 스폰지점")]
        [SerializeField] private AreaPattern mAreaPattern;
        
        private bool mbIsBossStage;
    }
}