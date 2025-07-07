using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowWeapon : Weapon
{
    private SpriteRenderer sr;
    private Player player;
    public GameObject arrow;

    void Start()
    {
        TryGetComponent(out sr); //sr 장착한 검에 대해서 모형 변화
        player = GetComponentInParent<Player>();        
    }

    public override bool Attack(Collider2D collider)
    {
        return false;
    }
    
}
