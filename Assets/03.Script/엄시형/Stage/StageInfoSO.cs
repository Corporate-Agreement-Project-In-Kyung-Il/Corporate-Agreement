using System.Collections.Generic;
using System.Diagnostics;
using _03.Script.엄시형.Data.V2;
using _03.Script.엄시형.Monster;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace _03.Script.엄시형.Stage
{
    [CreateAssetMenu(fileName = "StageInfo", menuName = "SO/Stage/StageInfo", order = 1)]
    public class StageInfoSo : ScriptableObject
    {
        // TODO : 엑셀이나 CSV부터 읽어오기 + 에디터에 올리기
        public List<AreaInfoSO> AreaInfoList => mAreaInfoList;
        public List<MonsterType> SpawnMonsterTypeList => mSpawnMonsterTypeList;
        public int StageId => mStageId;
        
        [Header("현재 스테이지")]
        [SerializeField] private int mStageId;

        [Header("몬스터 정보")]
        [SerializeField] private float mMonsterHp = 100f;
        [SerializeField] private float mMonsterAttack = 10f;
        [SerializeField] private List<MonsterType> mSpawnMonsterTypeList = new List<MonsterType>();
        
        [Header("구역 정보 1구역 2구역 3구역 4구역(보스스테이지)")]
        [SerializeField] private List<AreaInfoSO> mAreaInfoList = new List<AreaInfoSO>();
        
        [Header("보스 구역 정보")]
        [SerializeField] public AreaInfoSO BossAreaInfo;
        
        [Conditional("UNITY_EDITOR")]
        private void OnValidate()
        {
            Debug.Assert(mAreaInfoList.Count != 0, "mAreaInfoList 요소가 0 인스펙터 확인");
        }
    }
}
