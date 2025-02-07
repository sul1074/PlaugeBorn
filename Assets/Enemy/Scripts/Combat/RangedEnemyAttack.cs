using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RangedEnemyAttack : MonoBehaviour, IEnemyAttack
{
    [SerializeField] private GameObject bullet;
    public void Attack()
    {
        Instantiate(bullet, transform.position, transform.rotation);
    }
}
