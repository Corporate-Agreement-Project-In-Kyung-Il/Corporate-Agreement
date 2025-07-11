using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteelShield : MonoBehaviour, ISkillID
{
    public int SkillId;
    public int SkillID { get; set; }
    public float damageReductionRate = 0.4f; // 40% 경감
    public float duration = 5f;

    private Player_fusion player;

    public void SetSkillID()
    {
        SkillID = SkillId;
    }

    void Start()
    {
        Debug.Log("start SteelShield");

        player = FindObjectOfType<Player_fusion>();

        if (player != null)
        {
            if (player.HasBuff(BuffEffectType.Steel_Shield))
            {
                Debug.Log("이미 Steel_Shield 버프 적용 중");
                Destroy(gameObject);
                return;
            }

            player.SetBuffState(BuffEffectType.Steel_Shield, true);
            player.SetDamageReductionRate(damageReductionRate);

            StartCoroutine(RemoveBuffAfterDuration());
        }
        else
        {
            Debug.LogWarning("[SteelShield] Player_fusion 찾기 실패");
        }
    }

    private IEnumerator RemoveBuffAfterDuration()
    {
        yield return new WaitForSeconds(duration);

        if (player != null)
        {
            player.SetDamageReductionRate(0f);
            player.SetBuffState(BuffEffectType.Steel_Shield, false);
            Debug.Log("Steel_Shield 버프 종료: 데미지 경감 해제");
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // 데미지 입힘
        }
    }
}

