using _03.Script.엄시형.Data;
using UnityEngine;

namespace _03.Script.엄시형.Monster
{
    public enum MonsterType
    {
        Unknown = 0,
        Bear0,
        Catumon,
        Bugfly,
    }
    
    public abstract class BaseMonster : MonoBehaviour
    {
        public abstract MonsterType Type { get; }

        private MonsterType mType;
    }
}