using System;
using System.Collections.Generic;
using _03.Script.엄시형.Stage;
using _03.Script.엄시형.Stage.DTO;
using UnityEngine;

namespace _03.Script.엄시형.Data.V2
{
    /// <summary>
    /// 기획자
    /// </summary>
    [CreateAssetMenu(fileName = "AreaPatternTempSO", menuName = "SO/Stage/AreaPatternTempSO", order = 1)]
    public class AreaPatternTempSO : ScriptableObject
    {
        public int MonsterCount => SpawnInfoList.Count;
        public List<SpawnInfoDTO> SpawnInfoList = new List<SpawnInfoDTO>();
    }
}