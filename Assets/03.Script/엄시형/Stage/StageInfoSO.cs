using System;
using System.Collections;
using System.Collections.Generic;
using _03.Script.엄시형.Data.V2;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "StageInfo", menuName = "SO/Stage/StageInfo", order = 1)]
public class StageInfoSO : ScriptableObject
{
    // TODO : 엑셀이나 CSV부터 읽어오기 + 에디터에 올리기
    public List<AreaInfo> AreaInfoList => mAreaInfoList;
    
    [Header("현재 스테이지")]
    [SerializeField] private int mStageIndex;

    [Header("몬스터 정보")]
    [SerializeField] private float mMonsterHp = 100f;
    [SerializeField] private float mMonsterAttack = 10f;
    
    [Header("구역 정보 1구역 2구역 3구역 4구역(보스스테이지)")]
    [SerializeField] private List<AreaInfo> mAreaInfoList;
    
    
    private void OnValidate()
    {
        Debug.Assert(mAreaInfoList != null
            , "mAreaInfoList 인스펙터에서 빠짐");
    }
}
