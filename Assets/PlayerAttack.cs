using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float playerDamage = 1f;
    private Animator animator;
    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            PlayerAttackMethod();
        }
        else
        {
            animator.SetBool("Attack", false);
        }
    }

    void PlayerAttackMethod()
    {
        animator.SetBool("Attack", true);
    }
}


