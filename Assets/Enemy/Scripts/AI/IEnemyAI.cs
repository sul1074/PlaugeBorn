using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyAI
{
    void ChangeState(IEnemyState state);
    public bool IsPlayerInAttackRange();
    public void Move();
    public void AttackPlayer();
    public void KnockBack();

    SPUM_Prefabs Animator { get; }
}
