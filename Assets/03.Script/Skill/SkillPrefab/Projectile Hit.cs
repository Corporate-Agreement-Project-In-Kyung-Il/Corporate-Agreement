using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHit : MonoBehaviour, ISkillID
{
    public int SkillId;
    public int SkillID { get; set; }
    public float duration = 3f; // 버프 지속 시간

    private Player_jin player;
    private float originalAttackSpeed;

    public void SetSkillID()
    {
        SkillID = SkillId;
    }

    void Start()
    {
        Debug.Log("start ProjectileHit");

        player = FindObjectOfType<Player_jin>();

        if (player != null)
        {
            if (player.HasBuff(BuffEffectType.Projectile_Hit))
            {
                Debug.Log("이미 Projectile_Hit 버프가 적용되어 있음. 중복 적용 안함.");
                Destroy(gameObject);
                return;
            }

            player.SetBuffState(BuffEffectType.Projectile_Hit, true);

            originalAttackSpeed = player.GetAttackSpeed();
            player.SetAttackSpeed(originalAttackSpeed * 2.0f);

            StartCoroutine(RemoveBuffAfterDuration());
        }
        else
        {
            Debug.LogWarning("[ProjectileHit] Player_jin 찾기 실패");
        }
    }

    private IEnumerator RemoveBuffAfterDuration()
    {
        yield return new WaitForSeconds(duration);

        if (player != null)
        {
            player.SetAttackSpeed(originalAttackSpeed);
            player.SetBuffState(BuffEffectType.Projectile_Hit, false);
            Debug.Log("Projectile_Hit 버프 종료: 공격 속도 복원");
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