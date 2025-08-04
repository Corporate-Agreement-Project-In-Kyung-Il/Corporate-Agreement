using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPoint : MonoBehaviour
{
    //[SerializeField] private bool isFollow = false;

    
    private Vector2 size = new Vector2(5.5f, 10f);
    private Vector2 cameraPointPosition = new Vector2(0.5f, -2.65f);
    
    [SerializeField] private float currentY = -2.45f;
    [SerializeField] private float previousY = -2.46f;
    
    private void FixedUpdate()
    {
       // if (isFollow.Equals(false)) return;
       if (currentY > previousY) 
           previousY = currentY;
       

       Vector3 sumPositions = Vector3.zero;
        int count = 0;

        List<Collider2D> playerColliderList = AliveExistSystem.Instance.playerList;
        Collider2D[] overlapColliders = Physics2D.OverlapBoxAll(transform.position, size, 0f, LayerMask.GetMask("Player"));

        // 중복 검사 쉽게 하려고 HashSet 활용
        HashSet<Collider2D> overlapSet = new HashSet<Collider2D>(overlapColliders);

        // playerList에서 overlap된 콜라이더만 선택
        foreach (var col in playerColliderList)
        {
            if (overlapSet.Contains(col))
            {
                sumPositions += col.transform.position;
                count++;
            }
        }

        if (count > 0)
        {
            Vector3 averagePos = sumPositions / count;
            currentY = averagePos.y;
            
            if(currentY > previousY) 
                transform.position = averagePos;
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, size);
    }
    
    private void InitalPosition()
    {
        transform.position = cameraPointPosition;
        previousY = -2.66f;
        currentY = -2.65f;
    }
    private void OnEnable()
    {
        StageEvent.stageClearEvent += InitalPosition;
    }

    private void OnDisable()
    {
        StageEvent.stageClearEvent -= InitalPosition;
    }
}
