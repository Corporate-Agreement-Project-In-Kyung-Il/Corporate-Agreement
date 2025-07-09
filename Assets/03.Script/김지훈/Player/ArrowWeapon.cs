using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowWeapon : Weapon
{
    public ArrowShot arrowShot;

    public override bool Attack(Collider2D collider)
    {
        if (collider.gameObject.TryGetComponent(out IDamageAble enemyDamage).Equals(false))
            return false;
        
        Debug.Log($"공격 대상: {enemyDamage.GameObject.name}, HP: {enemyDamage.CurrentHp}");
        
        //var bullet = Instantiate(arrowShot, transform.position, Quaternion.identity);
        ArrowShot bullet = ObjectPoolSystem.Instance.GetObjectOrNull("ArrowShot") as ArrowShot;
        
        bullet.transform.position = transform.position;
        bullet.arrowDamage = player.Damage;
        bullet.target = collider.transform;
        bullet.player = player;
        bullet.straightAttackRange = player.attackRange;
        
        bullet.gameObject.SetActive(true);
        
        return arrowShot.isTargetNotDead;
    }
    
}
