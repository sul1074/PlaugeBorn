using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IEnemyState
{
    public void EnterState(IEnemyAI enemy)
    {
        Debug.Log("Entering Attack State"); // 로그
    }
    public void UpdateState(IEnemyAI enemy)
    {
        enemy.Animator.PlayAnimation(4);

        if (!enemy.IsPlayerInAttackRange())
        {
            enemy.ChangeState(new MoveState());
        }
        // Test Code
        enemy.ChangeState(new StunState());
    }
    public void ExitState(IEnemyAI enemy)
    {
    }
}
