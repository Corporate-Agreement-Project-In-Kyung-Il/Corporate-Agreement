using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordWeapon : Weapon
{
    private SpriteRenderer sr;
    private Player player;
    
    private void Start()
    {
        TryGetComponent(out sr); //sr 장착한 검에 대해서 모형 변화
        player = GetComponentInParent<Player>();
    }

    public override bool Attack(Collider2D collider)
    {
        if (collider.gameObject.TryGetComponent(out IDamageAble enemyDamage).Equals(false))
            return false;
        
        Debug.Log($"공격 대상: {enemyDamage.GameObject.name}, HP: {enemyDamage.CurrentHp}");
            
        CombatEvent combatEvent = new CombatEvent();
        combatEvent.Receiver = enemyDamage;
        combatEvent.Sender = player;
        combatEvent.Damage = player.Damage;
        combatEvent.collider = enemyDamage.mainCollider;
            
        CombatSystem.instance.AddCombatEvent(combatEvent);

        if (enemyDamage.CurrentHp > 0)
        {
            return true;
        }
        return false;
    }
}
