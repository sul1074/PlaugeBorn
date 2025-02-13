using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour // 움직임 스크립트
{
    private Stat stat;
    private LightningDash lightningDash;
    public Vector2 inputVec;
    private Rigidbody2D rigid;
    private Animator animator;
    [SerializeField] private float dashSpeed; // 일반 대쉬 속도
    [SerializeField] private float dashDuration; // 일반 대쉬 지속 시간
    public bool isDashing; // 대쉬 여부
    private AfterImage afterImage;
    private SwordSkillAttack swordSkillAttack;
    private float dashCoolTime = 1f;
    private float dashCoolTimer = 0f;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        stat = GetComponent<Stat>();
        afterImage = GetComponent<AfterImage>();
        swordSkillAttack = GetComponent<SwordSkillAttack>();
        lightningDash = GetComponent<LightningDash>();
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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            lightningDash.TryUseLightDash();
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

    
    public void Die()
    {
        isDashing = false;
        animator.SetTrigger("Die");
        rigid.velocity = Vector2.zero; // 움직임 정지
        this.enabled = false; // 조작 비활성화
    }

}
