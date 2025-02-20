using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using UnityEngine;
using Unity.VisualScripting;

public class RoomContentGenerator : MonoBehaviour
{
    // playerRoom과 defaultRoom을 RoomGenerator로 RoomGenerator로 캐스팅. boosRoom은 구현 예정
    [SerializeField]
    private RoomGenerator playerRoom, bossRoom, fightingPitRoom;  

    List<GameObject> spawnedObjects = new List<GameObject>();

    [SerializeField]
    private GraphTest graphTest;

    [SerializeField]
    private Transform itemParent;

    [SerializeField]
    private UnityEvent RegenerateDungeon;
    
    // Update is called once per frame
    void Update()
    {
        // 테스트 용
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (GameObject obj in spawnedObjects)
            {
                if (obj != null) Destroy(obj);
            }
            RegenerateDungeon?.Invoke();
        }
    }

    /// <summary>
    /// 모든 던전 방에 대해, 방 종류를 정하고 오브젝트를 배치
    /// </summary>
    public void GenerateRoomContent(DungeonData dungeonData)
    {
        // 생성하기 전에, 배치된 아이템들 제거
        foreach(GameObject item in spawnedObjects)
        {
            DestroyImmediate(item);
        }
        spawnedObjects.Clear();

        // 플레이어방을 제외한 나머지 방은, 적이 있는 방이 됨
        SelectPlayerSpawnPoint(dungeonData);
        SelectBossSpawnPoint(dungeonData);
        SelectEnemySpawnPoints(dungeonData);

        foreach (GameObject item in spawnedObjects)
        {
            // 배치된 아이템은 RoomContent 오브젝트의 자식으로
            if (item != null)
                item.transform.SetParent(itemParent, false);
        }
    }

    /// <summary>
    /// 플레이어가 처음 스폰될 방을 랜덤하게 정하고, 아이템을 배치. 그 방은 처리할 방에서 제거
    /// </summary>
    private void SelectPlayerSpawnPoint(DungeonData dungeonData)
    {
        int playerRoomIndex = UnityEngine.Random.Range(0, dungeonData.RoomsDictionary.Count);
        Vector2Int playerRoomCenter = dungeonData.RoomsDictionary.Keys.ElementAt(playerRoomIndex);
        Vector2Int playerRoomDictKey = playerRoomCenter;

        // 플레이어룸을 시작점으로, 다익스트라 알고리즘 수행하여 다른 방들로 가는 비용 계산
        graphTest.RunDijkstraAlgorithm(playerRoomCenter, dungeonData.FloorPositions);

        // 플레이어 룸에 아이템 배치
        List<GameObject> placedPrefabs = playerRoom.ProcessRoom(
            playerRoomCenter,
            dungeonData.RoomsDictionary.Values.ElementAt(playerRoomIndex),
            dungeonData.GetRoomFloorWithoutCorridors(playerRoomDictKey)
            );

        spawnedObjects.AddRange(placedPrefabs);
        dungeonData.RemoveRoom(playerRoomCenter);
    }


    /// <summary>
    /// 플레이어가 처음 스폰될 방을 랜덤하게 정하고, 아이템을 배치. 그 방은 처리할 방에서 제거.
    /// </summary>
    private void SelectBossSpawnPoint(DungeonData dungeonData)
    {
        // 다익스트라 알고리즘으로, 플레이어 방에서 가장 비용이 단일 타일 좌표
        Vector2Int highestDijkstraValueTile = graphTest.getHighestValueTile(); 
        Vector2Int bossRoomCenter = Vector2Int.zero;

        // 방들을 순회하며, 가장 높은 비용의 타일을 가진 방을 찾음.
        foreach (var room in dungeonData.RoomsDictionary)
        {
            if (room.Value.Contains(highestDijkstraValueTile))
            {
                bossRoomCenter = room.Key;
                break;
            }
        }

        Vector2Int bossRoomDictKey = bossRoomCenter;

        // 보스 룸에 아이템 배치.
        List<GameObject> placedPrefabs = bossRoom.ProcessRoom(
            bossRoomCenter,
            dungeonData.RoomsDictionary[bossRoomCenter],
            dungeonData.GetRoomFloorWithoutCorridors(bossRoomDictKey)
            );

        spawnedObjects.AddRange(placedPrefabs);
        dungeonData.RemoveRoom(bossRoomCenter);
    }

    /// <summary>
    /// 적이 나올 방에 대해, 적과 아이템을 배치.
    /// </summary>
    private void SelectEnemySpawnPoints(DungeonData dungeonData)
    {
        foreach (KeyValuePair<Vector2Int, HashSet<Vector2Int>> roomData in dungeonData.RoomsDictionary) 
        {
            spawnedObjects.AddRange(fightingPitRoom.ProcessRoom(
                roomData.Key,
                roomData.Value,
                dungeonData.GetRoomFloorWithoutCorridors(roomData.Key))
            );
        }
    }
}
