using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherStrongMind : MonoBehaviour,ISkillID
{
    public int SkillId;
    void Start()
    {
        Debug.Log("start ArcherStrongMind");
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

    public int SkillID { get; set; }
    public void SetSkillID()
    {
        SkillID = SkillId;
    }
}
