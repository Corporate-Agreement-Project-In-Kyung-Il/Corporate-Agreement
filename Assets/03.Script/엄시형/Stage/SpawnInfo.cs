using System;
using UnityEngine;

namespace _03.Script.엄시형.Stage
{
    [Serializable]
    public struct SpawnInfo
    {
        public Vector2 Point { get; private set; }
        public float Radius { get; private set; }
        
        public SpawnInfo(Vector2 point, float radius)
        {
            Point = point;
            Radius = radius;
        }
    }
}