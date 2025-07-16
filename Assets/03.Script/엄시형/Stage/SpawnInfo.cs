using System;
using UnityEngine;

namespace _03.Script.엄시형.Stage
{
    [Serializable]
    public class SpawnInfo
    {
        [field: SerializeField]
        public Vector2 Point { get; set; }
        
        [field: SerializeField]
        public float Radius { get; set; }
        
        public SpawnInfo(int x, int y, float radius)
        {
            Point = new Vector2(x, y);
            Radius = radius;
        }
        
        public SpawnInfo(Vector2 point, float radius)
        {
            Point = point;
            Radius = radius;
        }
    }
}