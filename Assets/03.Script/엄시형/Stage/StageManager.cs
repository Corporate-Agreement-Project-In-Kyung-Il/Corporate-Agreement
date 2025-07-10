using System;
using UnityEngine;

namespace _03.Script.엄시형.Stage
{
    // [CreateAssetMenu(fileName = "FILENAME", menuName = "MENUNAME", order = 0)]
    public class StageManager : MonoBehaviour
    {
        [SerializeField] private Spawner mSpawner;
        [SerializeField] private StageInfoSo mStageInfo;
    }
}