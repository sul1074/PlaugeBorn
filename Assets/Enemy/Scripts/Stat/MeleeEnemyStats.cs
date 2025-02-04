using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyStats : MonoBehaviour, IEnemyStats
{
    [SerializeField]
    private float Hp = 100.0f;
    // 공격속도: IdleState에서 WAIT_SECONDS 변수 서정
    // 이동속도: MeleeMovement에서 수정
    [SerializeField]
    private float Defence = 1.0f;
    
    public void TakeHit(float damage)
    {
        // 데미지 계산식
        float finalDamage = damage * (100.0f - Defence) / 100.0f;
        SubHp(finalDamage);

        // 이제 여기서 상태전환을 실행시켜줘야 함.
        GetComponent<MeleeEnemyAI>().ChangeState(new StunState());

        // 우선 스턴을 1초 고정, StunState에서 계속
    }

    private void SubHp(float damage)
    {
        this.Hp -= damage;
    }
}
