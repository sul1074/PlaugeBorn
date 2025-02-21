using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 던전 데이터를 가지고 있음.(던전 내 방 목록, 복도 좌표, 바닥 좌표)
/// </summary>
public class DungeonData
{
    private Dictionary<Vector2Int, HashSet<Vector2Int>> roomsDictionary; // Key: 방의 중심 좌표, Value: 방의 바닥(타일) 좌표 집합
    private HashSet<Vector2Int> floorPositions;
    private HashSet<Vector2Int> corridorPositions;

    // 읽기 전용 프로퍼티
    public IReadOnlyDictionary<Vector2Int, HashSet<Vector2Int>> RoomsDictionary => roomsDictionary;
    public IReadOnlyCollection<Vector2Int> FloorPositions => floorPositions;
    public IReadOnlyCollection<Vector2Int> CorridorPositions => corridorPositions;

    public void RemoveRoom(Vector2Int roomCenter)
    {
        roomsDictionary.Remove(roomCenter);
    }

    public DungeonData(Dictionary<Vector2Int, HashSet<Vector2Int>> roomsDictionary, 
                       HashSet<Vector2Int> floorPositions, 
                       HashSet<Vector2Int> corridorPositions)
    {
        this.roomsDictionary = new Dictionary<Vector2Int, HashSet<Vector2Int>>(roomsDictionary);
        this.floorPositions = new HashSet<Vector2Int>(floorPositions);
        this.corridorPositions = new HashSet<Vector2Int>(corridorPositions);
    }

    /// <summary>
    /// 복도를 제외한 해당 방의 좌표 반환
    /// </summary>
    public HashSet<Vector2Int> GetRoomFloorWithoutCorridors(Vector2Int dictionaryKey)
    {
        HashSet<Vector2Int> roomFloorNoCorridors = new HashSet<Vector2Int>(roomsDictionary[dictionaryKey]);

        roomFloorNoCorridors.ExceptWith(corridorPositions); // 복도 좌표를 제거

        return roomFloorNoCorridors;
    }
}
