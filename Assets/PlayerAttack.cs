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
        if (Input.GetMouseButton(0)) // 좌클릭 시 공격
        {
            PlayerAttackMethod();
        }
        else if (Input.GetMouseButton(1)) // 우클릭 시 스킬
        {
            PlayerSkillAttack();
        }
        else
        {
            animator.SetBool("Attack", false);
        }
    }

    void PlayerAttackMethod()
    {   
        // 평타 모션
        animator.SetBool("Attack", true); 
        animator.SetFloat("AttackState", 0);
    }

    void PlayerSkillAttack()
    {
        // 스킬 모션
        animator.SetBool("Attack", true);
        animator.SetFloat("AttackState", 1);
    }
}


