using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : MonoBehaviour, IEnemyState 
{
    public void EnterState(IEnemyAI enemy)
    {
        Debug.Log("Entering Idle State"); // 로그
    }
    public void UpdateState(IEnemyAI enemy)
    {
        enemy.Animator.PlayAnimation(0);
        enemy.ChangeState(new MoveState());
    }
    public void ExitState(IEnemyAI enemy)
    {

    }
}
