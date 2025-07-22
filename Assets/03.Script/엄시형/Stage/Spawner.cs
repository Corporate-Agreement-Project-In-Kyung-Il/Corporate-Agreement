using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using _00.Resources.엄시형.PrefabTable;
using _03.Script.엄시형.Monster;
using _03.Script.엄시형.Stage;
using _03.Script.엄시형.Stage.DTO;
using _03.Script.엄시형.Stage.V2;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;
using StageInfo = _03.Script.엄시형.Stage.V2.StageInfo;

// [CreateAssetMenu(fileName = "Spawner", menuName = "SO/Stage/Spawner", order = 1)]
public sealed class Spawner : MonoBehaviour
{
    public static Spawner Instance { get; private set; }
    public List<Tilemap> CurTilemapList => m_CurTilemapList;
    public int CurStageId = 1;
    
    [SerializeField] private StageEndDetector m_StageEndDetector;
    
    [SerializeField] private StagePatternTableSO m_StagePatternTable;
    [SerializeField] private AreaTilemapTableSO m_AreaTilemapTable;
    [SerializeField] private MonsterStatExel m_MonsterStatTable;
    [SerializeField] private MonsterData m_MonsterData;
    [SerializeField] private MonsterTableSO m_MonsterTable;
    
    [SerializeField] private List<Tilemap> m_TilemapList;
    [SerializeField] private GameObject m_Grid;  
    [SerializeField] private StageEndDetector m_StageEndPoint;
    [SerializeField] private Player[] m_PlayerList;
    
    private List<Tilemap> m_CurTilemapList = new List<Tilemap>();
    private List<AreaPattern> m_CurAreaList = new List<AreaPattern>();

    private StageInfo mStageInfo;
    // private List<AreaPattern> m_AreaPatternList;
    
    private Dictionary<character_class, Vector2> m_PlayerSpawnPointDic = new Dictionary<character_class, Vector2>
    {
        { character_class.궁수, new Vector2(-0.5f, -2f) },
        { character_class.전사 , new Vector2(0.5f, -1f) },
        { character_class.마법사 , new Vector2(1.5f, -2f) }
    };
    
    [Conditional("UNITY_EDITOR")]
    private void OnValidate()
    {
        // Debug.Assert(mAreaList.Count != 0, "mAreaList 요소가 0 인스펙터 확인");
        // Debug.Assert(mStageInfo != null, "mStageInfo 인스펙터에서 빠짐");
        // Debug.Assert(mMonsterTable != null, "MonsterTableSO 인스펙터에서 빠짐");
    }
    
    void Awake()
    {
        Instance = this;
        
        mStageInfo = new StageInfo(new int[] { 3, 4, 5 }
            , StageTheme.Grass
            , MonsterType.Slime
            , 15);
        
        // m_StagePatternTable.Init();
        // var areas = m_AreaTilemapTable.m_AreaTilemaps[10001];
        
        GameManager.Instance.GameStart();
    }

    private void OnEnable()
    {
        m_StageEndDetector.OnStageEnd += SetNextStage;
    }
    
    private void OnDisable()
    {
        m_StageEndDetector.OnStageEnd -= SetNextStage;
    }

    public void SetNextStage()
    {
        if (CurStageId % 3 == 0)
        {
            GameManager.Instance.CreateChoices(3);
        }
        
        CurStageId++;
        
        m_MonsterData.SetMonsterData(m_MonsterStatTable.GetValue(CurStageId));
        DestoryAllArea();
        GetRandomPattern();
        m_CurTilemapList = GenerateMap(mStageInfo.SpawnMonsterCounts);
        SpawnAllMonstersInStage();
    }
    
    private void Start()
    {
        // m_MonsterData.SetMonsterData(m_MonsterStatTable.GetValue(CurStageId));
        // mStageInfo = m_StagePatternTable.GetAreaList(1);
        m_MonsterData.SetMonsterData(m_MonsterStatTable.GetValue(CurStageId));
        GetRandomPattern();
        m_CurTilemapList = GenerateMap(mStageInfo.SpawnMonsterCounts);
        SpawnAllMonstersInStage();
    }

    // private Player SpawnPlayer(Vector2 position, character_class characterClass)
    // {
    //     return Instantiate(mMonsterTable.GetMonster(type)
    //         , position
    //         , Quaternion.identity);
    // }
    
    private BaseMonster SpawnMonster(Vector2 position, MonsterType type
        , GameObject parent)
    {
       var monster = Instantiate(
           m_MonsterTable.GetMonster(type)
           , position, Quaternion.identity
           , parent: parent.transform);
       monster.transform.localPosition = position;
       
       return monster;
    }
    
    public void DestoryAllArea()
    {
        for (int i = 0; i < m_CurTilemapList.Count; i++)
        {
            Destroy(m_CurTilemapList[i].gameObject);
        }
        
        m_CurTilemapList.Clear();
    }

    public void GetRandomPattern()
    {
        m_CurAreaList = new List<AreaPattern>(mStageInfo.AreaCount);
        
        for (int i = 0; i < mStageInfo.AreaCount; i++)
        {
            AreaPattern areaPattern = 
                m_StagePatternTable.GetRandomSpawnPattern(mStageInfo.SpawnMonsterCounts[i]);
            
            m_CurAreaList.Add(areaPattern);
        }
    }
    
    public void SpawnAllMonstersInStage()
    {
        for (var i = 0; i < m_PlayerList.Length; i++)
        {
            var player = m_PlayerList[i];
            SetPositionStartPoint(player);
        }
        
        Debug.Assert(mStageInfo != null, "널 들어옴");
        
        // 한 종류만 나옴
        MonsterType monsterType = mStageInfo.MonsterType;
        
        // 3마리 4마리 패턴중 랜덤리스트 가져옴
        
        // 구역(Area)별로 몬스터 스폰
        for (int i = 0; i < mStageInfo.AreaCount; i++)
        {
            var area = m_CurAreaList[i];
            
            for (int x = 0; x < area.SpawnMonsterCount; x++)
            {
                SpawnMonsterInRange(area.MonsterSpawnInfoList[i]
                    , monsterType
                    , parent: m_CurTilemapList[i].gameObject);
            }
        }

        // 보스 스테이지
        if (CurStageId % 3 == 0)
        {
            MonsterType type = mStageInfo.MonsterType;
            
            var boss = SpawnMonster(
                new Vector2(0f, 15f)
                , type
                , parent: m_CurTilemapList.Last().gameObject);
            
            boss.gameObject.transform.localScale = Vector3.one * 3f;
        }
        
        StageClearEvent.OnTriggerStageClearEvent(); // 맵 완성다 되었음.
    }
    
    /// <summary>
    /// 플레이어 캐릭터의 시작 위치를 설정합니다.
    /// **Instantiate하는게 아님**.
    /// </summary>
    /// <param name="character"></param>
    public void SetPositionStartPoint(Player character)
    {
        if (m_PlayerSpawnPointDic.TryGetValue(character.playerStat.characterClass
                , out var spawnPos))
        {
            character.transform.position = spawnPos;
        }
        else
        {
            Debug.Log("캐릭터 클래스에 맞는 스폰 위치가 없습니다.");
        }
    }

    /// <summary>
    /// 구역(Area)의 자식으로 몬스터를 스폰.
    /// </summary>
    /// <param name="type"> 몬스터 종류(MonsterType) </param>
    /// <param name="parent"> 구역(Area) </param>
    /// <returns> 스폰 몬스터 </returns>
    public BaseMonster SpawnMonsterInRange(SpawnInfo spawnInfo, MonsterType type, GameObject parent)
    {
        Vector2 randomOffset = Random.insideUnitCircle * spawnInfo.Radius;
        Vector2 spawnPos = spawnInfo.Point + randomOffset;
        BaseMonster monster = Instantiate(m_MonsterTable.GetMonster(type), parent.transform);
        monster.transform.localPosition = spawnPos; // 부모의 로컬 좌표로 스폰
        
        return monster;
    }
    
    public List<Tilemap> GenerateMap(int[] areaPatternList)
    {
        int areaPatternCount = areaPatternList.Length;
        float topY = 0f;
        
        List<Tilemap> areaList = new List<Tilemap>(areaPatternCount);
        
        for (int i = 0; i < areaPatternCount; i++)
        {
            var tilemap = m_AreaTilemapTable.GetTilemapOrNull(m_CurAreaList[i].PatternId);
            
            var curTileMap = Instantiate(tilemap
                , new Vector2(0, topY)
                , Quaternion.identity
                , parent: m_Grid.transform);

            areaList.Add(curTileMap);
            
            // Debug.Log(curTileMap.transform.position);
            topY += curTileMap.cellBounds.yMax;
            // topY = curTilemap.transform.position.y + curTileMap.localBounds.extents.y + cellSize.y * 0.5f;
        }

        // 보스 스테이지
        if (CurStageId % 3 == 0)
        {
            var bossTileMap = Instantiate(m_TilemapList[0]
                , new Vector2(0, topY)
                , Quaternion.identity
                , parent: m_Grid.transform);
            
            areaList.Add(bossTileMap);
            
            topY += bossTileMap.cellBounds.yMax;
        }
        
        m_StageEndPoint.transform.position = new Vector2(0, topY);
        return areaList;
    }
}
