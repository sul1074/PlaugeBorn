using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomGenerator : RoomGenerator
{
    [SerializeField]
    private GameObject boss;

    [SerializeField]
    private List<ItemPlacementData> itemData;

    [SerializeField]
    private PrefabPlacer prefabPlacer;

    public override List<GameObject> ProcessRoom(Vector2Int roomCenter, HashSet<Vector2Int> roomFloor, HashSet<Vector2Int> roomFloorNoCorridors)
    {
        ItemPlacementHelper itemPlacementHelper = new ItemPlacementHelper(roomFloor, roomFloorNoCorridors);

        // 아이템을 배치하고, 배치된 아이템들을 리스트로 저장
        List<GameObject> placedObjects = prefabPlacer.PlaceAllItems(itemData, itemPlacementHelper);

        // 플레이어는 방 중앙에 스폰
        Vector2Int bossSpawnPoint = roomCenter;

        // 중심에 배치하기 위해 0.5 오프셋 더해줌
        GameObject bossObject = prefabPlacer.CreateObject(boss, bossSpawnPoint + new Vector2(0.5f, 0.5f));

        placedObjects.Add(bossObject);

        return placedObjects;
    }
}
