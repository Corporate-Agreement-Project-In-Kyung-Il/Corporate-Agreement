using System.Numerics;
using _03.Script.엄시형.Monster;
using UnityEngine;

namespace _03.Script.엄시형.Data
{
    public enum MonsterType
    {
        Unknown = 0,
        Bear0
    }
    
    [System.Serializable]
    public class MonsterSpawnInfo
    {
        public BaseMonster Monster => mMonster;
        public Vector2Int Index => mIndex;
        
        [SerializeField] private BaseMonster mMonster;
        [SerializeField] private Vector2Int mIndex;
    }
}