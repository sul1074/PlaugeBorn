using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridorFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField] 
    private int corridorLength = 14, corridorCount = 5;

    [SerializeField]
    [Range(0.1f, 1)]
    private float roomPercent = 0.8f;

    [SerializeField]
    public SimpleRandomWalkSO roomGenerationParameters;

    protected override void RunProceduralGeneration()
    {
        CorridorFirstDungeonGeneration();
    }

    private void CorridorFirstDungeonGeneration()
    {
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();

        CreateCorridors(floorPositions);

        tilemapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
    }

    private void CreateCorridors(HashSet<Vector2Int> floorPositions)
    {
        Vector2Int corridorPos = base.startPos;
        
        for (int i = 0; i < corridorLength; i++)
        {
            List<Vector2Int> corridor = ProceduralGenerationAlgo.RandomWalkCorridor(corridorPos, corridorLength);

            corridorPos = corridor[corridor.Count - 1]; // 마지막으로 생성된 복도 좌표를 시작점으로. 그래야 복도가 이어지면서 생성됨
            floorPositions.UnionWith(corridor); // 생성된 복도 좌표 집합을 추가
        }
    }
}
