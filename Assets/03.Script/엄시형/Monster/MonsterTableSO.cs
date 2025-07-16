using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using _03.Script.엄시형.Data;
using _03.Script.엄시형.Monster;
using _03.Script.엄시형.Stage;
using UnityEngine;
using UnityEngine.Assertions;
using Debug = UnityEngine.Debug;

namespace _00.Resources.엄시형.PrefabTable
{
    [CreateAssetMenu(fileName = "MonsterTable", menuName = "SO/Table/MonsterTable", order = 1)]
    public class MonsterTableSO : ScriptableObject
    {
        [SerializeField] private BaseMonster[] mMonsters;
        private Dictionary<MonsterType, BaseMonster> mMonsterDic;
        
        [Conditional("UNITY_EDITOR")]
        private void OnValidate()
        {
            Debug.Assert(mMonsters != null, "몬스터 프리팹을 넣어주세요");
            mMonsterDic = mMonsters.ToDictionary((monster) => monster.Type);
            
            Debug.Assert(mMonsterDic != null, "mMonsters => mMonsterDic 변환 실패");
        }
        
        public BaseMonster GetMonster(MonsterType type)
        {
            return mMonsterDic[type];
        }
    }
}