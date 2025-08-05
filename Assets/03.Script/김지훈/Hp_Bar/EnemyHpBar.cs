using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBar : MonoBehaviour, IObjectPoolItem
{

    [Header("HpBar 위치")]
    public Vector3 offset = new Vector3(0, 0.4f, 0); // 머리 위에 위치
    
    [Header("Hp 감소 속도")]
    public float lerpSpeed = 5f;  // 보간 속도 (조절 가능)
    public MonsterController target; // 따라갈 Enemy
    
    public string Key { get; set; }
    public GameObject GameObject => gameObject;
    
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
        if (target == null || target.gameObject.activeSelf == false)
        {
            ReturnToPool();
            return;
        }
        
        transform.position = target.transform.position + offset;
    }

    public void ReturnToPool()
    {
        target = null;
        hpSlider.value = 1;
        gameObject.SetActive(false);
        ObjectPoolSystem.Instance.ReturnToPool(this);
    }

    public void SliderDown(MonsterController damagedMonster)
    {
        if (target.Equals(damagedMonster))
        {
            targetValue = damagedMonster.CurrentHp / damagedMonster.MaxHp;
            isLerping = true;
        }
    }
    
    private void OnEnable()
    {
        DamgeEvent.enemydamageEvent += SliderDown;
    }

    private void OnDisable()
    {
        DamgeEvent.enemydamageEvent -= SliderDown;
    }
}
