using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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

    [Header("유도 설정")] 
    [SerializeField] private float initialSpeed = 2f;
    [SerializeField] private float explosionTriggerDistance = 0.15f;
    
    [Header("생명주기")]
    [SerializeField] private float lifeTime = 7f;
    
    [Header("타켓에 다가가는 속도")]
    [SerializeField] private float timeSinceStart = 2f;
    private bool isRotate = true;
    
    //IObjectPoolItem
    public string Key { get; set; }
    public GameObject GameObject => gameObject;
    public void ReturnToPool()
    {
        target = null;
        timeSinceStart = 2.1f;
        isRotate = true;
        gameObject.SetActive(false);
        ObjectPoolSystem.Instance.ReturnToPool(this);
    }
    
    private void Start()
    {
        TryGetComponent(out animator);
        TryGetComponent(out collider);
       // Destroy(gameObject, lifeTime);
        
    }

    void Update()
    {
        if (target.gameObject.activeSelf.Equals(false))
        {
            FindNextTarget();
        }
        else
        {
            MoveToEnemyHurt();
        }
    }

    private void FindNextTarget()
    {
        //현재 위치를 기준으로 제일 가까운 애를 공격하게 
        List<Collider2D> targetList = AliveExistSystem.Instance.monsterList;

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
        }
    }
    private void MoveToEnemyHurt()
    {
        timeSinceStart += Time.deltaTime; 
        
        float t = timeSinceStart;
        float curveSpeed = Mathf.Pow(t, 2f);
        
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        float dynamicRotateSpeed = Mathf.Lerp(360f, 90f, distanceToTarget / 5f); // 가까울수록 빠르게 회전
        float straightDistance = 1f;
        
        currentStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        
        if (distanceToTarget <= straightDistance)
        {
            Vector3 nextPos = Vector2.MoveTowards(transform.position, target.position, curveSpeed * Time.deltaTime);
            Vector3 moveDir = (nextPos - transform.position).normalized;

            if (moveDir != Vector3.zero)
            {
                transform.up = moveDir;
            }

            transform.position = nextPos;
        }
        else
        {
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            float targetAngle = Mathf.Atan2(dirToTarget.y, dirToTarget.x) * Mathf.Rad2Deg - 90f;
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);

            float maxDelta = dynamicRotateSpeed * Time.deltaTime;

            if (isRotate)
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxDelta);

            transform.position += transform.up * (curveSpeed * Time.deltaTime);
        }
        
        if (distanceToTarget < 0.3f)
        {
            animator.SetTrigger(Explosion);
            FollowCamera.Shake();
            transform.rotation = Quaternion.identity;
            isRotate = false;
        }
        
        if (currentStateInfo.IsName("Explosion") && currentStateInfo.normalizedTime >= 0.25f &&
            currentStateInfo.normalizedTime < 0.5f)
        {
            collider.enabled = true;
        }
        else
        {
            collider.enabled = false;
        }

        if(currentStateInfo.IsName("Explosion") && currentStateInfo.normalizedTime >= 0.9f)
            ReturnToPool();
    }

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


}
