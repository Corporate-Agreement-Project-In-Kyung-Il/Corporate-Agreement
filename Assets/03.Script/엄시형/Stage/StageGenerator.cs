using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using _03.Script.엄시형.Stage;
using _03.Script.엄시형.Stage.DTO;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Debug = UnityEngine.Debug;

[Obsolete("Spawner로 병합했습니다", true)]
public class AutoMapGenerator : MonoBehaviour
{
    // [SerializeField] private Tilemap m_CurTilemap;
    // TODO : Tilemap을 여러개 넣어서 순차적으로 생성하는 기능 추가
    
    [SerializeField] private StageInfo m_StageInfo;
    [SerializeField] private GameObject m_Grid;  
    // [SerializeField] private TilemapRenderer m_CurTilemapRenderer;
    // [SerializeField] private int mTileCount = 10;
    // [SerializeField] private float mTileHeight = 1f;
    
    [Conditional("UNITY_EDITOR")]
    private void OnValidate()
    {
        Debug.Assert(m_Grid != null, "m_Grid 컴포넌트를 넣어주세요");
        // Debug.Log(m_AreaInfoTable.GetInfoOrNull(10001));
        // Debug.Assert(m_TilemapList.Count <= 0, "m_TilemapList가 없음 컴포넌트를 넣어주세요");
    }
}
