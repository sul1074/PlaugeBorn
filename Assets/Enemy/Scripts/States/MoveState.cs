using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : IEnemyState
{
    public void EnterState(IEnemyAI enemy)
    {
        Debug.Log("Entering Move State"); // 로그
    }
    public void UpdateState(IEnemyAI enemy)
    {
        enemy.Animator.PlayAnimation(1);
        enemy.Move(); 

        if (enemy.IsPlayerInAttackRange())
        {
            enemy.ChangeState(new AttackState());
        }
    }
    public void ExitState(IEnemyAI enemy)
    {

    }
}
