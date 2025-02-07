using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 정적 클래스로 선언
public static class ProceduralGenerationAlgo
{
    /// <summary>
    /// HashSet은 C++의 unordered_set과 동일한 기능. 따라서중복된 경로 허용 X
    /// startPosition으로부터 walkLengh만큼 랜덤한 길이를 walk
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
    /// 마지막으로 진행한 walk 좌표 인덱스에 접근하기 위해 List 사용. HashSet은 인덱스 접근이 안됨.
    /// 마지막으로 진행한 walk 좌표에서 다시 복도를 만들어야 계속 이어지기 때문.
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

    public static Vector2Int GetRandomCardinalDirection() { return cardinalDirectionsList[Random.Range(0, cardinalDirectionsList.Count)]; }
}

