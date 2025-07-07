using System.Numerics;

namespace _03.Script.엄시형.Data
{
    public enum MonsterType
    {
        Unknown = 0,
        Bear0,
        Bear1,
        BugFly,
        Catumon,
        Goblin,
        Slime,
    }
    
    public class MonsterSpawnInfo
    {
        private Vector2 mPosition;
        private MonsterType mType;
    }
}