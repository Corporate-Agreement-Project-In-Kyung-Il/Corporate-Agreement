using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WarriorStrongMind : MonoBehaviour, ISkillID
{
    public int SkillId;
    public int SkillID { get; set; }

    public void SetSkillID()
    {
        SkillID = SkillId;
    }

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