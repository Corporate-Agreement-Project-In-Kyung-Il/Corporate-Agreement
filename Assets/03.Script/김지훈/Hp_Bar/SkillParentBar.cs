using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillParentBar : MonoBehaviour
{
    [Header("HpBar 위치")]
    public Vector3 offset = new Vector3(0, -0.4f, 0); // 머리 위에 위치
    
    [Header("Hp 감소 속도")]
    public float lerpSpeed = 5f;  // 보간 속도 (조절 가능)
    public Player target; // 따라갈 Enemy
    
    public SkillChildrenBar[] skillSliders;
    private float targetValue;     // 목표 체력 비율
    private bool isLerping = false;

    private void Start()
    {
        skillSliders = GetComponentsInChildren<SkillChildrenBar>();
    }

    private void FixedUpdate()
    {
        switch (target.playerStat.characterName)
        {
            case character_name.기본_전사 :
                offset = Vector3.up * -1.0f; 
                break;
            case character_name.디노 :
                break;
            case character_name.아이언 :
                break;
            case character_name.기본_궁수 :
                offset = Vector3.up * -1.1f; 
                break;
            case character_name.쿠아 :
                break;
            case character_name.기본_마법사 :
                offset = Vector3.up * -1.3f; 
                break;
            case character_name.사비나 :
                break;

        }
        
        FollwTarget();
    }
    
    private void Update()
    {
        if (!isLerping) return;

        if (target.gameObject.activeSelf.Equals(false))
        {
            Destroy(gameObject);
            return;
        }
    }
    
    private void FollwTarget()
    {
        if (target.gameObject.activeSelf == false)
        {
            Destroy(gameObject);
        }

        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = target.transform.position + offset;
    }
}
