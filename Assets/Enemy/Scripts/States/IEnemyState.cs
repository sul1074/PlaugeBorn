using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyState
{
    void EnterState(IEnemyAI enemy);
    void UpdateState(IEnemyAI enemy);
    void ExitState(IEnemyAI enemy);
}
