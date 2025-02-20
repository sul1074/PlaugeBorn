using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class CorridorFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    private List<List<Vector2Int>> corridorList = new List<List<Vector2Int>>();
    private List<Color> corridorColors = new List<Color>();

    [SerializeField] 
    private int corridorLength = 14, corridorCount = 5;

    [SerializeField]
    [Range(0.1f, 1)]
    private float dungeonPercent = 0.8f;

    // ������ ����� �����ϴ� ��ųʸ�
    private Dictionary<Vector2Int, HashSet<Vector2Int>> roomsDictionary
        = new Dictionary<Vector2Int, HashSet<Vector2Int>>();

    // �ٴڰ� ���� ��ǥ�� ��� ������ ����
    private HashSet<Vector2Int> floorPositions, corridorPositions;

    // �溰 ���� �����ϴ� ����Ʈ
    private List<Color> roomColors = new List<Color>();

    [SerializeField]
    private bool showRoomGizmo = false, showCorridorsGizmo;

    public UnityEvent<DungeonData> OnDungeonFloorReady;

    protected override void RunProceduralGeneration()
    {
        CorridorFirstDungeonGeneration();
        DungeonData dungeonData = new DungeonData
        (
            this.roomsDictionary,
            this.floorPositions,
            this.corridorPositions
        );
        OnDungeonFloorReady?.Invoke(dungeonData);
    }

    /// <summary>
    /// ������ �������ִ� ������ ���� �����, ���� ���� ��ǥ�� ������� ������ ����� �Լ�
    /// </summary>
    private void CorridorFirstDungeonGeneration()
    {
        floorPositions = new HashSet<Vector2Int>();
        HashSet<Vector2Int> dungeonStartPositions = new HashSet<Vector2Int>(); // �������� ó�� ������ ���� ��ǥ ����(�� �������� ���� ������)

        CreateCorridors(floorPositions, dungeonStartPositions);

        //tilemapVisualizer.PaintFloorTiles(floorPositions);
        //WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);

        GenerateRooms(dungeonStartPositions);
        //StartCoroutine(GenerateRoomsCoroutine(potentialRoomPositions));
    }

    /// <summary>
    /// ���� ������ �ʿ��� ��ǥ ������ ����� ��ȯ. ������� ��ǥ ������ ù ��° ������ floorPosition�� �߰�.
    /// �� ��° ������ dungeonStartPositions���� �������� ��������µ� �־� ������ǥ ������ �߰���.
    /// �������� �����ǰ�, ��� �����鿡 �־� ó�� ������ �����Ǵ� ���� ��ǥ���� ������ �����ϸ�, �������� ���� ����� �� ����.
    /// </summary>
    /// <param name="floorPositions"></param>
    /// <param name="dungeonStartPositions"></param>
    private void CreateCorridors(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> dungeonStartPositions)
    {
        Vector2Int currPos = base.startPos;
        dungeonStartPositions.Add(currPos);

        corridorList.Clear();
        corridorColors.Clear();

        for (int i = 0; i < corridorCount; i++)
        {
            List<Vector2Int> corridor = ProceduralGenerationAlgo.RandomWalkCorridor(currPos, corridorLength);

            currPos = corridor[corridor.Count - 1]; // ���������� ������ ���� ��ǥ�� ����������. �׷��� ������ �̾����鼭 ������
            dungeonStartPositions.Add(currPos); // ������ ��������� �������� ������ ����� ���� ��ǥ�� �߰�
            floorPositions.UnionWith(corridor); // ������ ���� ��ǥ ������ �߰�

            corridorList.Add(corridor); // �� ������ ����Ʈ�� ����
            corridorColors.Add(UnityEngine.Random.ColorHSV()); // �� �������� ������ �� �߰�
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
        (
            this.roomsDictionary,
            this.corridorPositions,
            this.floorPositions
        );
        OnDungeonFloorReady?.Invoke(dungeonData);
    }

    /// <summary>
    /// ������ ���鿡 ���� �߰��� ������ �����ϴ� �Լ�
    /// </summary>
    /// <param name="isolatedEnds">������ ������ ������ ��ǥ ����</param>
    /// <param name="dungeonFloorPositions">�߰��� ������ ���� ��ǥ ������ �߰��� �з�����</param>
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
    /// �ֺ��� �ƹ� �͵� ���� ���� ���� ã�� �Լ�
    /// </summary>
    /// <param name="floorPositions">���� ���� ã�� ��ǥ ����</param>
    /// <returns></returns>
    private List<Vector2Int> FindAllIsolatedEnds(HashSet<Vector2Int> floorPositions)
    {
        List<Vector2Int> isolatedEnds = new List<Vector2Int>();

        foreach(Vector2Int pos in floorPositions)
        {
            int neighborCount = 0;
            // ��ǥ ���յ鿡 ���� ��,��,��,�� Ž��
            foreach(Vector2Int dir in Direction2D.cardinalDirectionsList)
            {
                if (floorPositions.Contains(pos + dir)) neighborCount++;
            }

            if (neighborCount <= 2) isolatedEnds.Add(pos); // ������ 2�ٷ� �����ż�, ����� ���� 2�� ���ϸ� ���ٸ� ���� ��.
        }

        return isolatedEnds;
    }

    private HashSet<Vector2Int> CreateDungeons(HashSet<Vector2Int> dungeonStartPositions)
    {
        HashSet<Vector2Int> dungeonFloorPositions = new HashSet<Vector2Int>();
        int dungeonToCreateCount = Mathf.RoundToInt(dungeonStartPositions.Count * dungeonPercent); // dungeonPercent �ۼ�Ʈ��ŭ�� ���� ����

        // dungeonStartPositions �����ϰ� ��� dungeonToCreateCount ������ŭ ����
        List<Vector2Int> dungeonStartPositionsToCreate = dungeonStartPositions.OrderBy(x => Guid.NewGuid()).Take(dungeonToCreateCount).ToList();

        ClearRoomData();

        // �����ϰ� ���õ� ���� ��ǥ�鿡 ���� ���� ����
        foreach(Vector2Int dungeonStartPos in dungeonStartPositionsToCreate)
        {
            // RunRandomWalk�� ���� �� ��ġ���� ���� ��ũ ������� ���� ��ǥ���� ����
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
        
        //if (showCorridorsGizmo && corridorPositions != null)
        //{
        //    Gizmos.color = Color.magenta;
        //    foreach (var corridorTile in corridorPositions)
        //    {
        //        Gizmos.DrawCube((Vector2)corridorTile + new Vector2(0.5f, 0.5f), Vector3.one);
        //    }
        //}

        if (showCorridorsGizmo && corridorList.Count > 0)
        {
            for (int i = 0; i < corridorList.Count; i++)
            {
                Gizmos.color = corridorColors[i]; // ������ ���� ����
                foreach (var tile in corridorList[i])
                {
                    Gizmos.DrawCube((Vector2)tile + new Vector2(0.5f, 0.5f), Vector3.one);
                }
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
