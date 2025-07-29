using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    private static readonly int Attack1 = Animator.StringToHash("Attack");
    public Animator playerAnimator;
    public Sprite CurrentSprite => sr.sprite;
    protected SpriteRenderer sr;
    protected Player player;
    protected Animator animator;

    public bool isSkill = false;

    [SerializeField] protected Sprite[] Level_Sprites;
    
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

    protected void changeWeapon()
    {
        int level = player.buffplayerStat.equip_level;

        if (level >= 0 && level < Level_Sprites.Length)
        {
            Debug.Log(player.gameObject.name);
            Debug.Log(level);
            sr.sprite = Level_Sprites[level];
        }
        else
        {
            Debug.LogWarning($"[Weapon] 레벨 {level}에 해당하는 스프라이트가 없습니다.");
        }
    }
    public abstract bool Attack(Collider2D collider);

    private void OnEnable()
    {
        StageEvent.equipmentEvent += changeWeapon;
    }

    private void OnDisable()
    {
        StageEvent.equipmentEvent -= changeWeapon;
    }
}
