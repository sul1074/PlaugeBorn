using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBeAttacked : MonoBehaviour
{
    private Player player;
    private Stat stat;
    private MeleeEnemyStats meleeEnemyStats;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("EnemyAttack")) {
            // stat.playerHealth -= meleeEnemyStats.damage;
            return;
        }
        /*if (stat.playerHealth <= 0)
        {
            player.Die();
        }*/
    }

}