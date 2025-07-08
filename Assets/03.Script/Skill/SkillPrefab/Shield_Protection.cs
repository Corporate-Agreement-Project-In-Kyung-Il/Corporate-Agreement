using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield_Protection : MonoBehaviour
{
    void Start()
    {
        Debug.Log("start Shield_Protection");
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
