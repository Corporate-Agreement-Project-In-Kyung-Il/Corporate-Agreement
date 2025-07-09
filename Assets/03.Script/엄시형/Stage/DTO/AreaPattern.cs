using System;
using System.Collections.Generic;
using UnityEngine;

namespace _03.Script.엄시형.Stage.DTO
{
    [Serializable]
    public class AreaPattern
    {
        [field: SerializeField]
        public int PatternId { get; private set; }
        
        [field: SerializeField]
        public List<SpawnInfo> MonsterSpawnPoints { get; private set; }
    }
}