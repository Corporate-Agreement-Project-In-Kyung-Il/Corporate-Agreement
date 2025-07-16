using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _03.Script.엄시형.Stage.DTO
{
    /// <summary>
    /// 데이터 교환용 클래스
    /// 다른용도 사용금지
    /// </summary>
    [Serializable]
    public sealed class SpawnInfoDTO
    {
        public float X;
        public float Y;
        public float Diameter;
        
        public Vector2 Pos => new Vector2(X, Y);
        
        public SpawnInfoDTO() {}
        public SpawnInfoDTO(Vector2 pos, float diameter)
        {
            X = pos.x;
            Y = pos.y;
            
            Diameter = diameter;
        }
        
        public SpawnInfo ToSpawnInfo()
        {
            return new SpawnInfo(Pos, Diameter);
        }
    }
}