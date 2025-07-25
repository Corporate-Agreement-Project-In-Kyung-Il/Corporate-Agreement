using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordWeapon : Weapon
{


    public override bool Attack(Collider2D collider)
    {
        if (collider.gameObject.TryGetComponent(out IDamageAble enemyDamage).Equals(false))
            return false;
        
       // Debug.Log($"공격 대상: {enemyDamage.GameObject.name}, HP: {enemyDamage.CurrentHp}");
            
        CombatEvent combatEvent = new CombatEvent();
        combatEvent.Receiver = enemyDamage;
        combatEvent.Sender = player;
        combatEvent.Damage = player.Damage;
        if (Random.value < player.playerStat.criticalProbability)
        {
            combatEvent.Damage =player.Damage* 2f;
        }
        combatEvent.collider = enemyDamage.mainCollider;
            
        CombatSystem.instance.AddCombatEvent(combatEvent);

        if (enemyDamage.CurrentHp > 0)
        {
            return true;
        }
        return false;
    }
}
