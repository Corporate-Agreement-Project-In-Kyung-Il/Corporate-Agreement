using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeastClaw : MonoBehaviour
{
    void Start()
    {
        Debug.Log("start BeastClaw");
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
