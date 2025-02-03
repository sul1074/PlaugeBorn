using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : IEnemyState
{
    public void EnterState(IEnemyAI enemy)
    {
        Debug.Log("Entering Stun State"); // 로그
    }
    public void UpdateState(IEnemyAI enemy)
    {
        enemy.Animator.PlayAnimation(3);
    }

    public void ExitState(IEnemyAI enemy)
    {
        enemy.KnockBack();
    }
}
