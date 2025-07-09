using System.Collections;
using UnityEngine;

public class WizardStrongMind : MonoBehaviour, ISkillID
{
    public int SkillId;
    public int SkillID { get; set; }
    public float duration = 3f; // 지속 시간 (예: 3초)

    private float originalAttackDamage;
    private Player_jin player;

    public void SetSkillID()
    {
        SkillID = SkillId;
    }

    void Start()
    {
        Debug.Log("start WizardStrongMind");

        // 시전자(Player_jin) 찾기
        player = FindObjectOfType<Player_jin>();

        if (player != null)
        {
            // 원래 공격력 저장
            originalAttackDamage = player.Damage;

            // 공격력 2배 증가
            player.SetAttackDamage(originalAttackDamage * 2.0f);

            // 일정 시간 후 원상 복구
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
            Debug.Log("버프 종료: 기본 공격력 복원됨");
        }

        Destroy(gameObject); // 버프 오브젝트 제거
    }
}