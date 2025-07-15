using UnityEngine;

public class Shield_Protection : MonoBehaviour, ISkillID
{
    public int SkillId;
    public int SkillID { get; set; }
    public void SetSkillID() => SkillID = SkillId;

    public Player owner;
    public BuffSO buffSO;

    private float duration;
    private float timer;
    private float activationRate;
    private bool initialized = false;

    public void Initialize(Player _owner, BuffSO _buff)
    {
        owner = _owner;
        buffSO = _buff;

        activationRate = buffSO.Skill_Activation_Rate + buffSO.Activation_Rate_Increase;
        owner.shieldBlockChance += activationRate;

        duration = buffSO.Skill_Duration + buffSO.Duration_Increase;
        timer = 0f;
        initialized = true;

        Debug.Log($"🛡️ 방어 확률 버프 ON: {owner.shieldBlockChance}");
    }

    void Update()
    {
        if (!initialized) return;

        timer += Time.deltaTime;
        if (timer >= duration)
        {
            owner.shieldBlockChance -= activationRate;
            Debug.Log($"🛡️ 방어 확률 버프 OFF: {owner.shieldBlockChance}");
            Destroy(gameObject);
        }
    }
}