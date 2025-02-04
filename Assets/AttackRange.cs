using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    private Vector2 mousePos;
    private float angle;
   void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 마우스 좌표 얻어옴
        angle = Mathf.Atan2(transform.position.y - mousePos.y, transform.position.x - mousePos.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }

}
