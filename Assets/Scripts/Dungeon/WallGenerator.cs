using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator
{
    public static void CreateWalls(HashSet<Vector2Int> floorPositions, TilemapVisualizer tilemapVisualizer)
    {
        HashSet<Vector2Int> basicWallPositions = FindWallsInDirections(floorPositions, Direction2D.cardinalDirectionsList); // 직교방향 벽 좌표 계산
        HashSet<Vector2Int> cornerWallPositions = FindWallsInDirections(floorPositions, Direction2D.diagonalDirectionsList); // 대각방향 벽 좌표 계산

        CreateBasicWall(tilemapVisualizer, basicWallPositions, floorPositions);
        CreateCornerWalls(tilemapVisualizer, cornerWallPositions, floorPositions);
    }

    private static void CreateCornerWalls(TilemapVisualizer tilemapVisualizer, HashSet<Vector2Int> cornerWallPositions, HashSet<Vector2Int> floorPositions)
    {
        foreach (Vector2Int pos in cornerWallPositions)
        {
            string neighbourBinaryType = "";

            // 상 방향을 시작으로 시계방향으로 8방향을 체크
            foreach (Vector2Int direcction in Direction2D.eightDirectionsList)
            {
                Vector2Int neighbourPos = pos + direcction;

                // 해당 주변 방향에 바닥이 있으면 1, 없으면 0
                if (floorPositions.Contains(neighbourPos))
                    neighbourBinaryType += "1";
                else
                    neighbourBinaryType += "0";
            }

            // 2진수 비트에 맞는 방향의 코너 벽을 생성
            tilemapVisualizer.PaintSingleCornerWall(pos, neighbourBinaryType);
        }
    }

    private static void CreateBasicWall(TilemapVisualizer tilemapVisualizer, HashSet<Vector2Int> basicWallPositions, HashSet<Vector2Int> floorPositions)
    {
        foreach (Vector2Int pos in basicWallPositions)
        {
            string neighboursBinaryType = "";
            
            // 상 -> 우 -> 하 -> 좌 순(시계방향)으로 체크
            foreach (Vector2Int direction in Direction2D.cardinalDirectionsList)
            {
                var neighbourPos = pos + direction;

                // 해당 주변 방향에 바닥이 있으면 1, 없으면 0
                if (floorPositions.Contains(neighbourPos)) 
                    neighboursBinaryType += "1";
                else
                    neighboursBinaryType += "0";
            }

            // 2진수 비트에 맞는 방향의 기본 벽을 생성
            tilemapVisualizer.PaintSingleWallTile(pos, neighboursBinaryType);
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
