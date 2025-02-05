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
    /// 던전을 연결해주는 복도를 먼저 만들고, 던전을 만드는 함수
    /// </summary>
    private void CorridorFirstDungeonGeneration()
    {
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>(); // 복도와 던전을 생성할(타일을 배치할) 좌표 집합
        HashSet<Vector2Int> dungeonStartPositions = new HashSet<Vector2Int>(); // 던전들이 처음 생성될 시작 좌표 집합(각 복도들의 생성 시작점)

        /*
         * floorPositions에 복도가 생성될 좌표 집합을 추가하고
         * potentialRoomPostions에는 던전이 생성될 시작 좌표 집합을 추가함
         */
        CreateCorridors(floorPositions, dungeonStartPositions);

        HashSet<Vector2Int> dungeonFloorPositions = CreateDungeons(dungeonStartPositions); // 던전의 생성 시작 좌표들에 대한 던전을 생성할 좌표 집합
        floorPositions.UnionWith(dungeonFloorPositions); // 기존의 복도들의 좌표 집합 + 던전들의 좌표 집합

        tilemapVisualizer.PaintFloorTiles(floorPositions); // 저장된 좌표 집합들에 대해 타일 배치 -> 복도 + 던전 생성
        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer); // 바닥과 더불어 벽 생성
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
    /// 복도 생성에 필요한 좌표 집합을 만들고, 만들어진 좌표 집합을 첫 번째 인자인 floorPosition에 추가.
    /// 두 번째 인자인 potentialDungeonmPositions 복도들이 만들어지는데 있어 시작좌표 집합이 추가됨.
    /// 복도들이 생성되고, 모든 복도들에 있어 처음 복도가 생성되는 시작 좌표에서 던전을 생성하면, 던전들은 서로 연결될 수 있음.
    /// </summary>
    /// <param name="floorPositions"></param>
    /// <param name="dungeonStartPositions"></param>
    private void CreateCorridors(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> dungeonStartPositions)
    {
        Vector2Int currPos = base.startPos;
        dungeonStartPositions.Add(currPos);

        for (int i = 0; i < corridorLength; i++)
        {
            List<Vector2Int> corridorPositions = ProceduralGenerationAlgo.RandomWalkCorridor(currPos, corridorLength);
            currPos = corridorPositions[corridorPositions.Count - 1]; // 마지막으로 생성된 복도 좌표를 시작점으로. 그래야 복도가 이어지면서 생성됨

            dungeonStartPositions.Add(currPos); // 복도가 만들어지는 시작점을 던전을 만들기 위한 좌표로 추가
            floorPositions.UnionWith(corridorPositions); // 생성된 복도 좌표 집합을 추가
        }
    }
}
