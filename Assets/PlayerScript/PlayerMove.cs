using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Stat stat;
    [SerializeField] private Vector2 inputVec;
    Rigidbody2D rigid;
    private Animator animator;
    [SerializeField] private float dashSpeed; // 대쉬 속도
    [SerializeField] private float dashDuration; // 대쉬 지속 시간
    private bool isDashing; // 대쉬 여부
    

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        stat = GetComponent<Stat>();
    }

    void Update()
    {   
        if (stat.playerHealth <= 0) {
            Die();  
            return;
        }

        if (!isDashing)
        {
            // 플레이어 이동 (WASD)
            inputVec.x = Input.GetAxisRaw("Horizontal");
            inputVec.y = Input.GetAxisRaw("Vertical");

        if (inputVec.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * -Mathf.Sign(inputVec.x);
            transform.localScale = scale;
        }

        // 달리기 모션
        bool isMoving = inputVec.magnitude > 0;
        animator.SetFloat("RunState", isMoving ? 0.5f : 0f);    
        }
        
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing) // 왼쪽 쉬프트 누를 시 대쉬(회피)
        {
            StartCoroutine(Dash());
        }
    }
    private void FixedUpdate() 
    {   
        if (!isDashing)
        {    
            Vector2 nextVec = inputVec.normalized * stat.playerSpeed * Time.fixedDeltaTime;
            // 위치 이동
            rigid.MovePosition(rigid.position + nextVec);
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        //animator.SetTrigger("Dash");

        Vector2 dashDirection = inputVec.normalized;
        if (dashDirection == Vector2.zero)
        {
            dashDirection = Vector2.left * Mathf.Sign(transform.localScale.x); 
        }

        float startTime = Time.time;
        while (Time.time < startTime + dashDuration)
        {
            rigid.MovePosition(rigid.position + dashDirection * dashSpeed * Time.fixedDeltaTime);
            yield return null;
        }
        isDashing = false;
    }

    void Die()
    {
        isDashing = false;
        animator.SetTrigger("Die");
        rigid.velocity = Vector2.zero; // 움직임 정지지
        this.enabled = false; // 조작 비활성화
    }
}
