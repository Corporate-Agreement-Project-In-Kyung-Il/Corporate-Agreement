using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamgeEvent
{
    public static event Action<MonsterController> enemydamageEvent;
    public static event Action<Player> playerDamageEvent;
    public static event Action ShakeEvent;

    public static void OnTriggerMonsterDamageEvent(MonsterController monster)
    {
        enemydamageEvent?.Invoke(monster);
    }

    public static void OnTriggerPlayerDamageEvent(Player player)
    {
        playerDamageEvent?.Invoke(player);
    }

    public static void OnTriggerShake()
    {
        ShakeEvent?.Invoke();
    }
}

public class StageClearEvent
{
    public static event Action stageClearEvent;

    public static void OnTriggerStageClearEvent()
    {
        Debug.Log("TileMap 생성 신호 발생");
        stageClearEvent?.Invoke();
    }
}
