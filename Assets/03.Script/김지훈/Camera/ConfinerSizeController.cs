using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(BoxCollider2D))]
public class ConfinerSizeController : MonoBehaviour
{
    public TilemapCollider2D[] tilemapColliders; // 여러 개 할당 가능

    private BoxCollider2D boxCollider;

    void Awake()
    {
        TryGetComponent(out boxCollider);
        UpdateBoxCollider();
    }

    void UpdateBoxCollider()
    {
        // 첫 번째 bounds로 초기화
        Bounds combinedBounds = tilemapColliders[0].bounds;

        // 나머지 bounds 합치기
        for (int i = 1; i < tilemapColliders.Length; i++)
        {
            combinedBounds.Encapsulate(tilemapColliders[i].bounds);
        }

        // BoxCollider2D 위치와 크기 세팅 (월드 좌표 기준)
        boxCollider.offset = combinedBounds.center - transform.position;
        boxCollider.size = combinedBounds.size;
    }
}