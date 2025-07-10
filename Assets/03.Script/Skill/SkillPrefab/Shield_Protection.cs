using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield_Protection : MonoBehaviour, ISkillID
{
    public int SkillId;
    public int SkillID { get; set; }

    public float blockChance = 0.3f; // 30%
    public float duration = 5f;

    private Player_jin player;

    public void SetSkillID()
    {
        SkillID = SkillId;
    }

    void Start()
    {
        Debug.Log("start Shield_Protection");

        player = FindObjectOfType<Player_jin>();

        if (player != null)
        {
            if (player.HasBuff(BuffEffectType.Shield_Protection))
            {
                Debug.Log("이미 Shield_Protection 버프 적용 중");
                Destroy(gameObject);
                return;
            }

            player.SetBuffState(BuffEffectType.Shield_Protection, true);
            player.SetShieldBlockChance(blockChance);

            StartCoroutine(RemoveBuffAfterDuration());
        }
        else
        {
            Debug.LogWarning("Player_jin 찾기 실패");
        }
    }

    private IEnumerator RemoveBuffAfterDuration()
    {
        yield return new WaitForSeconds(duration);

        if (player != null)
        {
            player.SetShieldBlockChance(0f);
            player.SetBuffState(BuffEffectType.Shield_Protection, false);
            Debug.Log("Shield_Protection 버프 종료");
        }

        Destroy(gameObject);
    }
}

