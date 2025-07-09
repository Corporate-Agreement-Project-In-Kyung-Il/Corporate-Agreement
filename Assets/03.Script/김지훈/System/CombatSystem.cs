using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    private const int MAX_EVENT_PROCESS_COUNT = 10;
    
    public class Callbacks
    {
        public Action<CombatEvent> OnCombatEvent;
    }
    
    public static CombatSystem instance;
    private Dictionary<Collider2D, IDamageAble> monsterDic = new Dictionary<Collider2D, IDamageAble>();

    private Queue<CombatEvent> CombatEventQueue = new Queue<CombatEvent>();
    public readonly Callbacks Events = new Callbacks();

    private void Awake()
    {
        instance = this;
    }
    
    
    void Update()
    {
        int processCount = 0;

        while (CombatEventQueue.Count > 0 && processCount < MAX_EVENT_PROCESS_COUNT)
        {
            var combatEvent = CombatEventQueue.Dequeue();
            processCount++;
            combatEvent.Receiver.TakeDamage(combatEvent);
            Events.OnCombatEvent?.Invoke(combatEvent);

            processCount++;
        }
    }

    public void RegisterMonster(IDamageAble monster)
    {
        if (monsterDic.TryAdd(monster.mainCollider, monster) == false)
        {
            Debug.LogWarning($"{monster.GameObject.name}가 등록되었습니다. {monsterDic[monster.mainCollider]}를 대체합니다");
            monsterDic[monster.mainCollider] = monster;
        }
    }

    public void AddCombatEvent(CombatEvent combatEvent)
    {
        CombatEventQueue.Enqueue(combatEvent);
    }
}

public class CombatEvent
{
    public IDamageAble Sender;
    public IDamageAble Receiver;

    public float Damage { get; set; }
    //public Vector3 HitPosition { get; set;}
    public Collider2D collider { get; set;}
}