using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 정적 클래스로 선언
public static class ProceduralGenerationAlgo
{
    /*
     * HashSet은 C++의 unordered_set과 동일한 기능. 따라서중복된 경로 허용 X
     * startPosition으로부터 walkLengh만큼 랜덤한 길이를 walk
     * walkLength 값만큼 진행한 경로의 집합을 HashSet<Vector2Int> 형태로 반환
     */
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

