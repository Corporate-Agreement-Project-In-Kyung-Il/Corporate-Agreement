using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public interface IDamageAble
{
    public Collider2D mainCollider { get; }
    public GameObject GameObject { get; }
    public float Damage { get; }
    
    public float CurrentHp { get; }
    public void TakeDamage(CombatEvent combatEvent);
}

public interface IBuffSelection
{ 
    public PlayerStat buffplayerStat { get; }
}

public interface ISpriteSelection
{
    public Sprite PlayerSprite { get; }
    public Sprite WeaponSprite { get; }
}

[System.Serializable]
public class PlayerStat
{
    //내가 따로 넣어준 것들(지훈)
    public float attackRange;
    public float moveSpeed;
    public Vector2 detectionRange;
    public bool isDead;
    
    //기존 기획 Table에 있는 것들
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

}

public enum character_class
{
    전사,
    궁수,
    마법사,
    Null
}

public enum character_name
{
    Null,
    [Description("기본 전사")]
    기본_전사,
    아이언,
    디노,
    [Description("기본 궁수")]
    기본_궁수,
    사비나,
    [Description("기본 마법사")]
    기본_마법사,
    쿠아
}

public enum character_grade
{
    A,
    S
}

