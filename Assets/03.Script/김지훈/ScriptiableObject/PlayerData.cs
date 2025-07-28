using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData")]
public class PlayerData : ScriptableObject
{
    //따로 내가 추가한 변수(지훈)
    public float attackRange; // 공격 범위
    public float moveSpeed; //기본 이동 속도
    public bool isDead = false;   
    //기존 기획 테이블에 있는 것들
    public int character_ID;

    
    public character_class characterClass; //캐릭터 직업
    public character_name characterName; //캐릭터 이름
    public character_grade characterGrade; //캐릭터 등급
    
    public float attackDamage; // 기본 공격력
    public float health; // 기본 체력
    public float attackSpeed; //기본 공격 속도
    public float criticalProbability; //기본 크리티컬 발동 확률

    
    public int training_type; //훈련 적용 분류
    public int equip_item; //장비 적용 분류
    public int equip_level;
    public List<int> skill_possed; //스킬 적용

    public Sprite playerUISprite;
}
