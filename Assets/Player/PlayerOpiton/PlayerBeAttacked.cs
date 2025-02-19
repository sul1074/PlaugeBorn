using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBeAttacked : MonoBehaviour
{
    private SpriteRenderer[] spriteRenderers;
    private Player player;
    private Stat stat;
    private CapsuleCollider2D playerbody;
    // 레이어 마스크 필요 시 추가
    private bool isDead = false;

    void Awake()
    {
        player = GetComponent<Player>();
        stat = GetComponent<Stat>();
        playerbody = GetComponent<CapsuleCollider2D>();
        playerbody.enabled = true;
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other) { // EnemyAttack 태그에 맞았을 때 피격되는 것으로 일단 구현
        if (other.CompareTag("EnemyAttack")) {
            if (isDead) return;
            stat.playerHealth -= 1;
            Debug.Log("피격, 체력 -1 감소");
            
            if (stat.playerHealth <= 0)
            {
                player.Die();
                isDead = true;
                return;
            }
            StartCoroutine(BlinkEffect(2, 0.05f));
        }
    }
/// <summary>
/// 피격 시 효과 함수
/// </summary>
/// <param name="blinkCount"></param>
/// <param name="blinkInterval"></param>
/// <returns>깜박임 횟수, 간격 설정</returns>
    private IEnumerator BlinkEffect(int blinkCount, float blinkInterval)
    {
        for (int i = 0; i < blinkCount; i++)
        {
            SetAlpha(0.3f);
            yield return new WaitForSeconds(blinkInterval);
            SetAlpha(1f);
            yield return new WaitForSeconds(blinkInterval);
        }
    }

// 피격 투명도 변화 함수
    private void SetAlpha(float alpha)
    {
        foreach (var sprite in spriteRenderers)
        {
            if (sprite.CompareTag("Attack")) continue;
            if (sprite != null) {
            Color color = sprite.color;
            color.a = alpha;
            sprite.color = color;
            }
        }
    }

}