using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using UnityEngine;
using Unity.VisualScripting;

public class RoomContentGenerator : MonoBehaviour
{
    // playerRoom과 defaultRoom을 RoomGenerator로 RoomGenerator로 캐스팅.
    [SerializeField]
    private RoomGenerator playerRoom, fightingPitRoom;  

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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (GameObject obj in spawnedObjects)
            {
                if (obj != null) Destroy(obj);
            }
            RegenerateDungeon?.Invoke();
        }
    }

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
        SelectEnemySpawnPoints(dungeonData);

        foreach (GameObject item in spawnedObjects)
        {
            if (item != null)
                item.transform.SetParent(itemParent, false);
        }
    }

    /// <summary>
    /// 플레이어가 처음 스폰될 방을 랜덤하게 정하고 그 방은 처리할 방에서 제거
    /// </summary>
    private void SelectPlayerSpawnPoint(DungeonData dungeonData)
    {
        int randomRoomIndex = UnityEngine.Random.Range(0, dungeonData.RoomsDictionary.Count);
        Vector2Int playerRoomCenter = dungeonData.RoomsDictionary.Keys.ElementAt(randomRoomIndex);
        Vector2Int playerRoomDictKey = playerRoomCenter;

        // 플레이어룸을 시작점으로, 다익스트라 알고리즘 수행하여 다른 방들로 가는 비용 계산
        graphTest.RunDijkstraAlgorithm(playerRoomCenter, dungeonData.FloorPositions);

        // 플레이어 룸에 아이템 배치.
        List<GameObject> placedPrefabs = playerRoom.ProcessRoom(
            playerRoomCenter,
            dungeonData.RoomsDictionary.Values.ElementAt(randomRoomIndex),
            dungeonData.GetRoomFloorWithoutCorridors(playerRoomDictKey)
            );

        spawnedObjects.AddRange(placedPrefabs);
        dungeonData.RemoveRoom(playerRoomCenter);
    }
    
    /// <summary>
    /// 처리할 방 리스트들에 대해 적을 채워넣음
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
