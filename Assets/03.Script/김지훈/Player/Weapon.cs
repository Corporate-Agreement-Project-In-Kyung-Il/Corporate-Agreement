using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    private static readonly int Attack1 = Animator.StringToHash("Attack");
    public Animator playerAnimator;
    protected SpriteRenderer sr;
    protected Player player;
    protected Animator animator;
    
    protected void Start()
    {
        TryGetComponent(out animator);
        TryGetComponent(out sr); //sr 장착한 검에 대해서 모형 변화
        player = GetComponentInParent<Player>();
        
    }

    protected void Update()
    {
        if (playerAnimator.GetBool("isAttack"))
        {
            animator.SetBool(Attack1, true);
        }
        else
        {
            animator.SetBool(Attack1, false);
        }
        
    }
    public abstract bool Attack(Collider2D collider);
}
