using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // ���� ��� �ð�
    [SerializeField] private float attackDelay;
    // ���̾ ��ġ
    [SerializeField] private Transform firePosition;
    // ���̾�� ������ ������ ���� �����̳�
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
        if (Input.GetKeyUp(KeyCode.Q) && delayTime > attackDelay && playerMovement.canAtk())
            Attack();

        delayTime += Time.deltaTime;
    }

    private void Attack()
    {
        _anim.SetTrigger("attack");
        delayTime = 0;

    }


}
