using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class MagicBall : MonoBehaviour, IObjectPoolItem
{
    //애니메이션 Bool값
    private static readonly int Explosion = Animator.StringToHash("Explosion");
    
    //Component 구성요소
    private Animator animator;
    private Collider2D collider;
    private AnimatorStateInfo currentStateInfo;
    
    //Weapon 타입한테 받아오는 값
    public Transform target;
    public Player player;
    public bool isTargetNotDead = true;
    public float magicDamage;

    [Header("최대 속도")] public float maxSpeed = 5f;
    [Header("유도 설정")] 
    [SerializeField] private float initialSpeed = 2f;
    [SerializeField] private float explosionTriggerDistance = 0.15f;
    
    [Header("생명주기")]
    [SerializeField] private float lifeTime = 7f;
    
    [Header("타켓에 다가가는 속도")]
    [SerializeField] private float timeSinceStart = 2f;
    private bool isRotate = true;

    private Vector3 lastTarget = new Vector3(0, 80f, 0f);
    //IObjectPoolItem
    public string Key { get; set; }
    public GameObject GameObject => gameObject;
    private SpriteRenderer spriteRenderer;
    public void ReturnToPool()
    {
        spriteRenderer.sortingLayerName = "Skill";
        spriteRenderer.sortingOrder = 2;
        target = null;
        timeSinceStart = 2.1f;
        straight = 1.25f;
        striaghtMove = 180f;
        isRotate = true;
        transform.localRotation = Quaternion.Euler(0f, 0f, -37f);
        gameObject.SetActive(false);
        ObjectPoolSystem.Instance.ReturnToPool(this);
    }
    
    private void Start()
    {
        TryGetComponent(out animator);
        TryGetComponent(out collider);
        TryGetComponent(out spriteRenderer);
       // Destroy(gameObject, lifeTime);
       targetList = AliveExistSystem.Instance.monsterList;   
    }


    void Update()
    {
        currentStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        
        if (currentStateInfo.IsName("Explosion"))
        {
            spriteRenderer.sortingLayerName = "Skill";
            spriteRenderer.sortingOrder = 2;
            transform.rotation = Quaternion.identity;
            
            switch (currentStateInfo.normalizedTime)
            {
                case >= 0.8f:
                    ReturnToPool();
                    break;
                case >= 0.25f and < 0.5f:
                    collider.enabled = true;
                    break;
                default:
                    collider.enabled = false;
                    break;
            }

            return;
        }
        
        if (targetList.Count <= 0)
        {
            target.position = lastTarget;
            MoveToEnemyHurt();
            return;
        }
        
        if ( target == null || target.gameObject.activeSelf.Equals(false))
        {
            FindNextTarget();
            striaghtMove = 360f;
            straight = 1.4f;
        }
        else
        {
            MoveToEnemyHurt();
        }
    }

    private List<Collider2D> targetList;
    private void FindNextTarget()
    {
        
        //현재 위치를 기준으로 제일 가까운 애를 공격하게 
        targetList = AliveExistSystem.Instance.monsterList;


        
        float minDistance = 100f;
        Transform closestTarget = null;
        
        for (int i = 0; i < targetList.Count; i++)
        {
            if (targetList[i] != null && targetList[i].gameObject.activeSelf.Equals(false))
                continue;
            
            float dist = Vector2.Distance(transform.position, targetList[i].transform.position);

            if (dist < minDistance)
            {
                minDistance = dist;
                closestTarget = targetList[i].transform;
            }
        }

        if (closestTarget != null)
        {
            target = closestTarget;
            spriteRenderer.sortingLayerName = "Forward";
            spriteRenderer.sortingOrder = 5;
        }
    }

    private bool shakeTrue = true;

    [Header("휘어지는 정도")] public float curvefloat;
    public float straight;
    
    private float velocity;
    private float striaghtMove = 270f;
    private void MoveToEnemyHurt()
    {
        timeSinceStart += Time.deltaTime; 
        
        float t = timeSinceStart;
        float curveSpeed = Mathf.Min(Mathf.Pow(t, curvefloat), maxSpeed);
        
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        float dynamicRotateSpeed = Mathf.Lerp(striaghtMove, 110f, distanceToTarget / 5f); // 가까울수록 빠르게 회전

        velocity = curveSpeed * initialSpeed * Time.deltaTime;
        if (distanceToTarget < straight || target.position.y < transform.position.y)
        {
            Vector3 toTarget = (target.position - transform.position).normalized;
            Vector3 newDirection = Vector3.Slerp(transform.up, toTarget, Time.deltaTime * 10f); // 전환 속도 조절

            if (shakeTrue)
            {
                transform.up = newDirection;
                transform.position += transform.up * velocity;
            }
            else
            {
                transform.up = toTarget;
                Vector3 nextPos = Vector2.MoveTowards(transform.position, target.transform.position, velocity);
                transform.position = nextPos;
            }

            //직선이동 
            //Vector3 nextPos = Vector2.MoveTowards(transform.position, target.position, velocity);
            //Vector3 moveDir = (nextPos - transform.position).normalized;
            //
            //if (moveDir != Vector3.zero)
            //{
            //    transform.up = moveDir;
            //}
            //
            //transform.position = nextPos;
        }
        else
        {
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            float targetAngle = Mathf.Atan2(dirToTarget.y, dirToTarget.x) * Mathf.Rad2Deg - 90f;
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);

            float maxDelta = dynamicRotateSpeed * Time.deltaTime;

            if (isRotate)
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxDelta);

            transform.position += transform.up * (velocity);
        }
        
        if (distanceToTarget < 0.3f)
        {
            animator.SetTrigger(Explosion);
            
            if (shakeTrue)
            {
                DamgeEvent.OnTriggerShake();
                shakeTrue = false;
            }
            isRotate = false;
        }
    }
    //private void MoveToLastTarget()
    //{
    //    float distanceToTarget = Vector3.Distance(transform.position, lastTarget);
    //    float dynamicRotateSpeed = Mathf.Lerp(180f, 90f, distanceToTarget / 5f); // 가까울수록 빠르게 회전
    //    Vector3 dirToTarget = (lastTarget - transform.position).normalized;
    //    float targetAngle = Mathf.Atan2(dirToTarget.y, dirToTarget.x) * Mathf.Rad2Deg - 90f;
    //    Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
    //
    //    float maxDelta = dynamicRotateSpeed * Time.deltaTime;
    //
    //    if (isRotate)
    //        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxDelta);
    //
    //    transform.position += transform.up * (velocity);
    //}
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Enemy")).Equals(false))
            return;
        
        if(other.gameObject.TryGetComponent(out IDamageAble enemyDamage))
        {
            CombatEvent combatEvent = new CombatEvent();
            combatEvent.Receiver = enemyDamage;
            combatEvent.Sender = player;
            combatEvent.Damage = magicDamage;
            combatEvent.collider = other;
            
            CombatSystem.instance.AddCombatEvent(combatEvent);

            if (enemyDamage.CurrentHp <= 0 && other.transform.Equals(target))
            {
                isTargetNotDead = false;
                ReturnToPool();
                return;
            }
            isTargetNotDead = true;
        }
    }

    private void OnEnable()
    {
        StageEvent.stageClearEvent += ReturnToPool;
        shakeTrue = true;
    }

    private void OnDisable()
    {
        StageEvent.stageClearEvent -= ReturnToPool;
    }

}
