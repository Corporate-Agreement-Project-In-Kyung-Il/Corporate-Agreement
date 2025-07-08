using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield_Protection : MonoBehaviour,ISkillID
{
    public int SkillId;
    public int SkillID { get; set; }
    public void SetSkillID()
    {
        SkillID = SkillId;
    }
    void Start()
    {
        Debug.Log("start Shield_Protection");
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
