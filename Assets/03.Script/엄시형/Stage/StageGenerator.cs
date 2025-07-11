using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Tilemaps;
using Debug = UnityEngine.Debug;

public class AutoMapGenerator : MonoBehaviour
{
    [SerializeField] private Tilemap mTilemap;
    [SerializeField] private int mTileCount = 10;
    [SerializeField] private float mTileHeight = 1f;

    [Conditional("UNITY_EDITOR")]
    private void OnValidate()
    {
        Debug.Assert(mTilemap != null, "Tilemap 컴포넌트를 넣어주세요");
    }

    private void Awake()
    {
        BoundsInt areaBounds = mTilemap.cellBounds;
        int centerX = (areaBounds.xMin + areaBounds.xMax - 1) / 2;
        int topY = areaBounds.yMax - 1;
        
        Vector3Int cellPosition = new Vector3Int(centerX, topY, 0);
        Vector3 worldPosition = mTilemap.GetCellCenterWorld(cellPosition);
        
        Debug.Log($"중앙 상단 셀: {cellPosition}, 월드 위치: {worldPosition}");
    }
}
