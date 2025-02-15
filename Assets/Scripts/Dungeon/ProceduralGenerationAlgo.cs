using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class ProceduralGenerationAlgo
{
    /// <summary>
    /// startPosition으로부터 walkLengh만큼 랜덤한 길이를 walk.
    /// walkLength 값만큼 진행한 경로의 집합을 HashSet<Vector2Int> 형태로 반환
    /// </summary>
    /// <param name="startPos"> random walk의 시작점</param>
    /// <param name="walkLength">한 번의 반복에서 진행할 walk 수</param>
    /// <returns></returns>
    public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startPos, int walkLength)
    {
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();
        path.Add(startPos);
        Vector2Int prevPos = startPos;

        for (int i = 0; i < walkLength; i++)
        {
            Vector2Int next = prevPos + Direction2D.GetRandomCardinalDirection(); // 다음으로 이동할 랜덤 위치

            path.Add(next);
            prevPos = next;
        }

        return path;
    }

    /// <summary>
    /// 지정된 시작 위치에서 시작하여 랜덤한 방향으로 일정 길이만큼 복도를 생성.
    /// 마지막으로 진행한 walk 좌표에서 다시 복도를 만들어서 자연스럽게 계속 이어지도록 함.
    /// </summary>
    /// <param name="startPos">복도를 생성할 시작점</param>
    /// <param name="corridorLength">한 번의 반복에서 생성할 복도의 길이</param>
    /// <returns></returns>
    public static List<Vector2Int> RandomWalkCorridor(Vector2Int startPos, int corridorLength)
    {
        List<Vector2Int> corridor = new List<Vector2Int>(); // 복도 경로를 저장하는 리스트
        corridor.Add(startPos);

        Vector2Int dir = Direction2D.GetRandomCardinalDirection(); // 복도가 생성될 방향
        Vector2Int curr = startPos;

        for (int i = 0; i < corridorLength; i++)
        {
            curr += dir; // 현재 위치에서 한 방향으로 나아감
            corridor.Add(curr);
        }

        return corridor;
    }

    /// <summary>
    /// 공간을 랜덤하게 나누고, 나눈 방들의 집합을 리스트로 반환
    /// </summary>
    /// <param name="spaceToSplit">나눌 공간</param>
    /// <param name="minWidth">더 이상 나눌 수 없는 최소 너비</param>
    /// <param name="minHeight">더 이상 나눌 수 없는 최소 높이</param>
    /// <returns></returns>
    public static List<BoundsInt> BinarySpacePartitioning(BoundsInt spaceToSplit, int minWidth, int minHeight)
    {
        Queue<BoundsInt> roomsQueue = new Queue<BoundsInt>();
        List<BoundsInt> roomsList = new List<BoundsInt>();

        roomsQueue.Enqueue(spaceToSplit);

        while (roomsQueue.Count > 0)
        {
            BoundsInt room = roomsQueue.Dequeue();

            if (room.size.x < minWidth || room.size.y < minHeight) continue; // 더 이상 나눌 수 없으면 패스
            
            // 구조 랜덤성을 위해 나누는 방향(수평, 수직)을 랜덤하게 정함
            if (Random.value < 0.5f) 
            {
                // 높이가 최소 조건 2배 이상일 때만 수평으로 나눌 수 있음.
                if (room.size.y >= minHeight * 2)
                {
                    SplitHorizontally(minHeight, roomsQueue, room);
                }
                // 너비가 최소 조건 2배 이상일 때만 수직으로 나눌 수 있음.
                else if (room.size.x >= minWidth * 2)
                {
                    SplitVertically(minWidth, roomsQueue, room);
                }
                // 더 이상 나눌 수 없으면, 최종 방 리스트에 추가
                else if (room.size.x >= minWidth && room.size.y >= minHeight)
                {
                    roomsList.Add(room);
                }
            }
            // 수직으로 먼저 나누고, 수평으로 나누도록 함
            else
            {
                // 너비가 최소 조건 2배 이상일 때만 수직으로 나눌 수 있음.
                if (room.size.x >= minWidth * 2)
                {
                    SplitVertically(minWidth, roomsQueue, room);
                }
                // 높이가 최소 조건 2배 이상일 때만 수평으로 나눌 수 있음.
                else if (room.size.y >= minHeight * 2)
                {
                    SplitHorizontally(minHeight, roomsQueue, room);
                }
                // 더 이상 나눌 수 없으면, 최종 방 리스트에 추가
                else if (room.size.x >= minWidth && room.size.y >= minHeight)
                {
                    roomsList.Add(room);
                }
            }
        }

        return roomsList;
    }

    private static void SplitVertically(int minWidth, Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        int xSplit = Random.Range(minWidth, room.size.x - minWidth); // room의 x좌표 분할지점을 랜덤하게 계산

        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(xSplit, room.size.y, room.size.z)); // min ~ xSplit까지 나눔
        BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x + xSplit, room.min.y, room.min.z),
            new Vector3Int(room.size.x - xSplit, room.size.y, room.size.z)); // xSplit ~ room의 x좌표 나머지 까지 나눔

        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
    }
     
    private static void SplitHorizontally(int minHeight, Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        int ySplit = Random.Range(minHeight, room.size.y - minHeight); // room의 y좌표 분할지점을 랜덤하게 계산

        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(room.size.x, ySplit, room.size.z)); // min ~ ySplit까지 나눔
        BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x, room.min.y + ySplit, room.min.z),
            new Vector3Int(room.size.x, room.size.y - ySplit, room.size.z)); // ySplit ~ room의 y좌표 나머지 까지 나눔

        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
    }
}

// 상,하,좌,우 중 랜덤한 (Cardinal Direction)직교 방향을 반환
public static class Direction2D 
{
    public static List<Vector2Int> cardinalDirectionsList = new List<Vector2Int>
    {
       new Vector2Int(0, 1), // Up
       new Vector2Int(1, 0), // Right
       new Vector2Int(0, -1), // Down
       new Vector2Int(-1, 0) // Left
    };

    public static List<Vector2Int> diagonalDirectionsList = new List<Vector2Int>
    {
       new Vector2Int(1, 1), // Up-Right
       new Vector2Int(1, -1), // Right-Down
       new Vector2Int(-1, -1), // Down-Left
       new Vector2Int(-1, 1) // Left-Up
    };

    // 시계방향으로
    public static List<Vector2Int> eightDirectionsList = new List<Vector2Int>
    {
        new Vector2Int(0, 1), // Up
        new Vector2Int(1, 1), // Up-Right
        new Vector2Int(1, 0), // Right
        new Vector2Int(1, -1), // Right-Down
        new Vector2Int(0, -1), // Down
        new Vector2Int(-1, -1), // Down-Left
        new Vector2Int(-1, 0), // Left
        new Vector2Int(-1, 1) // Left-Up
    };

    public static Vector2Int GetRandomCardinalDirection() { return cardinalDirectionsList[Random.Range(0, cardinalDirectionsList.Count)]; }
}

