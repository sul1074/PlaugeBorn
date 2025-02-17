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

        // sprite¿¡ offset ¼³Á¤
        spriteRenderer.transform.localPosition = new Vector2(0.5f * itemData.Size.x, 0.5f * itemData.Size.y);
        itemCollider.size = itemData.Size;
        itemCollider.offset = spriteRenderer.transform.localPosition;

        if (itemData.NonDestructible) nonDestructible = true;

        this.health = itemData.Health;
    }

    public void GetHit(int damage, GameObject damageDealer)
    {
        if (nonDestructible) return;

        if (health > 1)
            Instantiate(hitFeedback, spriteRenderer.transform.position, Quaternion.identity);
        else
            Instantiate(destroyFeedback, spriteRenderer.transform.position, Quaternion.identity);
        
        spriteRenderer.transform.DOShakePosition(0.2f, 0.3f, 75, 1, false, true).OnComplete(ReduceHealth);
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
