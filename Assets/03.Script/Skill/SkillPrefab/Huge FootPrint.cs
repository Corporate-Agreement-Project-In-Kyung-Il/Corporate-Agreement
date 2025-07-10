using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HugeFootPrint : MonoBehaviour,ISkillID
{//광역기 한번때림
    public int SkillId;
    public int SkillID { get; set; }
    public void SetSkillID()
    {
        SkillID = SkillId;
    }
    void Start()
    {
        Debug.Log("start HugeFootPrint");
    }

    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            //데미지입힘
        }
    }
}
