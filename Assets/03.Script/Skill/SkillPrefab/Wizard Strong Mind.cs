using UnityEngine;

public class WizardStrongMind : MonoBehaviour, ISkillID
{
    public Sprite SkillSprite { get; set; }
    public Sprite skillSprite;
    public void SetSkillSprite()
    {
        SkillSprite = skillSprite;
    }
    public int SkillId;
    public int SkillID { get; set; }
    public void SetSkillID() => SkillID = SkillId;

    public Player owner;
    public BuffSO buffSO;

    private float duration;
    private float timer;
    private float originalDamage;
    private bool initialized = false;

    public void Initialize(Player _owner, BuffSO _buff)
    {
        SetSkillSprite();
        owner = _owner;
        buffSO = _buff;

        originalDamage = owner.buffplayerStat.attackDamage;
        owner.buffplayerStat.attackDamage *= 2f;

        duration = buffSO.Skill_Duration + buffSO.Duration_Increase;
        timer = 0f;
        initialized = true;

        Debug.Log($"💥 마법사 공격력 2배 ON: {owner.buffplayerStat.attackDamage}");
    }

    void Update()
    {
        if (!initialized) return;

        timer += Time.deltaTime;
        if (timer >= duration)
        {
            owner.buffplayerStat.attackDamage = originalDamage;
            Debug.Log($"💥 마법사 공격력 2배 OFF: {owner.buffplayerStat.attackDamage}");
            Destroy(gameObject);
        }
    }
}