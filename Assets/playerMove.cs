using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    Rigidbody2D rigid;
    public float playerSpeed;
    private Animator animator;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
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

    private void FixedUpdate() 
    {   
        Vector2 nextVec = inputVec.normalized * playerSpeed * Time.fixedDeltaTime;
        // 위치 이동
        rigid.MovePosition(rigid.position + nextVec);
    }
}
