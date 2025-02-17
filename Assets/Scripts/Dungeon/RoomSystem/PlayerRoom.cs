using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerRoom : RoomGenerator
{
    public GameObject player;

    public List<ItemPlacementData> itemData;

    [SerializeField]
    private PrefabPlacer prefabPlacer;

    /// <summary>
    /// 방의 중앙을 기준으로, 플레이어와 아이템을 배치하고 배치된 오브젝트들을 리스트로 반환
    /// </summary>
    public override List<GameObject> ProcessRoom(Vector2Int roomCenter, HashSet<Vector2Int> roomFloor, HashSet<Vector2Int> roomFloorNoCorridors)
    {
        ItemPlacementHelper itemPlacementHelper = new ItemPlacementHelper(roomFloor, roomFloorNoCorridors);

        // 아이템을 배치하고, 배치된 아이템들을 리스트로 저장
        List<GameObject> placedObjects = prefabPlacer.PlaceAllItems(itemData, itemPlacementHelper);

        // 플레이어는 방 중앙에 스폰
        Vector2Int playerSpawnPoint = roomCenter;

        // 중심에 배치하기 위해 0.5 오프셋 더해줌
        GameObject playerObject = prefabPlacer.CreateObject(player, playerSpawnPoint + new Vector2(0.5f, 0.5f));

        placedObjects.Add(playerObject);

        return placedObjects;
    }
}

// 배치할 개수 랜덤하게 설정
public abstract class PlacementData
{
    [SerializeField]
    [Min(0)]
    private int minQuantity = 0;

    [SerializeField]
    [Min(0)]
    [Tooltip("Max is inclusive")]
    private int maxQuantity = 0;

    [SerializeField]
    private int quantity => UnityEngine.Random.Range(minQuantity, maxQuantity + 1);

    public int Quantity
    {
        get => quantity;
    }
}

// 아이템 배치 데이터
[SerializeField]
public class ItemPlacementData : PlacementData
{
    private ItemData itemData;

    public ItemData ItemData
    {
        get { return itemData; }
        set { itemData = value; }
    }
}

// 적 배치 데이터
[SerializeField]
public class EnemyPlacementData : PlacementData
{
    private GameObject enemyPrefab;
    private Vector2Int enemySize = Vector2Int.one;

    public GameObject EnemyPrefab
    {
        get { return enemyPrefab; }
        set { enemyPrefab = value; }
    }

    public Vector2Int EnemySize
    {
        get { return enemySize; }
        set { enemySize = value; }
    }
}