using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimpleRandomWalkDungeonGenerator : AbstractDungeonGenerator
{
    // 스크립터블 오브젝트를 통해 던전 생성에 필요한 패러미터(한 번에 walk할 횟수, walk의 반복 횟수, startRandomlyEachIteration)를 가져옴.
    [SerializeField] private SimpleRandomWalkSO randomWalkParameters;

    protected override void RunProceduralGeneration()
    {
        HashSet<Vector2Int> floorPostioins = RunRandomWalk(); // walkLength만큼 iterations(반복)한 random하게 walk한 경로를 생성 

        tilemapVisualizer.Clear(); // 맵 생성 전에, 타일맵 초기화
        tilemapVisualizer.PaintFloorTiles(floorPostioins); // random하게 walk한 경로에 타일을 그려서 던전을 시각화
        WallGenerator.CreateWalls(floorPostioins, tilemapVisualizer); // 생성된 바닥에 대해, 가장자리에 벽 타일을 그려서 벽 생성
    }

    // walkLength만큼 walk하는 과정을 iterations 만큼 반복한 경로 집합을 반환.
    protected HashSet<Vector2Int> RunRandomWalk()
    {
        Vector2Int curr = startPos;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>(); // 이때까지 walk한 경로를 저장

        for (int i = 0; i < randomWalkParameters.iterations; i++)
        {
            HashSet<Vector2Int> path = ProceduralGenerationAlgo.SimpleRandomWalk(curr, randomWalkParameters.walkLength); // curr을 시작으로 walkLength만큼 walk
            floorPositions.UnionWith(path); // curr에서 walk한 경로를 추가. UnionWith 함수를 통해 중복된 경로는 제거됨

            /* 
             * startRandomlyEachIteration 활성화 시, walk과정에서 매번 새로운 출발점을
             * 기존에 walk한 경로 중에서 랜덤하게 택
             * 따라서 더 분산된 형태의 넓은 던전이 생성
             * 
             * 반대로 startRandomlyEachIteration 비활성화 시, 처음 시작점을 기준으로 모든 walk가 진행
             * 따라서 한 지점에서 뻗어나간, 시작점(중앙) 기준으로 집중된 형태의 던전이 생성
             */
            if (randomWalkParameters.startRandomlyEachIteration)
                curr = floorPositions.ElementAt(UnityEngine.Random.Range(0, floorPositions.Count)); 
        }

        return floorPositions;
    }
}
