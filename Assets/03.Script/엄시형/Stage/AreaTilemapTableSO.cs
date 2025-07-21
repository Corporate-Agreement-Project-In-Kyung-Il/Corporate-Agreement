using System;
using System.Collections.Generic;
using System.Linq;
using _03.Script.엄시형.Stage.DTO;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

namespace _03.Script.엄시형.Stage
{
    [CreateAssetMenu(fileName = "AreaTilemapTableSO", menuName = "SO/Stage/AreaTilemapTable", order = 0)]
    public sealed class AreaTilemapTableSO : ScriptableObject
    {
        [SerializeField] private List<AreaTilemap> m_TilemapList = new List<AreaTilemap>();

        public Tilemap GetTilemapOrNull(int id)
        {
            var tile = m_TilemapList.FirstOrDefault(tile => id == tile.Id)?.Tilemap;
            
            Debug.Assert(tile != null, $"AreaTilemapTableSO에서 {id}에 해당하는 Tilemap을 찾을 수 없습니다. ");
            
            return tile;
        }
    }

    [Serializable]
    public class AreaTilemap
    {
        public int Id => m_Id;
        public Tilemap Tilemap => m_Tilemap;

        [SerializeField] private int m_Id;
        [SerializeField] private Tilemap m_Tilemap;

        public AreaTilemap(int id, Tilemap tilemap)
        {
            m_Id = id;
            m_Tilemap = tilemap;
        }
    }
}