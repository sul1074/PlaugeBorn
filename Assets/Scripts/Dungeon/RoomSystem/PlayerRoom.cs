using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Cinemachine;

public class PlayerRoom : RoomGenerator
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private List<ItemPlacementData> itemData;

    [SerializeField]
    private PrefabPlacer prefabPlacer;

    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;

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

        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        // 카메라 플레이어 따라다니게 설정
        if (virtualCamera != null)
        {
            virtualCamera.Follow = playerObject.transform;
        }

        placedObjects.Add(playerObject);

        return placedObjects;
    }
}