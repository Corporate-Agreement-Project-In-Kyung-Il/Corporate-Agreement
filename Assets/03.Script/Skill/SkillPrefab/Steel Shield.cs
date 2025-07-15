using UnityEngine;

public class SteelShield : MonoBehaviour, ISkillID
{
    public int SkillId;
    public int SkillID { get; set; }
    public void SetSkillID() => SkillID = SkillId;

    public Player owner;
    public BuffSO buffSO;

    private float duration;
    private float timer;
    private float reductionRate = 0.6f;
    private bool initialized = false;

    public SpriteRenderer sr;
    void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
    }
    public void Initialize(Player _owner, BuffSO _buff)
    {
        owner = _owner;
        buffSO = _buff;

        owner.damageReductionRate += reductionRate;

        duration = buffSO.Skill_Duration + buffSO.Duration_Increase;
        timer = 0f;
        initialized = true;

        Debug.Log($"🛡️ 데미지 감소 ON: {owner.damageReductionRate}");
    }

    void Update()
    {
        if (!initialized) return;

        timer += Time.deltaTime;
        if (timer >= duration)
        {
            owner.damageReductionRate -= reductionRate;
            Debug.Log($"🛡️ 데미지 감소 OFF: {owner.damageReductionRate}");
            Destroy(gameObject);
        }
    }
}