using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Tilemap mLevelTilemap;
    private void Awake()
    {
        Debug.Assert(mLevelTilemap != null, "mLevelTilemap 인스펙터에서 빠짐");
    }
}
