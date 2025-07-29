using System.Collections.Generic;
using UnityEngine;

namespace _03.Script.엄시형.Stage
{
    [CreateAssetMenu(fileName = "StageInfoSO", menuName = "SO/Stage/StageInfoSO", order = 0)]
    public class StageInfoTableSO : ScriptableObject
    {
        [SerializeField] private List<V2.StageInfo> m_stageInfoList;

        public V2.StageInfo GetStageInfoByIndex(int index)
        {
            return m_stageInfoList[index];
        }
    }
}