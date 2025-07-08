using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandWeapon : Weapon
{
    private SpriteRenderer sr;
    private Player player;
    public MagicBall magicBall;
    void Start()
    {
        TryGetComponent(out sr); //sr 장착한 검에 대해서 모형 변화
        player = GetComponentInParent<Player>();
    }
    
    
    public override bool Attack(Collider2D collider)
    {
        if (collider.gameObject.TryGetComponent(out IDamageAble enemyDamage).Equals(false))
            return false;
        
        Debug.Log($"공격 대상: {enemyDamage.GameObject.name}, HP: {enemyDamage.CurrentHp}");
        
        var bullet = Instantiate(magicBall, transform.position, Quaternion.identity, transform);

        bullet.magicDamage = player.Damage;
        bullet.target = collider.transform;
        bullet.player = player;
        
        return magicBall.isTargetNotDead;
    }
    
}
