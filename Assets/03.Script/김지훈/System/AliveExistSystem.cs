using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliveExistSystem : MonoBehaviour
{
    public static AliveExistSystem Instance;
    public List<Collider2D> monsterList = new List<Collider2D>();
    public List<Collider2D> playerList = new List<Collider2D>();
    
    private Collider2D col;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        TryGetComponent(out col);
    }
    
    private void OnDisable()
    {
        monsterList.Clear();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") && monsterList.Contains(other).Equals(false))
        {
            monsterList.Add(other);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && playerList.Contains(other).Equals(false))
        {
            playerList.Add(other);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (monsterList.Contains(other))
            monsterList.Remove(other);
        
        if (playerList.Contains(other))
            playerList.Remove(other);
    }
    
    public void RemoveEnemyFromList(Collider2D col)
    {
        if (monsterList.Contains(col))
            monsterList.Remove(col);
    }

    public void RemovePlayerFromList(Collider2D col)
    {
        if (playerList.Contains(col))
            playerList.Remove(col);
    }
}
