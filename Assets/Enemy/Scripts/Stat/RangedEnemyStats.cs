using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyStats : MonoBehaviour, IEnemyStats
{
    [SerializeField]
    private float Hp = 100.0f;
    [SerializeField]
    private float Defence = 0.5f;

    public void TakeHit(float damage)
    {
        float finalDamage = damage * (100.0f - Defence) / 100.0f;
        SubHp(finalDamage);

        GetComponent<RangedEnemyAI>().ChangeState(new StunState());
    }

    private void SubHp(float damage)
    {
        this.Hp -= damage;
    }
}
