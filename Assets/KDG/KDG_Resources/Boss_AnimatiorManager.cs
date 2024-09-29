using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_AnimatiorManager : MonoBehaviour


{

    private Animator animator;
    public Transform player;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

   
    void Update()
    {
        if (Vector3.Distance(transform.position, player.position) < 5f)
        {
            animator.SetTrigger("isChanging");
        }

    }

    public void Boss_Animation_isChanging()
    {
        animator.SetTrigger("isChanging");
    }
}
