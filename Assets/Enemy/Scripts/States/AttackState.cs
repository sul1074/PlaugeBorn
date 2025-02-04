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

        enemy.ChangeState(new IdleState());
    }
    public void ExitState(IEnemyAI enemy)
    {
    }
}
