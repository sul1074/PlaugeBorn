using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAttackScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Test Attack Log");
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Enemy")))
        {
            collision.GetComponent<IEnemyStats>().TakeHit(10.0f);
        }
    }
}
