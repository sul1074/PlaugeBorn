using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 던전 데이터를 가지고 있음.(던전 내 방 목록, 복도 좌표, 바닥 좌표)
/// </summary>
public class DungeonData
{
    public Dictionary<Vector2Int, HashSet<Vector2Int>> roomsDictionary;
    public HashSet<Vector2Int> floorPositions;
    public HashSet<Vector2Int> corridorPositions;

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
