using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimpleRandomWalkDungeonGenerator : AbstractDungeonGenerator
{
    // 스크립터블 오브젝트를 통해 던전 생성에 필요한 패러미터(한 번에 walk할 횟수, walk의 반복 횟수, startRandomlyEachIteration)를 가져옴.
    [SerializeField] protected SimpleRandomWalkSO randomWalkParameters;

    protected override void RunProceduralGeneration()
    {
        HashSet<Vector2Int> floorPostioins = RunRandomWalk(randomWalkParameters, base.startPos); // walkLength만큼 iterations(반복)한 random하게 walk한 경로를 생성 

        tilemapVisualizer.Clear(); // 맵 생성 전에, 타일맵 초기화
        tilemapVisualizer.PaintFloorTiles(floorPostioins); // random하게 walk한 경로에 타일을 그려서 던전을 시각화
        WallGenerator.CreateWalls(floorPostioins, tilemapVisualizer); // 생성된 바닥에 대해, 가장자리에 벽 타일을 그려서 벽 생성
    }

    /// <summary>
    /// walkLength만큼 walk하는 과정을 iterations 만큼 반복한 경로 집합을 반환.
    /// </summary>
    /// <param name="parameters">던전 생성에 필요한 패러미터를 가지고 있는 스크립터블 오브젝트</param>
    /// <param name="pos">Random Walk를 시작할 좌표</param>
    /// <returns></returns>
    protected HashSet<Vector2Int> RunRandomWalk(SimpleRandomWalkSO parameters, Vector2Int pos)
    {
        Vector2Int curr = pos;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>(); // 이때까지 walk한 경로를 저장

        for (int i = 0; i < parameters.iterations; i++)
        {
            HashSet<Vector2Int> path = ProceduralGenerationAlgo.SimpleRandomWalk(curr, parameters.walkLength); // curr을 시작으로 walkLength만큼 walk
            floorPositions.UnionWith(path);

            // startRandomlyEachIteration 활성화시, 매 반복마다 walk 시작점을 갱신
            if (parameters.startRandomlyEachIteration)
                curr = path.ElementAt(UnityEngine.Random.Range(0, path.Count)); 
        }

        return floorPositions;
    }
}
