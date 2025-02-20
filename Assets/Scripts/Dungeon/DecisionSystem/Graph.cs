using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Graph
{
    private static List<Vector2Int> neighbourse4directions = new List<Vector2Int>
    {
        new Vector2Int(0,1), // UP
        new Vector2Int(1,0), // RIGHT
        new Vector2Int(0,-1), // DOWN
        new Vector2Int(-1,0) // LEFT
    };

    private static List<Vector2Int> neighbourse8directions = new List<Vector2Int>
    {
        new Vector2Int(0,1), // UP
        new Vector2Int(1,0), // RIGHT
        new Vector2Int(0,-1), // DOWN
        new Vector2Int(-1,0), // LEFT
        new Vector2Int(1,1), // Diagonal
        new Vector2Int(1,-1), // Diagonal
        new Vector2Int(-1,1), // Diagonal
        new Vector2Int(-1,-1) // Diagonal
    };

    List<Vector2Int> graph;

    /// <summary>
    /// 생성자. 정점들을 받아서 그래프 생성.
    /// </summary>
    public Graph(IEnumerable<Vector2Int> vertices)
    {
        graph = new List<Vector2Int>(vertices);
    }
    /// <summary>
    /// 4방향(직교) 인접 정점들 반환
    /// </summary>
    public List<Vector2Int> GetNeighbours4Directions(Vector2Int startPosition)
    {
        return GetNeighbours(startPosition, neighbourse4directions);
    }

    /// <summary>
    /// 8방향(직교 + 대각) 인접 정점들 반환
    /// </summary>
    public List<Vector2Int> GetNeighbours8Directions(Vector2Int startPosition)
    {
        return GetNeighbours(startPosition, neighbourse8directions);
    }

    /// <summary>
    /// 전달받은 인접 방향대로 순회해서 이웃 정점을 찾아서 반환
    /// </summary>
    private List<Vector2Int> GetNeighbours(Vector2Int startPosition, List<Vector2Int> neighboursOffsetList)
    {
        List<Vector2Int> neighbours = new List<Vector2Int>();

        foreach (var neighboiurDirection in neighboursOffsetList)
        {
            Vector2Int potentialNeoghbour = startPosition + neighboiurDirection;
            
            if (graph.Contains(potentialNeoghbour))
                neighbours.Add(potentialNeoghbour);
        }

        return neighbours;
    }
}