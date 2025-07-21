using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveSkillBase : MonoBehaviour
{
    //owner로 데이터 받아옴
    //Instantiate 할때 같이 정보 넘겨줌
    public Player owner;

    //public Player owner;
    public SkillStat stat = new SkillStat();

    public class SkillStat
    {
        public int Minimum_LV;
        public int current_LV;
        public int Maximum_LV;

        public float Cooldown;
        public float Damage;
        public int Attack_Count;
        public bool Wide_Area;

        public float Range_width;
        public float Range_height;

        public float Cooldown_Reduction;
        public float Damage_Increase;
    }

    public abstract void Initialize();
}