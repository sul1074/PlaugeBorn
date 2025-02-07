using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAttack : MonoBehaviour
{
    Transform playerPos;
    Vector3 bulletDirection;

    public float bulletSpeed = 10f;

    private void Start()
    {
        // 방향 설정 코드
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        bulletDirection = playerPos.position - transform.position;

        float angle = Mathf.Atan2(bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.right * bulletSpeed, ForceMode2D.Impulse);

        Destroy(gameObject, 5f);
    }
}
