using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerDamage : MonoBehaviour //데미지 = 공격력 × (100 / (100 + 적방어력))
{
    private Stat stat;
    private void Awake()
    {
        stat = GetComponent<Stat>();
    }
    // 데미지 계산 함수
    public float CalculateDamage(float enemyDefense)
    {
        float playerDamage = stat.playerATK * (100f / (100f + enemyDefense));
        return Mathf.Max(playerDamage, 1); // 최소 데미지 1 보장
    }

    // 스킬 데미지 계산
    public float CalculateSkillDamage(float enemyDefense)
    {
        float playerSkillDamage = stat.playerATK * 2 * (100f / (100f + enemyDefense));
        return Mathf.Max(playerSkillDamage, 1);
    }

    // 벽력일섬 데미지 (예시로 방무 효과 추가)
    public float CalculateLightningDamage(float enemyDefense)
    {
        float reduceDefense = enemyDefense * 0.8f;
        float lightningDamage = stat.playerATK * 5 * (100f / (100f + reduceDefense));
        return Mathf.Max(lightningDamage, 1);
    }
}
