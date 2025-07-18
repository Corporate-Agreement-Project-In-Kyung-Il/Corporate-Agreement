using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(BoxCollider2D))]
public class ConfinerSizeController : MonoBehaviour
{
    [Header("카메라가 렌더링할 범위의 tilemapCollider들")]
    public List<TilemapCollider2D> tilemapColliders = new List<TilemapCollider2D>(); // 여러 개 할당 가능

    public TilemapCollider2D decoMap;
    private BoxCollider2D boxCollider;

    void Awake()
    {
        TryGetComponent(out boxCollider);
        StageClearEvent.stageClearEvent += GenerateLimitMap;
    }

    void UpdateBoxCollider()
    {
        // 첫 번째 bounds로 초기화
        Bounds combinedBounds = tilemapColliders[0].bounds;

        // 나머지 bounds 합치기
        for (int i = 1; i < tilemapColliders.Count; i++)
        {
            combinedBounds.Encapsulate(tilemapColliders[i].bounds);
        }

        // BoxCollider2D 위치와 크기 세팅 (월드 좌표 기준)
        boxCollider.offset = combinedBounds.center - transform.position;
        boxCollider.size = combinedBounds.size;
    }

    public void RemoveTileMap()
    {
        tilemapColliders.Clear();
    }

    private void GenerateLimitMap()
    {
        RemoveTileMap();
        Debug.Log("XXXXX");
        for (int i = 0; i < Spawner.Instance.CurAreaList.Count; i++)
        {
            Debug.Log("들어가기 시도 중");
            if (Spawner.Instance.CurAreaList[i].gameObject.TryGetComponent(out TilemapCollider2D tilemapCol))
            {
                Debug.Log("TileMap 들어감");
                tilemapColliders.Add(tilemapCol);
            }
        }
        tilemapColliders.Add(decoMap);
        UpdateBoxCollider();
    }
    
    private void OnEnable()
    {
        StageClearEvent.stageClearEvent += GenerateLimitMap;
    }

    private void OnDisable()
    {
        StageClearEvent.stageClearEvent -= GenerateLimitMap;
    }
}