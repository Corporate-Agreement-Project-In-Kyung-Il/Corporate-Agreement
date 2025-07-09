using System.Collections;
using UnityEngine;

public class WizardStrongMind : MonoBehaviour, ISkillID
{
    public int SkillId;
    public int SkillID { get; set; }
    public float duration = 3f; // 지속 시간

    private float originalAttackDamage;
    private Player_jin player;

    public void SetSkillID()
    {
        SkillID = SkillId;
    }

    void Start()
    {
        Debug.Log("start WizardStrongMind");

        player = FindObjectOfType<Player_jin>();

        if (player != null)
        {
            // 중복 버프 확인
            if (player.HasBuff(BuffEffectType.Wizard_Strong_Mind))
            {
                Debug.Log("이미 Wizard_Strong_Mind 버프가 적용되어 있음. 중복 적용 안함.");
                Destroy(gameObject);
                return;
            }

            // 버프 상태 등록
            player.SetBuffState(BuffEffectType.Wizard_Strong_Mind, true);

            // 원래 공격력 저장 및 2배 적용
            originalAttackDamage = player.Damage;
            player.SetAttackDamage(originalAttackDamage * 2.0f);

            StartCoroutine(EndBuffAfterDuration());
        }
        else
        {
            Debug.LogWarning("[WizardStrongMind] Player_jin 찾기 실패");
        }
    }

    private IEnumerator EndBuffAfterDuration()
    {
        yield return new WaitForSeconds(duration);

        if (player != null)
        {
            player.SetAttackDamage(originalAttackDamage);
            player.SetBuffState(BuffEffectType.Wizard_Strong_Mind, false);
            Debug.Log("Wizard_Strong_Mind 버프 종료: 공격력 복원");
        }

        Destroy(gameObject);
    }
}