using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandWeapon : Weapon_fusion
{
    public MagicBall magicBall;
    
    public override bool Attack(Collider2D collider)
    {
        if (collider.gameObject.TryGetComponent(out IDamageAble enemyDamage).Equals(false))
            return false;
        
        //Debug.Log($"공격 대상: {enemyDamage.GameObject.name}, HP: {enemyDamage.CurrentHp}");
        

        //var bullet = Instantiate(magicBall, transform.position, Quaternion.identity);
        MagicBall_fusion bullet = ObjectPoolSystem.Instance.GetObjectOrNull("MagicBall") as MagicBall_fusion;
        
        bullet.transform.position = transform.position;
        bullet.magicDamage = player.Damage;
        bullet.target = collider.transform;
        bullet.player = player;
        
        
        bullet.gameObject.SetActive(true);
        
        return magicBall.isTargetNotDead;
    }
    
}
