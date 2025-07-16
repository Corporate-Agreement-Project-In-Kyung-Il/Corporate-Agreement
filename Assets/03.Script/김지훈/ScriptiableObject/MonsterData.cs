using System.Collections;
using System.Collections.Generic;
using _03.Script.엄시형.Monster;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "ScriptableObjects/MonsterData")]
public class MonsterData : ScriptableObject
{
    //내가 추가한 것
    public MonsterType monsterType;
    public float attackRange;
    public float attackSpeed;
    public float moveSpeed;
    public float playerDetectionRange;
    
    //기존에 기획자 Table에 있는 것
    public int Stage_ID;
    
    public float Monster_HP;
    public float Monster_Attack;
    public float Monster_quantity;
    
    public bool Boss_Check;
    public float Boss_HP;
    public float Boss_Attack;

    public void SetMonsterData(MonsterExel data)
    {
        Monster_HP = data.Monster_HP;
        Monster_Attack = data.Monster_Attack;
        Boss_Check = data.IsBossStage;
        Boss_HP = data.Boss_HP;
        Boss_Attack = data.Boss_Attack;
    }
}
