using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class AttackRange : MonoBehaviour // 평타 범위 관련 코드
{
    private Vector2 mousePos;
    private float angle;
    private PolygonCollider2D attackCollider;
    public LayerMask enemyLayer;

    private Camera mainCamera;

    void Awake() 
    {
        attackCollider = GetComponent<PolygonCollider2D>();
        attackCollider.enabled = false;

        mainCamera = Camera.main;
    }
    

   void Update()
    {
        RotateAttackRange();

        // 좌클릭(평타)시에만 콜라이더 활성화
        if (Input.GetMouseButtonDown(0))
        {
           StartCoroutine(EnableColliderForSeconds(0.2f));
        }
    }

    private void RotateAttackRange()
    {
        // 마우스 위치 계산
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Mathf.Abs(mainCamera.transform.position.z);
        mousePos = mainCamera.ScreenToWorldPoint(mouseScreenPos);

        Vector2 attackPos = transform.position; // AttackRange의 월드 위치 (2D)
        Vector2 direction = (mousePos - attackPos).normalized;

        // 각도 계산 (반시계 방향으로 조정)
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 최상위 부모(실제 player)로 참조해서 보정
        UnityEngine.Transform playerTransform = transform.root;
        if (playerTransform != null)
        {
            float playerAngle = playerTransform.eulerAngles.z;
            // 오프셋 조정
            transform.rotation = Quaternion.Euler(0, 0, angle - playerAngle + 90);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, angle + 90); // 기본 오프셋
        }
    }

    IEnumerator EnableColliderForSeconds(float duration)
    {
        attackCollider.enabled = true;  // 콜라이더 활성화
        Debug.Log("콜라이더 켜짐");
        yield return new WaitForSeconds(duration); // 일정 시간 대기
        attackCollider.enabled = false; // 다시 비활성화
        Debug.Log("콜라이더 꺼짐");
    }
}