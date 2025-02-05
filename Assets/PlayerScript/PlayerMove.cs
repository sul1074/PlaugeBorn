using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public TrailRenderer trail;
    private Stat stat;
    [SerializeField] private Vector2 inputVec;
    private Rigidbody2D rigid;
    private Animator animator;
    [SerializeField] private float dashSpeed; // 일반 대쉬 속도
    [SerializeField] private float dashDuration; // 일반 대쉬 지속 시간
    private bool isDashing; // 대쉬 여부

    private bool isLightningCharged = false; // 벽력일섬 충전 여부
    [SerializeField] private float lightningDashSpeed = 30f; // 번개 대쉬 속도
    [SerializeField] private float lightningDashDuration = 0.1f; // 번개 대쉬 지속 시간

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        stat = GetComponent<Stat>();
        trail = GetComponent<TrailRenderer>();
        trail.enabled = false;
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

        // 일반 대시 (Left Shift)
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && !isLightningCharged)
        {
            StartCoroutine(Dash(dashSpeed, dashDuration));
        }

        // 번개 대시 (Q 키)
        if (Input.GetKeyDown(KeyCode.Q) && isLightningCharged && !isDashing)
        {
            StartCoroutine(LightDash(lightningDashSpeed, lightningDashDuration));
            isLightningCharged = false; // 한 번 사용 후 초기화
        }
    }

    private void FixedUpdate() 
    {   
        if (!isDashing)
        {    
            Vector2 nextVec = inputVec.normalized * stat.playerSpeed * Time.fixedDeltaTime;
            rigid.MovePosition(rigid.position + nextVec);
        }
    }

    IEnumerator Dash(float speed, float duration) // 일반 대쉬 (회피)
    {
        isDashing = true;
        // animator.SetTrigger("Dash");

        Vector2 dashDirection = inputVec.normalized;
        if (dashDirection == Vector2.zero)
        {
            dashDirection = Vector2.left * Mathf.Sign(transform.localScale.x);
        }

        float startTime = Time.time;
        while (Time.time < startTime + duration)
        {
            rigid.MovePosition(rigid.position + dashDirection * speed * Time.fixedDeltaTime);
            yield return null;
        }

        isDashing = false;
    }

    IEnumerator LightDash(float speed, float duration) // 벽력일섬 함수 (데미지 관련 추가 필요)
    {
        isDashing = true;

        animator.speed = 0.1f;
        animator.SetBool("Attack", true);
        trail.enabled = true;  // 궤적 효과 켜기

        yield return new WaitForSeconds(1f); // 선딜...
        animator.SetFloat("AttackState", 0);
        Vector2 dashDirection = inputVec.normalized;
        if (dashDirection == Vector2.zero)
        {
            dashDirection = Vector2.left * Mathf.Sign(transform.localScale.x);
        }

        float startTime = Time.time;
        while (Time.time < startTime + duration)
        {
            rigid.MovePosition(rigid.position + dashDirection * speed * Time.fixedDeltaTime);
            yield return null;
        }

        isDashing = false;
        animator.SetBool("Attack", false);
        animator.speed = 1f;
        trail.enabled = false;  // 궤적 효과 켜기
    }

    void Die()
    {
        isDashing = false;
        animator.SetTrigger("Die");
        rigid.velocity = Vector2.zero; // 움직임 정지
        this.enabled = false; // 조작 비활성화
    }

    // 번개 충전 함수 (LightningStrike에서 호출)
    public void ChargeLightning()
    {
        isLightningCharged = true;
    }
}
