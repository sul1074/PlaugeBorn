using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyStats : MonoBehaviour, IEnemyStats
{
    [SerializeField] private float Hp = 100.0f;
    // Attack Speed: It be handled in WAIT_SECONDS of IdleState
    // MoveSpeed: It be handled in MeleeMovement
    [SerializeField] private float Defence = 1.0f;
    
    public void TakeHit(float damage)
    {
        // Damage Calculation
        float finalDamage = damage * (100.0f - Defence) / 100.0f;
        SubHp(finalDamage);

        //// Change State to Stun State 
        //GetComponent<MeleeEnemyAI>().ChangeState(new StunState());

        // TODO: TEST CODE
        GetComponent<MeleeEnemyAI>().ChangeState(new DashState());
    }

    private void SubHp(float damage)
    {
        this.Hp -= damage;
    }
}
