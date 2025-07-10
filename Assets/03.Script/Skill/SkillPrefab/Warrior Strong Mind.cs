using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WarriorStrongMind : MonoBehaviour, ISkillID
{//단일 공격 3번때림 
    public int SkillId;
    public int SkillID { get; set; }

    public void SetSkillID()
    {
        SkillID = SkillId;
    }
 
    //owner로 데이터 받아옴
    public Player owner;
    public int attackCount;
    void Start()
    {
        Debug.Log("start WarriorStrongMind");
        AttakcTarget();
    }

    void Update()
    {
        
    }

    private void AttakcTarget()
    {
        //owner.target에게 데미지를 입힘
        
        attackCount++;
        AttakcTarget();
    }

}