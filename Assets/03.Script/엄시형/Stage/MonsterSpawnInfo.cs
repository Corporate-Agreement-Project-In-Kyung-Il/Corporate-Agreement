using System.Numerics;
using _03.Script.엄시형.Monster;
using UnityEngine;

namespace _03.Script.엄시형.Data
{
    
    [System.Serializable]
    public sealed class MonsterSpawnInfo
    {
        public BaseMonster Monster => mMonster;
        public Vector2Int Index => mIndex;
        
        [SerializeField] private BaseMonster mMonster;
        [SerializeField] private Vector2Int mIndex;
    }
}