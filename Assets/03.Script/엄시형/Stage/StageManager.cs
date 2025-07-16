using System;
using UnityEngine;

namespace _03.Script.엄시형.Stage
{
    // [CreateAssetMenu(fileName = "FILENAME", menuName = "MENUNAME", order = 0)]
    [Obsolete("추후작업예정", true)]
    public class StageManager : MonoBehaviour
    {
        [SerializeField] private Spawner mSpawner;
        [SerializeField] private StageInfo mStageInfo;
    }
}