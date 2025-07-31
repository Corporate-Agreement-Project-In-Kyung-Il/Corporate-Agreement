using UnityEngine;

public class Archer_Strong_Mind : MonoBehaviour, ISkillID
{
    public SFXData buffSound;
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
    private bool initialized = false;
    private void Start()
    {
        SFXManager.Instance.Play(buffSound);
    }
    public void Initialize(Player _owner, BuffSO _buff)
    {
        SetSkillSprite();
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