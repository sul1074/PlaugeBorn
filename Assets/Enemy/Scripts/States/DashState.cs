using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : IEnemyState
{
    private float DASH_DURATION = 1.5f;
    private float dashingPower = 20.0f;
    private Rigidbody2D rb;
    private TrailRenderer tr;
    private Coroutine dashCoroutine;

    public void EnterState(IEnemyAI enemy)
    {
        enemy.IsBlocking = true;
        Debug.Log("Entering Dash State");
        enemy.Animator.PlayAnimation(7);

        MonoBehaviour enemyMB = enemy as MonoBehaviour;
        if (enemyMB != null)
        {
            dashCoroutine = enemyMB.StartCoroutine(PerformDash(enemy));
        }
    }

    public void ExitState(IEnemyAI enemy)
    {
    }

    public void UpdateState(IEnemyAI enemy)
    {
    }

    private IEnumerator PerformDash(IEnemyAI enemy)
    {
        yield return new WaitForSeconds(DASH_DURATION);
        MonoBehaviour enemyMB = enemy as MonoBehaviour;
        if (enemyMB != null)
        {
            // 찾은 다음에
            rb = enemyMB.GetComponent<Rigidbody2D>();
            tr = enemyMB.GetComponent<TrailRenderer>();
            // 바로 조지기 
            rb.velocity = new Vector2(-enemyMB.transform.localScale.x * dashingPower, 0f);
            tr.emitting = true;
            yield return new WaitForSeconds(DASH_DURATION / 9);
            tr.emitting = false;
            rb.velocity = new Vector2(0,0);
        }
        enemy.IsBlocking = false;
        enemy.ChangeState(new IdleState());
    }
}
