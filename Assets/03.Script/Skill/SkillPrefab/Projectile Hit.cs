using System.Collections;
using UnityEngine;

public class ProjectileHit : MonoBehaviour, ISkillID
{
    public int SkillId;
    public int SkillID { get; set; }
    public void SetSkillID()
    {
        SkillID = SkillId;
    }

    public Player owner;
    public BuffSO buffSO;

    private float duration;
    private float timer;

    private float originalAttackSpeed;

    public void Initialize(Player _owner, BuffSO _buff)
    {
        owner = _owner;
        buffSO = _buff;

        originalAttackSpeed = owner.buffplayerStat.attackSpeed;
        owner.buffplayerStat.attackSpeed *= 2f;// 맛도 안나네 띠발

        duration = buffSO.Skill_Duration + buffSO.Duration_Increase;
        timer = 0f;
        initialized = true;
        Debug.Log("공속 버프 on :" +owner.buffplayerStat.attackSpeed);
    }

    private bool initialized = false;

    void Update()
    {
        if (!initialized) return;

        timer += Time.deltaTime;
        if (timer >= duration)
        {
            owner.buffplayerStat.attackSpeed = originalAttackSpeed;
            Debug.Log("공속 버프 off:" +owner.buffplayerStat.attackSpeed);
            Destroy(gameObject);
        }
    }

    //void OnDisable()
    //{
    //    if (owner != null)
    //    {
    //        owner.playerStat.attackSpeed = originalAttackSpeed;
    //    }
    //}
}