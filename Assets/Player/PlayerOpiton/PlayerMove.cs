using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour // 움직임 스크립트 (벽력일섬 포함)
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
    private float lightningDashSpeed = 30f; // 번개 대쉬 속도
    private float lightningDashDuration = 0.2f; // 번개 대쉬 지속 시간
    private AfterImage afterImage;
    private SwordSkillAttack swordSkillAttack;
    public LightningRange lightningRange; // 벽력일섬 범위
   private float dashCoolTime = 1f;
    private float dashCoolTimer = 0f;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        stat = GetComponent<Stat>();
        trail = GetComponent<TrailRenderer>();
        trail.enabled = false;
        afterImage = GetComponent<AfterImage>();
        swordSkillAttack = GetComponent<SwordSkillAttack>();
        lightningRange = GetComponent<LightningRange>();
    }

    void Update()
    {   

        if (stat.playerHealth <= 0) {
            Die();  
            return;
        }

        if (Input.GetMouseButton(1)) 
        { 
            if (swordSkillAttack.cooldownTimer > 0) 
            {
                return;
            }
            inputVec = Vector2.zero;
            animator.SetFloat("RunState", 0f);
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
            if (dashCoolTimer > 0)
        {
            dashCoolTimer -= Time.deltaTime;
            if (dashCoolTimer < 0)
                dashCoolTimer = 0;
        }

        }

        // 일반 대시 (Left Shift)
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && dashCoolTimer <= 0)
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

    IEnumerator Dash(float speed, float duration) // 일반 대쉬 (회피), 기능 추가 예정
    {
        isDashing = true;
        afterImage.StartGhosting();

        Vector2 dashDirection = inputVec.normalized;
        if (dashDirection == Vector2.zero)
        {
            dashDirection = Vector2.left * Mathf.Sign(transform.localScale.x);
        }

    // 회피 기능 구현 (테스트 예정)
    /*Collider2D playerCollider = GetComponent<Collider2D>();

    // Enemy 태그를 가진 모든 적의 Collider2D와 충돌 무시
    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
    List<Collider2D> enemyColliders = new List<Collider2D>();

    foreach (GameObject enemy in enemies)
    {
        Collider2D enemyCollider = enemy.GetComponent<Collider2D>();
        if (enemyCollider != null)
        {
            Physics2D.IgnoreCollision(playerCollider, enemyCollider, true);
            enemyColliders.Add(enemyCollider);
        }
    }*/


        float startTime = Time.time;
        while (Time.time < startTime + duration)
        {
            rigid.MovePosition(rigid.position + dashDirection * speed * Time.fixedDeltaTime);
            yield return null;
        }

        afterImage.StopGhosting();
        isDashing = false;
        dashCoolTimer = dashCoolTime;
    }

    IEnumerator LightDash(float speed, float duration) // 벽력일섬 함수 (데미지 관련 추가 필요, 회피도 넣어야함 돌진하기 전에 시간 느려지는 것 구현하는 것도 괜찮을 거 같아요)
    {
        isDashing = true;

        animator.speed = 0.1f;
        animator.SetBool("Attack", true);
        trail.enabled = true;  // 궤적 효과 켜기

        if (lightningRange != null) // 공격 콜라이더 활성화
        {
            lightningRange.dashColliderObj.SetActive(true);
        }

        yield return new WaitForSeconds(1f); // 선딜

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

        // 공격 범위 비활성화
        if (lightningRange != null)
        {
            lightningRange.dashColliderObj.SetActive(false);
        }
    }

    public void Die()
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
