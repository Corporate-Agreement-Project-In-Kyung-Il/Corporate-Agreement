using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpBar : MonoBehaviour
{
    [Header("HpBar 위치")]
    public Vector3 offset = new Vector3(0, -0.4f, 0); // 머리 위에 위치
    
    [Header("Hp 감소 속도")]
    public float lerpSpeed = 5f;  // 보간 속도 (조절 가능)
    public Player target; // 따라갈 Enemy
    
    private Slider hpSlider;
    private float targetValue;     // 목표 체력 비율
    private bool isLerping = false;

    private void Start()
    {
        TryGetComponent(out hpSlider);
    }

    private void LateUpdate()
    {
        FollwTarget();
    }

    private void Update()
    {
        if (!isLerping) return;

        hpSlider.value = Mathf.Lerp(hpSlider.value, targetValue, Time.deltaTime * lerpSpeed);

        // 거의 도달하면 보간 종료
        if (Mathf.Abs(hpSlider.value - targetValue) < 0.001f)
        {
            hpSlider.value = targetValue;
            isLerping = false;
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

    private void SliderDown(Player damagedPlayer)
    {
        if (target.Equals(damagedPlayer))
        {
            targetValue = damagedPlayer.playerStat.health / damagedPlayer.data.health;
            isLerping = true;
        }

    }
    
    
    private void OnEnable()
    {
        DamgeEvent.playerDamageEvent += SliderDown;
    }

    private void OnDisable()
    {
        DamgeEvent.playerDamageEvent -= SliderDown;
    }
}
