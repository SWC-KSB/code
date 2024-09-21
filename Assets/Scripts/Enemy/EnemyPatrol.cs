using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] public Transform player;  // 플레이어를 추적하기 위한 변수
    [SerializeField] private Transform enemy;
    [SerializeField] public float speed;
    [SerializeField] public float idleDuration;
    [SerializeField] public float followRange = 5f;  // 플레이어를 추적하는 범위
    [SerializeField] public Animator anim;

    private Vector3 iniScale;
    private bool isFollowingPlayer = false;  // 플레이어를 추적 중인지 여부
    private float idleTimer;

    private void Awake()
    {
        if (enemy == null)
        {
            Debug.LogError("Enemy reference is missing in the EnemyPatrol script.");
            return;
        }

        iniScale = enemy.localScale;
    }

    private void Update()
    {
        if (anim.GetBool("IsMoving") == false && idleTimer > 0)
        {
            idleTimer -= Time.deltaTime;
            return;
        }

        float distanceToPlayer = Vector3.Distance(enemy.position, player.position);

        if (distanceToPlayer <= followRange)
        {
            isFollowingPlayer = true;
            MoveTowardsPlayer();
        }
        else
        {
            if (isFollowingPlayer)
            {
                StartIdle();
            }
        }
    }

    private void StartIdle()
    {
        anim.SetBool("IsMoving", false);
        idleTimer = idleDuration;
        isFollowingPlayer = false;
    }

    private void MoveTowardsPlayer()
    {
        if (idleTimer <= 0)
        {
            Vector3 direction = (player.position - enemy.position).normalized;
            enemy.position += direction * speed * Time.deltaTime;

            if (enemy != null)
            {
                enemy.localScale = new Vector3(Mathf.Abs(iniScale.x) * (direction.x < 0 ? -1 : 1), iniScale.y, iniScale.z);
            }

            anim.SetBool("IsMoving", true);
        }
    }

    public void Initialize(Transform playerTarget, float spd, float idleDur, Animator animator, float followRange)
    {
        player = playerTarget;
        speed = spd;
        idleDuration = idleDur;
        anim = animator;
        this.followRange = followRange;  // 새로운 인수 처리
    }
}
