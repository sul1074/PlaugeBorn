using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkillAttack : MonoBehaviour // 차징 공격 스크립트
{
    public float maxChargeTime = 2f; // 최대 차징 시간
    public float maxSkillRange = 1f; // 최대 공격 범위
    //skillDamage (Stat.cs)
    //stunDuration 

    public float chargeTime = 0f; // 현재 차징 시간
    public bool isCharging = false;
    public CircleCollider2D chargeCollider;
    public LayerMask enemyLayer;
    [SerializeField] private GameObject chargeEffect; // 원 스프라이트
    private SpriteRenderer chargeSprite;
    void Start()
    {
        chargeCollider = GetComponentInChildren<CircleCollider2D>();
        chargeCollider.enabled = false; // 처음엔 비활성화
        chargeSprite = chargeEffect.GetComponent<SpriteRenderer>();
        chargeEffect.SetActive(false);
    }

    public void StartCharging()
    {
        isCharging = true;
        chargeTime = 0f;
        chargeCollider.enabled = true;
        chargeEffect.SetActive(true);
    }

    public void StopCharging()
    {
        isCharging = false;
        chargeCollider.enabled = false;
        chargeEffect.SetActive(false);
    }

    void Update()
    {
        if (!isCharging) return;

        chargeTime += Time.deltaTime;
        chargeTime = Mathf.Min(chargeTime, maxChargeTime);

        // 범위 조정
        float scale = Mathf.Lerp(0, maxSkillRange, chargeTime / maxChargeTime);
        chargeCollider.radius = scale;
        chargeEffect.transform.localScale = new Vector3(scale, scale, 1f);
        // 차징 완료 시 공격 실행
        if (chargeTime >= maxChargeTime)
        {
            PerformAttack();
            StopCharging();
        }
    }


    void PerformAttack()
    {

        // 데미지 및 스턴효과 적용
    }


}
