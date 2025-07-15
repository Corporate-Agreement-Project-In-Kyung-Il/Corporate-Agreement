using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamgeEvent : MonoBehaviour
{
    public static event Action<MonsterController> enemydamageEvent;
    
    public static event Action<Player> playerDamageEvent;

    public static void OnTriggerMonsterDamageEvent(MonsterController monster)
    {
        enemydamageEvent?.Invoke(monster);
    }

    public static void OnTriggerPlayerDamageEvent(Player player)
    {
        playerDamageEvent?.Invoke(player);
    }
}

public class StageClearEvent : MonoBehaviour
{
    public static event Action stageClearEvent;

    public static void OnTriggerStageClearEvent()
    {
        Debug.Log("TileMap 생성 신호 발생");
        stageClearEvent?.Invoke();
    }
}
