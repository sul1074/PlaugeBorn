using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator
{
    public static void CreateWalls(HashSet<Vector2Int> floorPositions, TilemapVisualizer tilemapVisualizer)
    {
        HashSet<Vector2Int> basicWallPositions = FindWallsInDirections(floorPositions, Direction2D.cardinalDirectionsList);
        foreach(Vector2Int pos in basicWallPositions)
        {
            tilemapVisualizer.PaintSingleWallTile(pos);
        }
    }

    private static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPositions, List<Vector2Int> directionList)
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>(); // 바닥을 기준으로, 벽이 될 수 있는 가장자리 위치 집합

        // 모든 바닥 타일 위치에 대해 검사
        foreach(Vector2Int pos in floorPositions)
        {
            // 모든 바닥 타일 위치에 대해 상,하,좌,우 방향을 체크
            foreach(Vector2Int dir in directionList)
            {
                Vector2Int neighbor = pos + dir; // 바닥 타일 위치를 기준으로 바로 인접한 위치를 계산
                if (floorPositions.Contains(neighbor)) continue; // 계산한 위치가 바닥 타일 위치에 포함되면 바닥임. 즉, 벽을 세워야 할 경계선 위치가 아니므로 패스

                wallPositions.Add(neighbor);
            }
        }

        return wallPositions;
    }
}
