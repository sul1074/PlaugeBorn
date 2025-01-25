using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    Rigidbody2D rigid;
    public float playerSpeed;
    private Animator animator;
    public float dashSpeed; // 대쉬 속도
    public float dashDuration; // 대쉬 지속 시간
    private bool isDashing; // 대쉬 여부

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {   
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
            Vector2 nextVec = inputVec.normalized * playerSpeed * Time.fixedDeltaTime;
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
            dashDirection = Vector2.right * Mathf.Sign(transform.localScale.x); // 일단 뒤로 가는 방향으로 설정해뒀어요
        }

        float startTime = Time.time;
        while (Time.time < startTime + dashDuration)
        {
            rigid.MovePosition(rigid.position + dashDirection * dashSpeed * Time.fixedDeltaTime);
            yield return null;
        }
        isDashing = false;
    }
}
