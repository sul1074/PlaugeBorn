using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour // 기본 평타 및 스킬
{
    private Stat stat;
    private Animator animator;
    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        stat = GetComponent<Stat>();
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

    void PlayerAttackMethod() // 데미지 추가 필요
    {   
        // 평타 모션
        animator.SetBool("Attack", true); 
        animator.SetFloat("AttackState", 0);
        
    }

    void PlayerSkillAttack() // 데미지 추가 필요
    {
        // 스킬 모션
        animator.SetBool("Attack", true);
        animator.SetFloat("AttackState", 1);
    }
}


