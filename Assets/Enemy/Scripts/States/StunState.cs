using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : IEnemyState
{
    [SerializeField]
    private float WAIT_SECONDS = 2.0f;
    public void EnterState(IEnemyAI enemy)
    {
        Debug.Log("Entering Stun State"); // 로그
        enemy.Animator.PlayAnimation(3);
        MonoBehaviour enemyMB = enemy as MonoBehaviour;
        // 되는 지 안되는 지 모름
        // enemy.KnockBack();
        enemyMB.StartCoroutine(WaitForNextState(enemy));
    }
    public void UpdateState(IEnemyAI enemy)
    {
    }

    public void ExitState(IEnemyAI enemy)
    {
    }
    private IEnumerator WaitForNextState(IEnemyAI enemy)
    {
        yield return new WaitForSeconds(WAIT_SECONDS);
        enemy.ChangeState(new MoveState()); 
    }
}
