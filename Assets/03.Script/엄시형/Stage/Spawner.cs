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
    public int m_CurStageId = 1;
    [SerializeField] private float m_BossSpawnYOffset = 0f; 
    
    [SerializeField] private StageEndDetector m_StageEndDetector;
    
    [SerializeField] private StagePatternTableSO m_StagePatternTable;
    [SerializeField] private AreaTilemapTableSO m_AreaTilemapTable;
    [SerializeField] private MonsterStatExel m_MonsterStatTable;
    [SerializeField] private MonsterTableSO m_MonsterTable;
    [SerializeField] private StageInfoTableSO m_StageInfoTable;
    [SerializeField] private MonsterData m_MonsterData;
    [SerializeField] private PlayerCharacter m_CharacterTable;
    // 스킬 매니저 추가 했습니다.
    [SerializeField] private SkillManager m_SkillManager;
    
    [SerializeField] private List<Player> m_CharacterPrefabs;
    [SerializeField] private List<PlayerData> m_PlayerData;
    
    private List<Player> m_PlayerList = new List<Player>();
    
    [SerializeField] private GameObject m_Grid;  
    [SerializeField] private StageEndDetector m_StageEndPoint;
    [SerializeField] private GameObject m_reviveGameObject;
    
    private List<Tilemap> m_CurTilemapList = new List<Tilemap>();
    private List<AreaPattern> m_CurPatternList = new List<AreaPattern>();
    private StageTheme m_CurTheme = StageTheme.Grass; // 현재 테마, 초기값은 Grass로 설정
    private StageInfo m_StageInfo;
    private int m_CurStageInfoIndex = 0;
    
    
    private Dictionary<character_class, Vector2> m_PlayerSpawnPointDic = new Dictionary<character_class, Vector2>
    {
        { character_class.궁수, new Vector2(-0.5f, 1f) },
        { character_class.전사 , new Vector2(0.5f, 2f) },
        { character_class.마법사 , new Vector2(1.5f, 1f) }
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
        m_AreaTilemapTable.Init();

        m_CurStageInfoIndex = m_CurStageId switch
        {
            >= 400 => 4,
            >= 60 => 3,
            >= 45 => 2,
            >= 30 => 1,
            >= 15 => 0,
            _ => m_CurStageInfoIndex
        };

        m_StageInfo = m_StageInfoTable.GetStageInfoByIndex(m_CurStageInfoIndex);
        
        
         // m_StageInfo = m_StageInfoTable.GetStageInfoByIndex(m_CurStageInfoIndex);
        
        // m_StagePatternTable.Init();
        // var areas = m_AreaTilemapTable.m_AreaTilemaps[10001];
        
        GameManager.Instance.GameStart();
        
        // 캐릭터 스폰
        SpawnCharacters();
        m_SkillManager.SetPlayers(m_PlayerList.ToArray());
    }

    private void OnEnable()
    {
        m_StageEndDetector.OnStageEnd += SetStage;
    }
    
    private void OnDisable()
    {
        m_StageEndDetector.OnStageEnd -= SetStage;
    }

    public void SetStage()
    {
        m_CurStageId++;
        
        // 선택지
        if (m_CurStageId % 3 == 0)
        {
            GameManager.Instance.CreateChoices(3);
        }
        
        // 보스 이후 테마 변경
        if (m_CurStageId % 3 == 1)
        {
            var prevTheme = m_CurTheme;
        
            // 전 테마랑 같지 않게 랜덤으로 테마 설정
            do
            {
                var stageThemes = Enum.GetValues(typeof(StageTheme));
                m_CurTheme = (StageTheme) Random.Range(1, stageThemes.Length);
            } while (m_CurTheme == prevTheme);
        
            // Debug.Log(m_CurTheme);
        }
        

        
        // 15 30 45 60 400 넘어가면 몬스터 수, 구역 패턴 변화
        if (m_StageInfo.MaxStage < m_CurStageId)
        {
            ++m_CurStageInfoIndex;
            m_StageInfo = m_StageInfoTable.GetStageInfoByIndex(m_CurStageInfoIndex);
        }
        
        // 테이블에서 몬스터 정보를 가져와 몬스터 스탯에 적용 SO
        m_MonsterData.SetMonsterData(m_MonsterStatTable.GetValue(m_CurStageId));
        
        // 모든 구역 파괴 (몬스터도 구역 하위로 들어가 사라짐)
        DestoryAllArea();
        
        // 몬스터 수에 따라 스폰 패턴 랜덤으로 
        GetRandomPattern();
        m_CurTilemapList = GenerateMap(m_StageInfo.SpawnMonsterCounts);
        SpawnAllMonstersInStage();
    }
    
    private void Start()
    {
        // m_MonsterData.SetMonsterData(m_MonsterStatTable.GetValue(CurStageId));
        // mStageInfo = m_StagePatternTable.GetAreaList(1);
        m_MonsterData.SetMonsterData(m_MonsterStatTable.GetValue(m_CurStageId));
        GetRandomPattern();
        m_CurTilemapList = GenerateMap(m_StageInfo.SpawnMonsterCounts);
        
        SpawnAllMonstersInStage();
    }

    // private Player SpawnPlayer(Vector2 position, character_class characterClass)
    // {
    //     return Instantiate(mMonsterTable.GetMonster(type)
    //         , position
    //         , Quaternion.identity);
    // }
    
    
    /// <summary>
    /// parent
    /// </summary>
    /// <param name="position"></param>
    /// <param name="type"></param>
    /// <param name="parent">구역</param>
    /// <returns></returns>
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

    private void DestoryAllArea()
    {
        for (int i = 0; i < m_CurTilemapList.Count; i++)
        {
            Destroy(m_CurTilemapList[i].gameObject);
        }
        
        m_CurTilemapList.Clear();
    }

    private void GetRandomPattern()
    {
        m_CurPatternList = new List<AreaPattern>(m_StageInfo.AreaCount);
        
        // TODO: 테마로 가져오기
        for (int i = 0; i < m_StageInfo.AreaCount; i++)
        {
            // 테마에서 가져오기
            AreaPattern areaPattern = 
                m_StagePatternTable.GetRandomSpawnPattern(m_StageInfo.SpawnMonsterCounts[i]);
            
            m_CurPatternList.Add(areaPattern);
        }
    }

    private void SpawnAllMonstersInStage()
    {
        for (var i = 0; i < m_PlayerList.Count; i++)
        {
            var player = m_PlayerList[i];
            SetPositionStartPoint(player);
        }
        
        Debug.Assert(m_StageInfo != null, "널 들어옴");
        
        // 한 종류만 나옴
        // MonsterType monsterType = m_StageInfo.MonsterType;
        MonsterType monsterType = MonsterType.Slime;
        // 3마리 4마리 패턴중 랜덤리스트 가져옴
        
        // 구역(Area)별로 몬스터 스폰
        for (int i = 0; i < m_StageInfo.AreaCount; i++)
        {
            var area = m_CurPatternList[i];
            
            for (int x = 0; x < area.SpawnMonsterCount; x++)
            {
                SpawnMonsterInRange(area.MonsterSpawnInfoList[x]
                    , monsterType
                    , parent: m_CurTilemapList[i].gameObject);
            }
        }

        // 보스 스테이지
        if (m_CurStageId % 3 == 0)
        {
            Tilemap bossTilemap = m_CurTilemapList.Last();
            
            bossTilemap.CompressBounds();
            
            var boss = SpawnMonster(
                new Vector2(bossTilemap.localBounds.center.x, bossTilemap.localBounds.max.y - m_BossSpawnYOffset)
                , monsterType
                , parent: bossTilemap.gameObject);
            
            boss.gameObject.transform.localScale = Vector3.one * 3f;
        }
        
        StageEvent.OnTriggerStageClearEvent(); // 맵 완성다 되었음.
    }
    
    /// <summary>
    /// 플레이어 캐릭터의 시작 위치를 설정합니다. <br/>
    /// <b>Instantiate하는게 아님</b>
    /// </summary>
    /// <param name="character"></param>
    private void SetPositionStartPoint(Player character)
    {
        if (m_PlayerSpawnPointDic.TryGetValue(character.buffplayerStat.characterClass
                , out var spawnPos))
        {
            character.transform.position = spawnPos;
        }
        else
        {
            Debug.Log("캐릭터 클래스에 맞는 스폰 위치가 없습니다.");
        }
    }

    private void SpawnCharacters()
    {
        // index 0 전사 100001
        // index 1 궁수 100004
        // index 2 마법사 100006
        // var playerList = PlayerList.Instance.CharacterIDs;
        // TODO : 플레이어 ID를 외부에서 받아오는 로직으로 변경 필요
        // 임시로 0, 1, 2로 설정
        var playerList = new int[] {100001, 100004, 100006};
        
        for (int i = 0; i < playerList.Length; i++)
        {
            // TODO: 플레이어 프리팹을 ID로 찾는 로직으로 변경 필요
            // 프리맵이랑 매핑필요
            character_class characterClass = (playerList[i]) switch
            {
                100001 => character_class.전사,
                100004 => character_class.궁수,
                100006 => character_class.마법사,
                _ => throw new Exception("잘못된 캐릭터 클래스 ID입니다.")
            };

            // Prefab에서 캐릭터를 찾음
            // TODO : 지금은 Class를 찾지만 특수 캐릭터 프리팹 추가하면 ID로 찾아야함
            // Player player = m_CharacterPrefabs.Find(charactrer =>
            // {
            //     return charactrer.buffplayerStat.characterClass == characterClass;
            // });
            
            // Player player = null;
            // foreach (var p in m_CharacterPrefabs)
            // {
            //     if (p.buffplayerStat.characterClass == characterClass)
            //     {
            //         player = p;
            //         break;
            //     }
            // }

            var playerData = m_PlayerData.Find(data =>
            {
                return data.character_ID == playerList[i];
            });
            
            if (m_PlayerSpawnPointDic
                .TryGetValue(characterClass, out Vector2 spawnPos))
            {
                var spawnedCharacter = Instantiate(m_CharacterPrefabs[i], spawnPos, Quaternion.identity
                    , parent: m_reviveGameObject.transform);
                spawnedCharacter.initialSetPlayerStats(playerData);
                m_PlayerList.Add(spawnedCharacter);
            }
        }
    }
    
    /// <summary>
    /// 구역(Area)의 자식으로 몬스터를 스폰.
    /// </summary>
    /// <param name="type"> 몬스터 종류(MonsterType) </param>
    /// <param name="parent"> 구역(Area) </param>
    /// <returns> 스폰 몬스터 </returns>
    public void SpawnMonsterInRange(SpawnInfo spawnInfo, MonsterType type, GameObject parent)
    {
        Vector2 randomOffset = Random.insideUnitCircle * (spawnInfo.Radius * 0.5f);
        // Debug.Log(randomOffset);
        Vector2 spawnPos = spawnInfo.Point + randomOffset;
        // Vector2 spawnPos = spawnInfo.Point;
        BaseMonster monster = Instantiate(m_MonsterTable.GetMonster(type), parent.transform);
        monster.transform.localPosition = spawnPos; // 부모의 로컬 좌표로 스폰
    }
    
    public List<Tilemap> GenerateMap(int[] areaPatternList)
    {
        int areaPatternCount = areaPatternList.Length;
        float topY = 0f;
        
        List<Tilemap> areaList = new List<Tilemap>(areaPatternCount);
        
        var themeAreaList = m_AreaTilemapTable.GetThemeAreaList(m_CurTheme);
        
        for (int i = 0; i < areaPatternCount; i++)
        {
            var tilemap = themeAreaList.Find((map) =>
            {
                return map.Id == areaPatternList[i];
            }).Tilemap;
            
            var curTileMap = Instantiate(tilemap
                , new Vector2(0, topY)
                , Quaternion.identity
                , parent: m_Grid.transform);

            // 사이즈가 이상하게 나와서 CompressBounds를 통해 사이즈를 재조정
            curTileMap.CompressBounds();
            
            areaList.Add(curTileMap);
            // Debug.Log(curTileMap.transform.position);
            topY += curTileMap.cellBounds.yMax;
            // topY = curTilemap.transform.position.y + curTileMap.localBounds.extents.y + cellSize.y * 0.5f;
        }

        // 보스 스테이지
        if (m_CurStageId % 3 == 0)
        {
            // 보스 스테이지 타일맵 ID
            var tilemap = m_AreaTilemapTable.GetTilemap(m_CurTheme, -1);
            
            var bossTileMap = Instantiate(tilemap
                , new Vector2(0, topY)
                , Quaternion.identity
                , parent: m_Grid.transform);
            
            areaList.Add(bossTileMap);
            tilemap.CompressBounds();
            topY += bossTileMap.cellBounds.yMax;
        }
        
        m_StageEndPoint.transform.position = new Vector2(0, topY);
        
        return areaList;
    }
}
