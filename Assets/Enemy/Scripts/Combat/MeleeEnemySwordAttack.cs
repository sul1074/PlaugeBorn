using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemySwordAttack : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // TODO: ATTACK 어택하는 신호 여기에 넣으시면 됩니다.
        }
    }
}
