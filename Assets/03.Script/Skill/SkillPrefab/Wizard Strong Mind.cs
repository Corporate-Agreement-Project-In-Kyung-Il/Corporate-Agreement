using UnityEngine;

public class WizardStrongMind : BuffBase, ISkillID
{
    public SFXData buffSound;
    public int SkillId;
    public int SkillID { get; set; }
    public void SetSkillID() => SkillID = SkillId;

    public Player owner;
    public BuffSO buffSO;

    private float duration;
    private float timer;
    private float originalDamage;
    private bool initialized = false;
    private void Start()
    {
        SFXManager.Instance.Play(buffSound);
    }
    public void Initialize(Player _owner, BuffSO _buff)
    {
        if (initialized) return; // ✅ 중복 방지
        initialized = true;
        owner = _owner;
        buffSO = _buff;

        originalDamage = owner.buffplayerStat.attackDamage;
        owner.buffplayerStat.attackDamage *= 2.0f;

        duration = buffSO.Skill_Duration + buffSO.Duration_Increase;
        timer = 0f;
        

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