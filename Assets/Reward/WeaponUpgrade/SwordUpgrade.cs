using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordUpgrade : MonoBehaviour // 검사 무기 업그레이드
{
private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 무기 업그레이드 적용 
            
            Destroy(gameObject);
        }
    }
}
