using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyMovement
{ 
    /// <summary>
    /// 적 개체의 기본적인 움직임을 구현한 인터페이스 입니다.
    /// </summary>
    float MoveSpeed { get; set; }
    void Move();
}
