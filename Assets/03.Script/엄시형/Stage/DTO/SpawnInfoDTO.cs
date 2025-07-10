using System;
using UnityEngine;

namespace _03.Script.엄시형.Stage.DTO
{
    /// <summary>
    /// 데이터 교환용 클래스
    /// 다른용도 사용금지
    /// </summary>
    [Serializable]
    public class SpawnInfoDTO
    {
        public float X;
        public float Y;
        public float Radius;
        
        public SpawnInfoDTO() {}
        public SpawnInfoDTO(Vector2 pos, float radius)
        {
            X = pos.x;
            Y = pos.y;
            Radius = radius;
        }
    }
}