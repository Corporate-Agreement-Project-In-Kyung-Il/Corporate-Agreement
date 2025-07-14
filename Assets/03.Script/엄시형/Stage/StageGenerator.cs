using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using _03.Script.엄시형.Stage.DTO;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Debug = UnityEngine.Debug;

public class AutoMapGenerator : MonoBehaviour
{
    // [SerializeField] private Tilemap m_CurTilemap;
    [SerializeField] private List<Tilemap> m_TilemapList;
    
    
    /// <summary>
    /// int: Spawn
    /// </summary>
    private Dictionary<int, List<AreaPattern>> m_AreaPatterns;
    [SerializeField] private GameObject m_Grid;  
    // [SerializeField] private TilemapRenderer m_CurTilemapRenderer;
    // [SerializeField] private int mTileCount = 10;
    // [SerializeField] private float mTileHeight = 1f;

    [Conditional("UNITY_EDITOR")]
    private void OnValidate()
    {
        Debug.Assert(m_Grid != null, "m_Grid 컴포넌트를 넣어주세요");
        // Debug.Assert(m_TilemapList.Count <= 0, "m_TilemapList가 없음 컴포넌트를 넣어주세요");
    }

    private void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        // Bounds bounds = m_CurTilemap.localBounds;
        // Vector3 cellSize = m_CurTilemap.cellSize;
        
        // Vector2 topCenter = new Vector2(0, bounds.extents.y + (cellSize.y * 0.5f));
        
        // Debug.Log($"타일맵 중앙 상단 위치: {topCenter}");

        // var curTilemap = Instantiate(m_TilemapList[0], parent: m_Grid.transform);
        // var cellSize = m_TilemapList[0].cellSize;
        // var topY =  curTilemap.localBounds.extents.y + cellSize.y * 0.5f;
        
        // Debug.Log(topY);
        float topY = 0f;
        
        for (int i = 0; i < m_TilemapList.Count; i++)
        {
            var curTileMap = Instantiate(m_TilemapList[i]
                , new Vector2(0, topY)
                , Quaternion.identity
                , parent: m_Grid.transform);

            // Debug.Log(curTileMap.transform.position);
            topY += curTileMap.cellBounds.yMax;
            // topY = curTilemap.transform.position.y + curTileMap.localBounds.extents.y + cellSize.y * 0.5f;
        }
    }
}
