using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IEnemyState
{
    public void EnterState(IEnemyAI enemy)
    {
        Debug.Log("Entering Attack State"); // 로그
        enemy.Animator.PlayAnimation(4);
        enemy.AttackPlayer();
    }
    public void UpdateState(IEnemyAI enemy)
    {
        enemy.ChangeState(new IdleState());
    }
    public void ExitState(IEnemyAI enemy)
    {
    }
}
