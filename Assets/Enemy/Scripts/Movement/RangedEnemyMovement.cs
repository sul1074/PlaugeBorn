using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyMovement : MonoBehaviour, IEnemyMovement
{
    public float moveSpeed = 3f;
    private Transform player;

    // knockback 관련 변수들
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.5f;

    public float MoveSpeed
    {
        get
        {
            return moveSpeed;
        }
        set
        {
            // 하한선 상한선 설정
            if (value < 0f) value = 0f;
            if (value > 20f) value = 20f;
            moveSpeed = value;
        }
    }
    public void Move()
    {
        Vector3 scale = transform.localScale; 
        if (player.position.x > transform.position.x)
        {
            scale.x = -1.0f;
        }
        else
        {
            scale.x = 1.0f;
        }
        transform.localScale = scale;
        Vector3 direction = player.position - transform.position;
        direction.Normalize();

        transform.position += direction * moveSpeed * Time.deltaTime;
    }
    // TODO: 잘 안됨
    public void KnockBack(Vector2 direction)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        rb.AddForce(-direction * knockbackForce, ForceMode2D.Impulse);
    }
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;    
    }
}
