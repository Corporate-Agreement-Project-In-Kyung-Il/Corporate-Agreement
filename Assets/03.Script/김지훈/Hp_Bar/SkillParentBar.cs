using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillParentBar : MonoBehaviour
{
    [Header("SkillBar 위치")]
    public Vector3 offset = new Vector3(0, -0.4f, 0); // 머리 위에 위치
    
    [Header("Skill Cool 감소 속도")]
    public float lerpSpeed = 5f;  // 보간 속도 (조절 가능)
    public Player target; // 따라갈 Player
    
    public SkillChildrenBar[] skillSliders;
    private float targetValue;     // 목표 체력 비율
    private bool isLerping = false;
    
    private float[] skillcoolTimer;
    private float[] skillTimer;
    
    [Header("Transform 위치, 0번 기본전사, 1번 기본 궁수, 2번 기본 마법사, 나머지는 후에 추후 구현")]
    public float[] targetPositions;  

    private void Start()
    {
        skillSliders = GetComponentsInChildren<SkillChildrenBar>();
        skillcoolTimer = target.MaxskillCoolTimer;
    }

    private void FixedUpdate()
    {
        switch (target.playerStat.characterName)
        {
            case character_name.기본_전사 :
                offset = Vector3.up * targetPositions[0]; 
                break;
            case character_name.디노 :
                break;
            case character_name.아이언 :
                break;
            case character_name.기본_궁수 :
                offset = Vector3.up * targetPositions[1]; 
                break;
            case character_name.쿠아 :
                break;
            case character_name.기본_마법사 :
                offset = Vector3.up * targetPositions[2]; 
                break;
            case character_name.사비나 :
                break;

        }
        
        FollwTarget();
    }
    
    private void Update()
    {
        ChangeSkillBar();
        if (!isLerping) return;
        
        

        if (target.gameObject.activeSelf.Equals(false))
        {
            Destroy(gameObject);
            return;
        }
    }

    private void ChangeSkillBar()
    {
        skillSliders[0].Slider.value = 1f-(target.CurrentCoolTimer[0]/target.MaxskillCoolTimer[0]);
        skillSliders[1].Slider.value =  1f-(target.CurrentCoolTimer[1]/target.MaxskillCoolTimer[1]);
        
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
