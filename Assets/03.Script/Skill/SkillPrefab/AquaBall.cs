using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AquaBall : ActiveSkillBase, ISkillID
{
    //광역기 투사체, 적 추적후 터짐
    public int SkillId;
    public int SkillID { get; set; }
    public GameObject boomEffect;

    private List<Collider2D> targetList;
    private bool isBoom=false;
    [Header("최대 속도")] public float maxSpeed = 5f;
    [Header("유도 설정")]
    [SerializeField] private float initialSpeed = 2f;

    [Header("생명주기")]
    [SerializeField] private float lifeTime = 7f;

    [Header("타켓에 다가가는 속도")]
    [SerializeField] private float timeSinceStart = 2f;
    private bool isRotate = true;

    [SerializeField] Collider2D lastTarget;


    public void SetSkillID()
    {
        SkillID = SkillId;
    }

    public float moveSpeed;
    private BoxCollider2D coll;

    private void Awake()
    {
        boomEffect.SetActive(false);
        Initialize();
    }

    void Start()
    {
        Debug.Log("start aquaball");
        targetList = AliveExistSystem.Instance.monsterList;
        coll.enabled = false;
        SFXManager.Instance.Play(skillSound);
    }


    void Update()
    {
        if (isBoom)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        if (targetList.Count <= 0)
        {
            GameObject tempTargetObj = GameObject.Find("TempTarget");
            if (tempTargetObj.TryGetComponent(out Collider2D collider))
            {
                owner.target = collider;
            }
            MoveToEnemyHurt();
            return;
        }

        if (owner.target == null)
        {
            FindNextTarget();
            return;
        }

        MoveToEnemyHurt();
       
    }

    private void FindNextTarget()
    {
        if (shakePossible.Equals(false))
            return;
        //현재 위치를 기준으로 제일 가까운 애를 공격하게 
        targetList = AliveExistSystem.Instance.monsterList;



        float minDistance = 100f;
        Collider2D closestTarget = null;

        for (int i = 0; i < targetList.Count; i++)
        {
            if (targetList[i] != null && targetList[i].gameObject.activeSelf.Equals(false))
                continue;

            float dist = Vector2.Distance(transform.position, targetList[i].transform.position);

            if (dist < minDistance)
            {
                minDistance = dist;
                closestTarget = targetList[i];
            }
        }

        if (closestTarget != null)
        {
            owner.target = closestTarget;
        }
    }

    [Header("휘어지는 정도")] public float curvefloat = 3f;
    private float velocity;
    public float straight;
    private bool shakePossible = true;
    private void MoveToEnemyHurt()
    {
        //시간에 따라서 빠르게 움직이게
        timeSinceStart += Time.deltaTime;

        //곡선형 움직임
        float t = timeSinceStart;
        float curveSpeed = Mathf.Min(Mathf.Pow(t, curvefloat), maxSpeed);

        //target과의 거리에 따라서 곡선형 움직임에서 직선형으로
        float distanceToTarget = Vector3.Distance(transform.position, owner.target.transform.position);
        float dynamicRotateSpeed = Mathf.Lerp(180f, 90f, distanceToTarget / 5f); // 가까울수록 빠르게 회전

        velocity = curveSpeed * initialSpeed * Time.deltaTime;
        if (distanceToTarget < straight || owner.target.transform.position.y < transform.position.y)
        {
            Vector3 toTarget = (owner.target.transform.position - transform.position).normalized;
            Vector3 newDirection = Vector3.Slerp(transform.up, toTarget, Time.deltaTime * 10f); // 전환 속도 조절

            //아직 터지기 전인데 목표물에 곡선형으로 접근하지 못한다면 직선형 움직임으로 
            if (shakePossible)
            {
                transform.up = newDirection;
                transform.position += transform.up * velocity;
            } //터졌다면 직선형으로 움직이게
            else
            {
                transform.up = toTarget;
                Vector3 nextPos = Vector2.MoveTowards(transform.position, owner.target.transform.position, velocity);
                transform.position = nextPos;
            }
            //직선 이동
            //Vector3 moveDir = (nextPos - transform.position).normalized;
            //Vector3 nextPos = Vector2.MoveTowards(transform.position, owner.target.transform.position, velocity);

            //if (moveDir != Vector3.zero)
            //{
            //    transform.up = moveDir;
            //}

            //transform.position = nextPos;
        }
        else //아직 터지기 전으로 목표와의 거리가 있어서 곡선형으로 완만하게 움직이게
        {
            Vector3 dirToTarget = (owner.target.transform.position - transform.position).normalized;

            float targetAngle = Mathf.Atan2(dirToTarget.y, dirToTarget.x) * Mathf.Rad2Deg - 90f;
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);

            float maxDelta = dynamicRotateSpeed * Time.deltaTime;

            if (isRotate)
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxDelta);

            transform.position += transform.up * (velocity);
        }

        if (distanceToTarget < 0.5f) //터지기 시작하는 거리
        {
            if (shakePossible)
            {
                transform.localRotation = Quaternion.identity;
                DamgeEvent.OnTriggerShake(); //순차적으로  활성화시키면 자동으로 카메라 떨림 됨
                coll.enabled = true;
                shakePossible = false;
            }
            isRotate = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Enemy")).Equals(false))
            return;

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        if (other.gameObject.TryGetComponent(out IDamageAble enemyDamage))
        {
            CombatEvent combatEvent = new CombatEvent();
            combatEvent.Receiver = enemyDamage;
            combatEvent.Sender = owner;
            combatEvent.Damage = stat.Damage;
            combatEvent.collider = other;

            CombatSystem.instance.AddCombatEvent(combatEvent);

            Debug.Log("마법폭발 공격!");

            StartCoroutine(effectDelay());
            //데미지입힘
        }
    }

    IEnumerator effectDelay()
    {
        isBoom = true;
        boomEffect.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    public override void Initialize()
    {
        coll = GetComponent<BoxCollider2D>();
        SetSkillID();

        if (owner.skills[0].SkillID == SkillID && owner.skills[0] is ActiveSkillSO skill)
        {
            stat.Damage = owner.Damage * skill.Skill_Damage;
            stat.Range_width = skill.Skill_Range_width;
            stat.Range_height = skill.Skill_Range_height;
            stat.SkillName = skill.Skill_Name;
            coll.size = new Vector2(stat.Range_width, stat.Range_height);
        }
        else if (owner.skills[1].SkillID == SkillID && owner.skills[1] is ActiveSkillSO skill2)
        {
            stat.Damage = owner.Damage * skill2.Skill_Damage;
            stat.Range_width = skill2.Skill_Range_width;
            stat.Range_height = skill2.Skill_Range_height;
            stat.SkillName = skill2.Skill_Name;
            coll.size = new Vector2(stat.Range_width, stat.Range_height);
        }
        boomEffect.transform.localScale = new Vector3(stat.Range_width, stat.Range_height, 1f);
    }

    private void ReturnToPool()
    {
        Destroy(gameObject);
    }

    private void OnEnable()
    {
        StageEvent.stageClearEvent += ReturnToPool;
    }

    private void OnDisable()
    {
        StageEvent.stageClearEvent -= ReturnToPool;
    }
}