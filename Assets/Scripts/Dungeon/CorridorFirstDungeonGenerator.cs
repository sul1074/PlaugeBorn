using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class CorridorFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField] 
    private int corridorLength = 14, corridorCount = 5;

    [SerializeField]
    [Range(0.1f, 1)]
    private float dungeonPercent = 0.8f;

    // 생성된 방들을 저장하는 딕셔너리
    private Dictionary<Vector2Int, HashSet<Vector2Int>> roomsDictionary
        = new Dictionary<Vector2Int, HashSet<Vector2Int>>();

    // 바닥과 복도 좌표를 멤버 변수로 관리
    private HashSet<Vector2Int> floorPositions, corridorPositions;

    // 방별 색깔 저장하는 리스트
    private List<Color> roomColors = new List<Color>();

    [SerializeField]
    private bool showRoomGizmo = false, showCorridorsGizmo;

    public UnityEvent<DungeonData> OnDungeonFloorReady;

    protected override void RunProceduralGeneration()
    {
        CorridorFirstDungeonGeneration();
        DungeonData dungeonData = new DungeonData
        {
            roomsDictionary = this.roomsDictionary,
            corridorPositions = this.corridorPositions,
            floorPositions = this.floorPositions
        };
        OnDungeonFloorReady?.Invoke(dungeonData);
    }

    /// <summary>
    /// 던전을 연결해주는 복도를 먼저 만들고, 복도 끝점 좌표를 기반으로 던전을 만드는 함수
    /// </summary>
    private void CorridorFirstDungeonGeneration()
    {
        floorPositions = new HashSet<Vector2Int>();
        HashSet<Vector2Int> dungeonStartPositions = new HashSet<Vector2Int>(); // 던전들이 처음 생성될 시작 좌표 집합(각 복도들의 생성 시작점)

        CreateCorridors(floorPositions, dungeonStartPositions);

        //tilemapVisualizer.PaintFloorTiles(floorPositions);
        //WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);

        GenerateRooms(dungeonStartPositions);
        //StartCoroutine(GenerateRoomsCoroutine(potentialRoomPositions));
    }

    /// <summary>
    /// 복도 생성에 필요한 좌표 집합을 만들고 반환. 만들어진 좌표 집합을 첫 번째 인자인 floorPosition에 추가.
    /// 두 번째 인자인 dungeonStartPositions에는 복도들이 만들어지는데 있어 시작좌표 집합이 추가됨.
    /// 복도들이 생성되고, 모든 복도들에 있어 처음 복도가 생성되는 시작 좌표에서 던전을 생성하면, 던전들은 서로 연결될 수 있음.
    /// </summary>
    /// <param name="floorPositions"></param>
    /// <param name="dungeonStartPositions"></param>
    private void CreateCorridors(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> dungeonStartPositions)
    {
        Vector2Int currPos = base.startPos;
        dungeonStartPositions.Add(currPos);

        for (int i = 0; i < corridorCount; i++)
        {
            List<Vector2Int> corridor = ProceduralGenerationAlgo.RandomWalkCorridor(currPos, corridorLength);

            currPos = corridor[corridor.Count - 1]; // 마지막으로 생성된 복도 좌표를 시작점으로. 그래야 복도가 이어지면서 생성됨
            dungeonStartPositions.Add(currPos); // 복도가 만들어지는 시작점을 던전을 만들기 위한 좌표로 추가
            floorPositions.UnionWith(corridor); // 생성된 복도 좌표 집합을 추가
        }

        corridorPositions = new HashSet<Vector2Int>(floorPositions);
    }

    private void GenerateRooms(HashSet<Vector2Int> dungeonStartPositions)
    {
        HashSet<Vector2Int> roomsPositions = CreateDungeons(dungeonStartPositions);

        List<Vector2Int> isolatedEnds = FindAllIsolatedEnds(floorPositions);

        CreateRoomsAtIsolatedEnds(isolatedEnds, roomsPositions);

        floorPositions.UnionWith(roomsPositions);

        tilemapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
    }

    private IEnumerator GenerateRoomsCoroutine(HashSet<Vector2Int> potentialRoomPositions)
    {
        yield return new WaitForSeconds(2);

        tilemapVisualizer.Clear();
        GenerateRooms(potentialRoomPositions);

        DungeonData dungeonData = new DungeonData
        {
            roomsDictionary = this.roomsDictionary,
            corridorPositions = this.corridorPositions,
            floorPositions = this.floorPositions
        };
        OnDungeonFloorReady?.Invoke(dungeonData);
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
            
            SaveRoomData(pos, dungeonFloor);
            dungeonFloorPositions.UnionWith(dungeonFloor);
        }
    }

    /// <summary>
    /// 주변에 아무 것도 없는 외진 곳을 찾는 함수
    /// </summary>
    /// <param name="floorPositions">외진 곳을 찾을 좌표 집합</param>
    /// <returns></returns>
    private List<Vector2Int> FindAllIsolatedEnds(HashSet<Vector2Int> floorPositions)
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

            if (neighborCount <= 2) isolatedEnds.Add(pos); // 복도가 2줄로 생성돼서, 연결된 곳이 2곳 이하면 막다른 곳이 됨.
        }

        return isolatedEnds;
    }

    private HashSet<Vector2Int> CreateDungeons(HashSet<Vector2Int> dungeonStartPositions)
    {
        HashSet<Vector2Int> dungeonFloorPositions = new HashSet<Vector2Int>();
        int dungeonToCreateCount = Mathf.RoundToInt(dungeonStartPositions.Count * dungeonPercent); // dungeonPercent 퍼센트만큼의 던전 개수

        // dungeonStartPositions 랜덤하게 섞어서 dungeonToCreateCount 개수만큼 선택
        List<Vector2Int> dungeonStartPositionsToCreate = dungeonStartPositions.OrderBy(x => Guid.NewGuid()).Take(dungeonToCreateCount).ToList();

        ClearRoomData();

        // 랜덤하게 선택된 시작 좌표들에 대해 던전 생성
        foreach(Vector2Int dungeonStartPos in dungeonStartPositionsToCreate)
        {
            // RunRandomWalk을 통해 각 위치에서 랜덤 워크 방식으로 던전 좌표들을 생성
            HashSet<Vector2Int> dungeonPositions = RunRandomWalk(randomWalkParameters, dungeonStartPos);

            SaveRoomData(dungeonStartPos, dungeonPositions);
            dungeonFloorPositions.UnionWith(dungeonPositions);
        }

        return dungeonFloorPositions;
    }

    private void OnDrawGizmosSelected()
    {
        if (showRoomGizmo)
        {
            int i = 0;
            foreach (var roomData in roomsDictionary)
            {
                Color color = roomColors[i];
                color.a = 0.5f;
                Gizmos.color = color;
                Gizmos.DrawSphere((Vector2)roomData.Key, 0.5f);
                foreach (var position in roomData.Value)
                {
                    Gizmos.DrawCube((Vector2)position + new Vector2(0.5f, 0.5f), Vector3.one);
                }
                i++;
            }
        }
        if (showCorridorsGizmo && corridorPositions != null)
        {
            Gizmos.color = Color.magenta;
            foreach (var corridorTile in corridorPositions)
            {
                Gizmos.DrawCube((Vector2)corridorTile + new Vector2(0.5f, 0.5f), Vector3.one);
            }
        }
    }

    private void ClearRoomData()
    {
        roomsDictionary.Clear();
        roomColors.Clear();
    }

    private void SaveRoomData(Vector2Int roomPosition, HashSet<Vector2Int> roomFloor)
    {
        roomsDictionary[roomPosition] = roomFloor;
        roomColors.Add(UnityEngine.Random.ColorHSV());
    }
}
