using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Item : MonoBehaviour
{
    [SerializeField]
    private GameObject dropItem;
    
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
        dropItem = itemData.DropItem;
        spriteRenderer.sprite = itemData.Sprite;

        // 스프라이트의 실제 크기 가져오기
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;

        // 스프라이트를 아이템 데이터 크기에 맞추기 위한 스케일 조정
        spriteRenderer.transform.localScale = new Vector2(itemData.Size.x / spriteSize.x, itemData.Size.y / spriteSize.y);
        // sprite의 위치 조정
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

        // 회전 흔들기 이펙트
        spriteRenderer.transform.DOShakeRotation(0.2f, new Vector3(0, 0, 20), 50, 90, true).OnComplete(ReduceHealth);
    }

    private void ReduceHealth()
    {
        health--;
        
        if (health <= 0)
        {
            spriteRenderer.transform.DOComplete();
            DropItem();
            Destroy(gameObject);
        }
    }

    private void DropItem()
    {
        if (dropItem == null) return;

       float riseHeight = 0.5f;    // 떠오르는 높이
       float riseDuration = 0.5f; // 떠오르는 시간
       float dropDuration = 0.5f; // 떨어지는 시간

        // 나중에 드롭 아이템별 확률 정해서 해야 함
        if ((int)Random.Range(0, 6) >= 0)
        {
            GameObject dropped = Instantiate(dropItem, spriteRenderer.transform.position, Quaternion.identity);
            Sequence dropSequence = DOTween.Sequence();

            // 드롭 애니메이션 적용
            dropSequence
                .Append(dropped.transform.DOMoveY((spriteRenderer.transform.position.y + spriteRenderer.size.y * 0.5f) + riseHeight, riseDuration)
                    .SetEase(Ease.OutSine)) // 상자에서 위로 부드럽게 떠오름
                .Append(dropped.transform.DOMoveY(spriteRenderer.transform.position.y, dropDuration)
                    .SetEase(Ease.OutQuad)); // 다시 상자 위치로 내려옴
        }
    }
}
