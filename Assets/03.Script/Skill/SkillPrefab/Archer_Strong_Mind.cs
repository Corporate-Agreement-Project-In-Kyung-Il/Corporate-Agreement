using UnityEngine;

public class Archer_Strong_Mind : MonoBehaviour, ISkillID
{
    public int SkillId;
    public int SkillID { get; set; }
    public void SetSkillID() => SkillID = SkillId;

    public Player_fusion owner;
    public BuffSO buffSO;

    private float duration;
    private float timer;
    private bool initialized = false;

    public void Initialize(Player_fusion _owner, BuffSO _buff)
    {
        owner = _owner;
        buffSO = _buff;

        owner.SetBuffState(BuffEffectType.Archer_Strong_Mind, true);

        duration = buffSO.Skill_Duration + buffSO.Duration_Increase;
        timer = 0f;
        initialized = true;

        Debug.Log("🏹 아처 스트롱 마인드 ON");
    }

    void Update()
    {
        if (!initialized) return;

        timer += Time.deltaTime;
        if (timer >= duration)
        {
            owner.SetBuffState(BuffEffectType.Archer_Strong_Mind, false);
            Debug.Log("🏹 아처 스트롱 마인드 OFF");
            Destroy(gameObject);
        }
    }
}