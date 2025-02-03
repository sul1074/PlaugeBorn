using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyMovement : MonoBehaviour, IEnemyMovement
{
    public float moveSpeed = 5f;
    private Transform player;

    // knockback 관련 변수들
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.5f;

    public float MoveSpeed
    {
        get { return moveSpeed; }
        set
        {
            if (value < 0f) value = 0f;
            if (value > 20f) value = 20f;
            moveSpeed = value;
        }
    }

    /// <summary>
    /// 적을 플레이어 방향으로 움직이게 하는 함수입니다.
    /// </summary>
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
    // Start is called before the first frame update
    void Start()
    {
        // 태그로 플레이어 찾기
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
}
