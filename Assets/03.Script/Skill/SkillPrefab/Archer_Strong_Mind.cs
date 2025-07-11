using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_Strong_Mind : MonoBehaviour, ISkillID
{
    public int SkillId;
    public int SkillID { get; set; }
    public float duration = 2f;

    private Player_fusion player;

    public void SetSkillID()
    {
        SkillID = SkillId;
    }

    void Start()
    {
        Debug.Log("start ArcherStrongMind");

        player = FindObjectOfType<Player_fusion>();

        if (player != null)
        {
            if (player.HasBuff(BuffEffectType.Archer_Strong_Mind))
            {
                Debug.Log("이미 Archer_Strong_Mind 버프가 적용되어 있음. 중복 적용 안함.");
                Destroy(gameObject);
                return;
            }

            player.SetBuffState(BuffEffectType.Archer_Strong_Mind, true);
            StartCoroutine(RemoveBuffAfterDuration());
        }
        else
        {
            Debug.LogWarning("[ArcherStrongMind] Player_fusion 찾기 실패");
        }
    }

    private IEnumerator RemoveBuffAfterDuration()
    {
        yield return new WaitForSeconds(duration);

        if (player != null)
        {
            player.SetBuffState(BuffEffectType.Archer_Strong_Mind, false);
            Debug.Log("Archer_Strong_Mind 버프 종료");
        }

        Destroy(gameObject);
    }
}
