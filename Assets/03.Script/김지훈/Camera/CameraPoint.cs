using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPoint : MonoBehaviour
{
    [SerializeField] private bool isFollow = false;
    private Vector2 size = new Vector2(5.5f, 10f);
    
    private void Update()
    {
        //카메라
        if (isFollow.Equals(false))
            return;

        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, size, 0f, LayerMask.GetMask("Player"));

        if (colliders.Length == 0)
            return;

        Vector3 points = Vector3.zero;

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].TryGetComponent(out ICameraPosition cameraPosition) && cameraPosition.cameraMoveTransform != null)
            {
               // Debug.Log($"cameraPosition.canMove = {cameraPosition.canMove}");
               if (cameraPosition.canMove) 
                   points += cameraPosition.cameraMoveTransform.position;
            }
        }

        transform.position = points / colliders.Length;
        
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, size);
    }
}
