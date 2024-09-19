using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{
    // 공격 대기 시간
    [SerializeField] private float attackDelay;
    // 파이어볼 위치
    [SerializeField] private Transform firePosition;
    //파이어볼을 여러개 가지고 있을 컨테이너
    [SerializeField] private GameObject[] fireballs;

    private Animator _anim;
    private PlayerMovement playerMovement;
    private float delayTime = Mathf.Infinity;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();

    }
    
    private void Update()
    {
        if (Input.GetMouseButton(0) && delayTime > attackDelay && playerMovement.canAtk())
            Attack();
        
        delayTime += Time.deltaTime;
    }
    private void Attack()
    {
        _anim.SetTrigger("attack");
        delayTime = 0;

    }

    private int FindFireball()
    {
        for(int i = 0; i < fireballs.Length; i++)
        {
            if(!fireballs[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
}

