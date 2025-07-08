using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorStrongMind : MonoBehaviour
{
    void Start()
    {
        Debug.Log("start WarriorStrongMind");
    }

    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            //데미지입힘
        }
    }
}
