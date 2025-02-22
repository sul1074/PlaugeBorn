using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템 데이터를 가지고 있는 스크립터블 오브젝트
/// </summary>
[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    [SerializeField] private GameObject dropItem;
    [SerializeField] private Sprite sprite;
    [SerializeField] private Vector2 size = new Vector2(1.0f, 1.0f);
    [SerializeField] private PlacementType placementType;
    [SerializeField] private bool addOffset;
    [SerializeField] private int health = 1;
    [SerializeField] private bool nonDestructible;

    // 프로퍼티 (Getter, Setter)
    public GameObject DropItem
    {
        get { return dropItem; }
    }

    public Sprite Sprite
    {
        get => sprite;
        set => sprite = value;
    }

    public Vector2 Size
    {
        get => size;
        set => size = value;
    }

    public PlacementType PlacementType
    {
        get => placementType;
        set => placementType = value;
    }

    public bool AddOffset
    {
        get => addOffset;
        set => addOffset = value;
    }

    public int Health
    {
        get => health;
        set => health = Mathf.Max(0, value); // 최소 0 이상 유지
    }

    public bool NonDestructible
    {
        get => nonDestructible;
        set => nonDestructible = value;
    }
}
