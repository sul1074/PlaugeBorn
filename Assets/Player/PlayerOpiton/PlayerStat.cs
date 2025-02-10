using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    [SerializeField] public float playerATK; // 플레이어 공격력
    [SerializeField] public float playerSpeed; // 이동 속도
    [SerializeField] public float playerHealth; // 플레이어 체력
    [SerializeField] public float playerDefence; // 플레이어 방어력
// 데미지 = 공격력 * (1 - 방어율)
// 방어율 = 방어력 / (1 + 방어력)
}


    