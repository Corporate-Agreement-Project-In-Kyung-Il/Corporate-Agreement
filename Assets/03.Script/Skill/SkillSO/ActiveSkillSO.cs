using UnityEngine;

[CreateAssetMenu(fileName = "ActiveSkill", menuName = "ScriptableObjects/Skill/ActiveSkill")]
public class ActiveSkillSO : ScriptableObject,ISkillID
{
    public int SkillID { get;private set; }
    
    public int Skill_ID;
    public string Skill_Name;
    public SkillType Skill_Type;
    public int Skill_Minimum_LV;

    public int Skill_Maximum_LV;
    public float Skill_Cooldown;
    public float Skill_Damage;
    public int Skill_Attack_Count;
    public bool Wide_Area;
    public int Skill_Range_width;
    public int Skill_Range_height;
    public float Cooldown_Reduction;
    public float Damage_Increase;

    public virtual void UseSkill()
    {
        Debug.Log($"Use Skill: {Skill_Name}");
    }
    
}




