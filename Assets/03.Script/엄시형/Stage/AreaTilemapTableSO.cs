using System;
using System.Collections.Generic;
using System.Linq;
using _03.Script.엄시형.Stage.DTO;
using _03.Script.엄시형.Stage.V2;
using _03.Script.엄시형.Util;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace _03.Script.엄시형.Stage
{
    [CreateAssetMenu(fileName = "AreaTilemapTableSO", menuName = "SO/Stage/AreaTilemapTable", order = 0)]
    public sealed class AreaTilemapTableSO : ScriptableObject
    {
        [SerializeField] private List<AreaTilemap> m_TilemapList = new List<AreaTilemap>();
        private Dictionary<StageTheme, List<AreaTilemap>> m_TilemapDic;
        
        public Tilemap GetTilemap(StageTheme theme, int id)
        {
            if (m_TilemapDic.TryGetValue(theme, out var themeAreaList))
            {
                Tilemap tilemap = themeAreaList
                    .Find(map => map.Id == id)
                    .Tilemap;
                
                return tilemap;
            }

            Debug.LogError($"널나옴");
            return null;
        }
        
        public List<AreaTilemap> GetThemeAreaList(StageTheme theme)
        {
            return m_TilemapDic[theme];
        }

        public Tilemap GetRandomTilemapOrNull(StageTheme theme, int count)
        {
            if (m_TilemapDic.TryGetValue(theme, out var themeAreaList))
            {
                return themeAreaList[Random.Range(0, count)].Tilemap;
            }
            
            Debug.LogError($"Theme {theme}에 해당하는 Tilemap이 없습니다.");
            return null;
        }
        
        public void Init()
        {
            m_TilemapDic = m_TilemapList
                .GroupBy(map => map.Theme)
                .ToDictionary(group => group.Key
                    , group => group.ToList());
        }
    }
    
    [Serializable]
    public class AreaTilemap : ICloneable<AreaTilemap>
    {
        public StageTheme Theme => m_Theme;
        public int Id => m_Id;
        public Tilemap Tilemap => m_Tilemap;

        [SerializeField] private StageTheme m_Theme;
        [SerializeField] private int m_Id;
        [SerializeField] private Tilemap m_Tilemap;

        public AreaTilemap(StageTheme theme, int id, Tilemap tilemap)
        {
            m_Theme = theme; 
            m_Id = id;
            m_Tilemap = tilemap;
        }

        public AreaTilemap Clone()
        {
            return new AreaTilemap(m_Theme, m_Id, m_Tilemap);
        }
    }
}