using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Item : MonoBehaviour
{ 
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private BoxCollider2D itemCollider;

    [SerializeField]
    int health = 3;

    [SerializeField]
    bool nonDestructible;

    [SerializeField]
    private GameObject hitFeedback, destroyFeedback;

    public UnityEvent OnGetHit
    {
        get => throw new System.NotImplementedException();
        set => throw new System.NotImplementedException();
    }

    public void Init(ItemData itemData)
    {
        spriteRenderer.sprite = itemData.Sprite;

        // sprite에 offset 설정
        spriteRenderer.transform.localPosition = new Vector2(0.5f * itemData.Size.x, 0.5f * itemData.Size.y);
        itemCollider.size = itemData.Size;
        itemCollider.offset = new Vector2(spriteRenderer.transform.localPosition.x, itemData.Size.y);

        if (itemData.NonDestructible) nonDestructible = true;

        this.health = itemData.Health;
    }

    public void GetHit()
    {
        if (nonDestructible) return;

        // 타격 이펙트인데, 지금은 없으니깐
        //if (health > 1)
        //    Instantiate(hitFeedback, spriteRenderer.transform.position, Quaternion.identity);
        //else
        //    Instantiate(destroyFeedback, spriteRenderer.transform.position, Quaternion.identity);
        
        // 흔들거리는 이펙트를 주고 체력을 감소
        // spriteRenderer.transform.DOShakePosition(0.2f, 0.3f, 75, 1, false, true).OnComplete(ReduceHealth);

        // 위치 흔들기 대신 크기 흔들기 적용
        // spriteRenderer.transform.DOShakeScale(0.2f, 0.2f, 50, 90, true).OnComplete(ReduceHealth);

        // 위치 흔들기 대신 회전 흔들기 적용
        spriteRenderer.transform.DOShakeRotation(0.2f, new Vector3(0, 0, 20), 50, 90, true).OnComplete(ReduceHealth);
    }

    private void ReduceHealth()
    {
        health--;
        
        if (health <= 0)
        {
            spriteRenderer.transform.DOComplete();
            Destroy(gameObject);
        }
    }
}
