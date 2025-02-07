using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyAI
{
    // 상태 변화를 막게하는 변수
    bool IsBlocking { get; set; }
    void ChangeState(IEnemyState state);
    public bool IsPlayerInAttackRange();
    public void Move();
    public void AttackPlayer();
    public void KnockBack();

    SPUM_Prefabs Animator { get; }
}
