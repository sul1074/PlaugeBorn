using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 던전 내 특정 위치에 아이템을 배치하는 클래스.
/// 방 안의 타일을 OpenSpace(열린 공간) / NearWall(벽 근처)로 분류.
/// 특정 유형의 공간에 아이템을 배치할 수 있는 좌표를 반환.
/// 크기가 큰 아이템도 적절한 위치에 배치 가능.
/// </summary>
public class ItemPlacementHelper
{
    // NearWall, OpenSpace 타입에 따른 타일 좌표를 저장하는 딕셔너리
    Dictionary<PlacementType, HashSet<Vector2Int>> tileByType 
        = new Dictionary<PlacementType, HashSet<Vector2Int>>();

    HashSet<Vector2Int> roomFloorNoCorridor;

    /// <summary>
    /// 생성자. 방의 바닥 타일 정보를 받아서, OpenSpace, NearWall 타입으로 분류
    /// </summary>
    public ItemPlacementHelper(HashSet<Vector2Int> roomFloor, HashSet<Vector2Int> roomFloorNoCorridor)
    {
        Graph graph = new Graph(roomFloor); // 방의 바닥 좌표 정보를, 그래프로 변환

        this.roomFloorNoCorridor = roomFloorNoCorridor;

        // 각 바닥 좌표의 타입 계산
        foreach (Vector2Int position in roomFloorNoCorridor)
        {
            // 인접 8방향을 체크해서 해당 위치가 어떤 타입인지 계산
            int neighboursCount8Dir = graph.GetNeighbours8Directions(position).Count;
            PlacementType type = neighboursCount8Dir < 8 ? PlacementType.NearWall : PlacementType.OpenSpace;

            if (tileByType.ContainsKey(type) == false)
                tileByType[type] = new HashSet<Vector2Int>();

            // 벽 근처인데, 사방이 막혀있으면 제외
            if (type == PlacementType.NearWall && graph.GetNeighbours4Directions(position).Count == 4)
                continue;

            tileByType[type].Add(position);
        }
    }

    /// <summary>
    /// 특정 타입의 공간에 아이템을 배치할 좌표 반환
    /// </summary>
    /// <param name="placementType">배치할 좌표의 타입</param>
    /// <param name="iterationsMax">최대 반복 횟수(자리 찾기 실패 방지)</param>
    /// <param name="size">아이템 크기</param>
    /// <param name="addOffset">아이템이 차지할 공간의 여유 공간</param>
    /// <returns>배치 좌표 or null</returns>
    public Vector2? GetItemPlacementPosition(PlacementType placementType, int iterationsMax, Vector2Int size, bool addOffset)
    {
        // 아이템의 크기가, 배치할 수 있는 면적보다 크면 놓을 수 없음.
        int itemArea = size.x * size.y;
        if (tileByType[placementType].Count < itemArea)
            return null;

        int iteration = 0;
        while (iteration < iterationsMax)
        {
            iteration++;
            int index = UnityEngine.Random.Range(0, tileByType[placementType].Count);
            Vector2Int pos = tileByType[placementType].ElementAt(index);

            // 아이템 크기가 1x1보다 크면, 해당 좌표를 기준으로 그만큼 여유 공간이 있는지 확인해야 함.
            if (itemArea > 1)
            {
                (bool result, List<Vector2Int> placementPositions) = PlaceBigItem(pos, size, addOffset);

                // 배치할 공간 부족시 다른 위치 시도
                if (result == false) continue;

                // 사용한 좌표는 제거
                tileByType[placementType].ExceptWith(placementPositions);
                tileByType[PlacementType.NearWall].ExceptWith(placementPositions);
            }
            // 1x1 크기면 그냥 배치할 수 있음.
            else
            {
                tileByType[placementType].Remove(pos);
            }

            return pos;
        }

        return null;
    }

    /// <summary>
    /// 1x1 보다 큰 아이템을 배치할 공간이 충분한지 확인하고, 가능하면 좌표 리스트를 반환
    /// </summary>
    /// <param name="originPos"></param>
    /// <param name="size"></param>
    /// <param name="addOffset"></param>
    private (bool, List<Vector2Int>) PlaceBigItem(Vector2Int originPos, Vector2Int size, bool addOffset)
    {
        List<Vector2Int> positions = new List<Vector2Int>() { originPos };

        // offset 계산
        int maxX = addOffset ? size.x + 1 : size.x;
        int maxY = addOffset ? size.y + 1 : size.y;
        int minX = addOffset ? -1 : 0;
        int minY = addOffset ? -1 : 0;

        // 아이템이 차지할 타일 좌표 계산
        for (int row = minX; row <= maxX; row++)
        {
            for (int col = minY; col <= maxY; col++)
            {
                // originalPos는 체크할 필요 X
                if (col == 0 && row == 0) continue;

                Vector2Int newPosToCheck = new Vector2Int(originPos.x + row, originPos.y + col);
                
                // 아이템이 놓일 곳들 중에 바닥이 아닌 곳이 있으면, originalPos는 아이템을 놓을 수 없는 좌표이므로 사용 불가함.
                if (roomFloorNoCorridor.Contains(newPosToCheck) == false)
                    return (false, positions);

                positions.Add(newPosToCheck);
            }
        }

        return (true, positions);
    }
}

public enum PlacementType
{
    OpenSpace,
    NearWall
}