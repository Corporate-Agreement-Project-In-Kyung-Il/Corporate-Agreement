using System.Collections.Generic;
using _03.Script.엄시형.Data.V2;
using _03.Script.엄시형.Monster;
using UnityEngine;

namespace _03.Script.엄시형.Stage
{
    [CreateAssetMenu(fileName = "StageInfo", menuName = "SO/Stage/StageInfo", order = 1)]
    public class StageInfoSo : ScriptableObject
    {
        // TODO : 엑셀이나 CSV부터 읽어오기 + 에디터에 올리기
        public List<AreaInfoSO> AreaInfoList => mAreaInfoList;
        public List<MonsterType> SpawnMonsterTypeList => mSpawnMonsterTypeList;

        /// <summary>
        /// -1이면 데이터를 가져오지 못한경우
        /// </summary>
        [Header("현재 스테이지")]
        [SerializeField] private int mStageId = -1;

        [Header("몬스터 정보")]
        [SerializeField] private float mMonsterHp = 100f;
        [SerializeField] private float mMonsterAttack = 10f;
        [SerializeField] private List<MonsterType> mSpawnMonsterTypeList;
        
        [Header("구역 정보 1구역 2구역 3구역 4구역(보스스테이지)")]
        [SerializeField] private List<AreaInfoSO> mAreaInfoList;
        
        private void OnValidate()
        {
            Debug.Assert(mAreaInfoList != null
                , "mAreaInfoList 인스펙터에서 빠짐");
        }
    }
}
