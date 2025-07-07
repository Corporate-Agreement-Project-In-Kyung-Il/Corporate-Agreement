using System;
using System.Collections;
using System.Collections.Generic;
using _03.Script.엄시형.Data;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "SO/LevelInfo", order = 1)]
public class LevelInfoSO : ScriptableObject
{
    // TODO : 엑셀이나 CSV부터 읽어오기 + 에디터에 올리기
    public List<StageInfo> StageInfoList => mStageInfoList;
    
    [Header("Reference")]
    [SerializeField] private List<StageInfo> mStageInfoList;
    [SerializeField] private int mLevelIndex;

    private void Reset()
    {
        Debug.Assert(mStageInfoList != null
            , "mStageInfo 인스펙터에서 빠짐");
    }
}
