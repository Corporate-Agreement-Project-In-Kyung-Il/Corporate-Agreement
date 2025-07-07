using System.Collections;
using System.Collections.Generic;
using _03.Script.엄시형.Data;
using UnityEngine;

public class LevelInfo
{
    public int MonsterCount => mMonsterSpawnPointList.Count;
    
    private List<MonsterSpawnInfo> mMonsterSpawnPointList;
    private int mLevelIndex;
    private bool mbBossStage;
}
