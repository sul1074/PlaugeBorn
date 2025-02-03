using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class MeleeEnemyAI : MonoBehaviour, IEnemyAI
{
    private IEnemyState currentState;
    // 공격 범위
    public float AttackRange = 1f;
    public LayerMask playerLayer;

    [SerializeField]
    private SPUM_Prefabs animator;
    public SPUM_Prefabs Animator => animator;

    // Start is called before the first frame update
    private void Start()
    {
        ChangeState(new IdleState());
    }
    // Update is called once per frame
    private void Update()
    {
        currentState.UpdateState(this);
    }
/// <summary>
/// 상태를 바꾸는 함수입니다.
/// </summary>
/// <param name="newState"></param>
    public void ChangeState(IEnemyState newState)
    {
        currentState?.ExitState(this);

        currentState = newState;
        currentState.EnterState(this);
    }

    public bool IsPlayerInAttackRange()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, AttackRange, playerLayer);
        return hits.Length > 0;
    }

    public void Move()
    {
        transform.GetComponent<MeleeEnemyMovement>().Move();
    }

    public void AttackPlayer()
    {
        throw new System.NotImplementedException();
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }

    public void KnockBack()
    {
        transform.GetComponent<MeleeEnemyMovement>().KnockBack(Vector2.down);
    }
}
