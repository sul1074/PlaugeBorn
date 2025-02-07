using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : MonoBehaviour, IEnemyState 
{
    // 해당 변수는 공격속도와 밀접한 연관을 가지고 있습니다.
    // 그러므로 적 개체별로 따로 할 수 있게 하든지,
    // 무슨 묘수가 필요할 듯

    private Coroutine IdleCoroutine;

    [SerializeField]
    private float WAIT_SECONDS = 1.0f; 
    public void EnterState(IEnemyAI enemy)
    {
        Debug.Log("Entering Idle State"); // 로그
        enemy.Animator.PlayAnimation(0);
        MonoBehaviour enemyMB = enemy as MonoBehaviour;

        if (enemyMB != null)
        {
            if (IdleCoroutine != null)
            {
                enemyMB.StopCoroutine(IdleCoroutine);
            } 
        }
        IdleCoroutine = enemyMB.StartCoroutine(WaitForNextState(enemy));
    }
    public void UpdateState(IEnemyAI enemy)
    {
    }
    public void ExitState(IEnemyAI enemy)
    {
        if (IdleCoroutine != null)
        {
            MonoBehaviour enemyMB = enemy as MonoBehaviour;
            if (enemyMB != null)
            {
                enemyMB.StopCoroutine(IdleCoroutine);
            }
            IdleCoroutine = null;
        }
    }

    private IEnumerator WaitForNextState(IEnemyAI enemy)
    {
        yield return new WaitForSeconds(WAIT_SECONDS);
        enemy.ChangeState(new MoveState()); 
    }
}
