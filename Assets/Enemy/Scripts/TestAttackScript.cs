using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAttackScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("응 충돌은 했네");
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Enemy")))
        {
            collision.GetComponent<IEnemyStats>().TakeHit(10.0f);
        }
    }
}
