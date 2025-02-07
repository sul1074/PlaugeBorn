using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CorridorFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField] 
    private int corridorLength = 14, corridorCount = 5;

    [SerializeField]
    [Range(0.1f, 1)]
    private float dungeonPercent = 0.8f;

    protected override void RunProceduralGeneration()
    {
        CorridorFirstDungeonGeneration();
    }

    /// <summary>
    /// 던전을 연결해주는 복도를 먼저 만들고, 복도 끝점 좌표를 기반으로 던전을 만드는 함수
    /// </summary>
    private void CorridorFirstDungeonGeneration()
    {
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>(); // 복도와 던전을 생성할(타일을 배치할) 전체 좌표 집합
        HashSet<Vector2Int> dungeonStartPositions = new HashSet<Vector2Int>(); // 던전들이 처음 생성될 시작 좌표 집합(각 복도들의 생성 시작점)

        // 복도가 생성될 좌표 집합. floorPositions에 복도가 생성될 좌표 집합을 추가하고 potentialRoomPostions에는 던전이 생성될 시작 좌표 집합을 추가함
        List<List<Vector2Int>> corridorPositions = CreateCorridors(floorPositions, dungeonStartPositions);

        // 던전을 생성할 좌표 집합
        HashSet<Vector2Int> dungeonFloorPositions = CreateDungeons(dungeonStartPositions);

        // 막힌 외진 지점 찾기
        List<Vector2Int> isolatedEnds = FindAllDeadEnds(floorPositions);

        // 단절된 곳들에 대해 추가 던전을 생성하여 자연스럽게 확장
        CreateRoomsAtIsolatedEnds(isolatedEnds, dungeonFloorPositions);
        floorPositions.UnionWith(dungeonFloorPositions);

        // 복도의 크기를 증가시켜 좀 더 넓은 길을 형성
        for (int i = 0; i < corridorPositions.Count; i++)
        {
            //corridorPositions[i] = IncreaseCorridorSizeByOne(corridorPositions[i]);
            corridorPositions[i] = IncreaseCorridorBrush3by3(corridorPositions[i]);
            floorPositions.UnionWith(corridorPositions[i]);
        }

        // 타일 매핑 및 벽 생성
        tilemapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
    }

    /// <summary>
    /// 단절된 점들에 대해 추가로 던전을 생성하는 함수
    /// </summary>
    /// <param name="isolatedEnds">던전을 생성할 단절된 좌표 집합</param>
    /// <param name="dungeonFloorPositions">추가로 생성할 던전 좌표 집합이 추가될 패러미터</param>
    private void CreateRoomsAtIsolatedEnds(List<Vector2Int> isolatedEnds, HashSet<Vector2Int> dungeonFloorPositions)
    {
        foreach (Vector2Int pos in isolatedEnds)
        {
            if (dungeonFloorPositions.Contains(pos)) continue;

            HashSet<Vector2Int> dungeonFloor = RunRandomWalk(randomWalkParameters, pos);
            dungeonFloorPositions.UnionWith(dungeonFloor);
        }
    }

    /// <summary>
    /// 주변에 아무 것도 없는 외진 곳을 찾는 함수
    /// </summary>
    /// <param name="floorPositions">외진 곳을 찾을 좌표 집합</param>
    /// <returns></returns>
    private List<Vector2Int> FindAllDeadEnds(HashSet<Vector2Int> floorPositions)
    {
        List<Vector2Int> isolatedEnds = new List<Vector2Int>();

        foreach(Vector2Int pos in floorPositions)
        {
            int neighborCount = 0;
            // 좌표 집합들에 대해 상,하,좌,우 탐색
            foreach(Vector2Int dir in Direction2D.cardinalDirectionsList)
            {
                if (floorPositions.Contains(pos + dir)) neighborCount++;
            }

            if (neighborCount <= 1) isolatedEnds.Add(pos); // 인접한 곳이 한 곳 이하라면, 주위에 아무 컨텐츠도 없는 경로임
        }

        return isolatedEnds;
    }

    private HashSet<Vector2Int> CreateDungeons(HashSet<Vector2Int> dungeonStartPositions)
    {
        HashSet<Vector2Int> dungeonFloorPositions = new HashSet<Vector2Int>();
        int dungeonToCreateCount = Mathf.RoundToInt(dungeonStartPositions.Count * dungeonPercent); // dungeonPercent 퍼센트만큼의 던전 개수

        // dungeonStartPositions 랜덤하게 섞어서 dungeonToCreateCount 개수만큼 선택
        List<Vector2Int> dungeonStartPositionsToCreate = dungeonStartPositions.OrderBy(x => Guid.NewGuid()).Take(dungeonToCreateCount).ToList();

        // 랜덤하게 선택된 시작 좌표들에 대해 던전 생성
        foreach(Vector2Int dungeonStartPos in dungeonStartPositionsToCreate)
        {
            // RunRandomWalk을 통해 각 위치에서 랜덤 워크 방식으로 던전 좌표들을 생성
            HashSet<Vector2Int> dungeonPositions = RunRandomWalk(randomWalkParameters, dungeonStartPos); 
            dungeonFloorPositions.UnionWith(dungeonPositions);
        }

        return dungeonFloorPositions;
    }

    /// <summary>
    /// 복도 생성에 필요한 좌표 집합을 만들고 반환. 만들어진 좌표 집합을 첫 번째 인자인 floorPosition에 추가.
    /// 두 번째 인자인 potentialDungeonmPositions에는 복도들이 만들어지는데 있어 시작좌표 집합이 추가됨.
    /// 복도들이 생성되고, 모든 복도들에 있어 처음 복도가 생성되는 시작 좌표에서 던전을 생성하면, 던전들은 서로 연결될 수 있음.
    /// </summary>
    /// <param name="floorPositions"></param>
    /// <param name="dungeonStartPositions"></param>
    private List<List<Vector2Int>> CreateCorridors(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> dungeonStartPositions)
    {
        Vector2Int currPos = base.startPos;
        dungeonStartPositions.Add(currPos);
        List<List<Vector2Int>> corridors = new List<List<Vector2Int>>();

        for (int i = 0; i < corridorLength; i++)
        {
            List<Vector2Int> corridorPositions = ProceduralGenerationAlgo.RandomWalkCorridor(currPos, corridorLength);

            corridors.Add(corridorPositions);
            currPos = corridorPositions[corridorPositions.Count - 1]; // 마지막으로 생성된 복도 좌표를 시작점으로. 그래야 복도가 이어지면서 생성됨

            dungeonStartPositions.Add(currPos); // 복도가 만들어지는 시작점을 던전을 만들기 위한 좌표로 추가
            floorPositions.UnionWith(corridorPositions); // 생성된 복도 좌표 집합을 추가
        }

        return corridors;
    }

    /// <summary>
    /// 복도 크기 2x2 크기로 증가. 코너는 3x3
    /// </summary>
    /// <param name="corridorPositions"></param>
    public List<Vector2Int> IncreaseCorridorSizeByOne(List<Vector2Int> corridorPositions)
    {
        List<Vector2Int> newCorridorPositions = new List<Vector2Int>();
        Vector2Int previousDir = Vector2Int.zero;

        for (int i = 1; i < corridorPositions.Count; i++)
        {
            Vector2Int directionalFromCell = corridorPositions[i] - corridorPositions[i - 1]; // 이전 칸 -> 현재칸 방향벡터

            // 꺾이는 부분(코너)일 때 3x3 크기로 확장
            if (previousDir != Vector2Int.zero && previousDir != directionalFromCell)
            {
                // 3x3 (-1 ~ 1)
                for (int x = -1; x < 2; x++)
                {
                    for (int y = -1; y < 2; y++)
                    {
                        newCorridorPositions.Add(corridorPositions[i - 1] + new Vector2Int(x, y));
                    }
                }         
            }
            // 직전 구간일 때 2x2 크기로 확장
            else
            {
                Vector2Int newCorridorTileOffset = GetDirection90From(directionalFromCell);

                // 기존 타일을 추가하고, 90도 회전한 방향으로 1칸 확장
                newCorridorPositions.Add(corridorPositions[i - 1]);
                newCorridorPositions.Add(corridorPositions[i - 1] + newCorridorTileOffset);
            }

            previousDir = directionalFromCell;
        }

        return newCorridorPositions;
    }

    /// <summary>
    /// 복도 크기 일관되게 3x3 크기로 확장
    /// </summary>
    /// <param name="corridorPositions"></param>
    public List<Vector2Int> IncreaseCorridorBrush3by3(List<Vector2Int> corridorPositions)
    {
        List<Vector2Int> newCorridorPositions = new List<Vector2Int>();
        for (int i = 1; i < corridorPositions.Count; i++)
        {
            // 3x3 크기로 확장(-1 ~ 1)
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    newCorridorPositions.Add(corridorPositions[i - 1] + new Vector2Int(x, y));
                }
            }
        }

        return newCorridorPositions;
    }

    /// <summary>
    /// 현재 방향에서 시계방향으로 90도 회전한 방향 반환
    /// </summary>
    private Vector2Int GetDirection90From(Vector2Int direction)
    {
        if (direction == Vector2Int.up) return Vector2Int.right;
        if (direction == Vector2Int.right) return Vector2Int.down;
        if (direction == Vector2Int.down) return Vector2Int.left;
        if (direction == Vector2Int.left) return Vector2Int.up;

        return Vector2Int.zero;
    }
}
