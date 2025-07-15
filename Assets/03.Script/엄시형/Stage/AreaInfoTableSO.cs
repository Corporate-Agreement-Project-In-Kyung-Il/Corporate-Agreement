using System.Linq;
using _03.Script.엄시형.Data.V2;
using UnityEngine;

namespace _03.Script.엄시형.Stage
{
    [CreateAssetMenu(fileName = "AreaInfoTableSO", menuName = "SO/Table/AreaInfoTableSO", order = 1)]
    public class AreaInfoTableSO : ScriptableObject
    {
        [SerializeField] private AreaInfoSO[] mAreaInfos;

        public AreaInfoSO GetInfoOrNull(int id)
        {
            return mAreaInfos.FirstOrDefault(info => info.AreaId == id);
        }
    }
}