using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBossEnemyStats : MonoBehaviour, IEnemyStats
{
    [SerializeField] private float Hp = 100.0f;
    [SerializeField] private float Defence = 5.0f;
    public void TakeHit(float damage)
    {
        float finalDamage = damage * (100.0f - Defence) / 100.0f;
        SubHp(finalDamage);
        
        // 보스는 스턴 없이 진행
    }
    private void SubHp(float damage)
    {
        this.Hp -= damage;
    }
}
