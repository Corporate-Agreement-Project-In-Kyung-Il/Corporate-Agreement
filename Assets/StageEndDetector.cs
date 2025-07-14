using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageEndDetector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.name);
        
        // TODO : 다음 스테이지 넘어가게
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Spawner.Instance.SpawnAllMonstersInStage();
        }
    }
}
