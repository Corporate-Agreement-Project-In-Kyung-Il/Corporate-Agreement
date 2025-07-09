using System;
using UnityEngine;

namespace _03.Script.엄시형.Stage
{
    [Serializable]
    public struct SpawnInfo
    {
        public Vector2 SpawnPoint { get; private set; }
        public float SpawnRadius { get; private set; }
        
        public SpawnInfo(Vector2 spawnPoint, float spawnRadius)
        {
            SpawnPoint = spawnPoint;
            SpawnRadius = spawnRadius;
        }
    }
}