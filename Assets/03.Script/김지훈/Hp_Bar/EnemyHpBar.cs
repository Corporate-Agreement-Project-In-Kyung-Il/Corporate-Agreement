using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpBar : MonoBehaviour
{
    public Transform target; // 따라갈 Enemy
    public Vector3 offset = new Vector3(0, -0.1f, 0); // 머리 위에 위치

    void LateUpdate()
    {
        if (target.gameObject.activeSelf == false)
        {
            Destroy(this.gameObject);
        }
        if (target == null) return;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position + offset);
        transform.position = screenPos;
    }
}
