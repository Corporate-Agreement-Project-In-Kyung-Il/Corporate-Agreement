using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamgeEvent : MonoBehaviour
{
    public static event Action<MonsterController> enemydamageEvent;

    public static void OnTriggerMonsterDamageEvent(MonsterController monster)
    {
        enemydamageEvent?.Invoke(monster);
    }
}
