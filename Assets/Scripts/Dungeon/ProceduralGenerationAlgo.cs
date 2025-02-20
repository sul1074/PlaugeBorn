using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� Ŭ������ ����
public static class ProceduralGenerationAlgo
{
    /// <summary>
    /// HashSet�� C++�� unordered_set�� ������ ���. �����ߺ��� ��� ��� X
    /// startPosition���κ��� walkLengh��ŭ ������ ���̸� walk
    /// walkLength ����ŭ ������ ����� ������ HashSet<Vector2Int> ���·� ��ȯ
    /// </summary>
    /// <param name="startPos"> random walk�� ������</param>
    /// <param name="walkLength">�� ���� �ݺ����� ������ walk ��</param>
    /// <returns></returns>
    public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startPos, int walkLength)
    {
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();
        path.Add(startPos);
        Vector2Int prevPos = startPos;

        for (int i = 0; i < walkLength; i++)
        {
            Vector2Int next = prevPos + Direction2D.GetRandomCardinalDirection(); // �������� �̵��� ���� ��ġ

            path.Add(next);
            prevPos = next;
        }

        return path;
    }

    /// <summary>
    /// ������ ���� ��ġ���� �����Ͽ� ������ �������� ���� ���̸�ŭ ������ ����.
    /// ���������� ������ walk ��ǥ �ε����� �����ϱ� ���� List ���. HashSet�� �ε��� ������ �ȵ�.
    /// ���������� ������ walk ��ǥ���� �ٽ� ������ ������ ��� �̾����� ����.
    /// </summary>
    /// <param name="startPos">������ ������ ������</param>
    /// <param name="corridorLength">�� ���� �ݺ����� ������ ������ ����</param>
    /// <returns></returns>
    public static List<Vector2Int> RandomWalkCorridor(Vector2Int startPos, int corridorLength)
    {
        List<Vector2Int> corridor = new List<Vector2Int>(); // ���� ��θ� �����ϴ� ����Ʈ
        Vector2Int dir = Direction2D.GetRandomCardinalDirection(); // ������ ������ ����
        Vector2Int curr = startPos;

        corridor.Add(curr);
        corridor.Add(CalculateAdditionalCorridorTile(curr, dir));

        for (int i = 0; i < corridorLength; i++)
        {
            curr += dir; // ���� ��ġ���� �� �������� ���ư�
            corridor.Add(curr);
            corridor.Add(CalculateAdditionalCorridorTile(curr, dir));
        }

        return corridor;
    }

    /// <summary>
    /// ������ �� ĭ Ȯ����
    /// </summary>
    private static Vector2Int CalculateAdditionalCorridorTile(Vector2Int currentPosition, Vector2Int direction)
    {
        Vector2Int offset = Vector2Int.zero;
        if (direction.y > 0)
            offset.x = 1;
        else if (direction.y < 0)
            offset.x = -1;
        else if (direction.x > 0)
            offset.y = -1;
        else
            offset.y = 1;
        return currentPosition + offset;
    }

    /// <summary>
    /// ������ �����ϰ� ������, ���� ����� ������ ����Ʈ�� ��ȯ
    /// </summary>
    /// <param name="spaceToSplit">���� ����</param>
    /// <param name="minWidth">�� �̻� ���� �� ���� �ּ� �ʺ�</param>
    /// <param name="minHeight">�� �̻� ���� �� ���� �ּ� ����</param>
    /// <returns></returns>
    public static List<BoundsInt> BinarySpacePartitioning(BoundsInt spaceToSplit, int minWidth, int minHeight)
    {
        Queue<BoundsInt> roomsQueue = new Queue<BoundsInt>();
        List<BoundsInt> roomsList = new List<BoundsInt>();

        roomsQueue.Enqueue(spaceToSplit);

        while (roomsQueue.Count > 0)
        {
            BoundsInt room = roomsQueue.Dequeue();

            if (room.size.x < minWidth || room.size.y < minHeight) continue; // �� �̻� ���� �� ������ �н�
            
            // ���� �������� ���� ������ ����(����, ����)�� �����ϰ� ����
            if (Random.value < 0.5f) 
            {
                // ���̰� �ּ� ���� 2�� �̻��� ���� �������� ���� �� ����.
                if (room.size.y >= minHeight * 2)
                {
                    SplitHorizontally(minHeight, roomsQueue, room);
                }
                // �ʺ� �ּ� ���� 2�� �̻��� ���� �������� ���� �� ����.
                else if (room.size.x >= minWidth * 2)
                {
                    SplitVertically(minWidth, roomsQueue, room);
                }
                // �� �̻� ���� �� ������, ���� �� ����Ʈ�� �߰�
                else if (room.size.x >= minWidth && room.size.y >= minHeight)
                {
                    roomsList.Add(room);
                }
            }
            else
            {
                // �ʺ� �ּ� ���� 2�� �̻��� ���� �������� ���� �� ����.
                if (room.size.x >= minWidth * 2)
                {
                    SplitVertically(minWidth, roomsQueue, room);
                }
                // ���̰� �ּ� ���� 2�� �̻��� ���� �������� ���� �� ����.
                else if (room.size.y >= minHeight * 2)
                {
                    SplitHorizontally(minHeight, roomsQueue, room);
                }
                // �� �̻� ���� �� ������, ���� �� ����Ʈ�� �߰�
                else if (room.size.x >= minWidth && room.size.y >= minHeight)
                {
                    roomsList.Add(room);
                }
            }
        }

        return roomsList;
    }

    private static void SplitVertically(int minWidth, Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        int xSplit = Random.Range(minWidth, room.size.x - minWidth); // room�� x��ǥ ���������� �����ϰ� ���

        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(xSplit, room.size.y, room.size.z)); // min ~ xSplit���� ����
        BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x + xSplit, room.min.y, room.min.z),
            new Vector3Int(room.size.x - xSplit, room.size.y, room.size.z)); // xSplit ~ room�� x��ǥ ������ ���� ����

        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
    }
     
    private static void SplitHorizontally(int minHeight, Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        int ySplit = Random.Range(minHeight, room.size.y - minHeight); // room�� y��ǥ ���������� �����ϰ� ���

        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(room.size.x, ySplit, room.size.z)); // min ~ ySplit���� ����
        BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x, room.min.y + ySplit, room.min.z),
            new Vector3Int(room.size.x, room.size.y - ySplit, room.size.z)); // ySplit ~ room�� y��ǥ ������ ���� ����

        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
    }

// ��,��,��,�� �� ������ (Cardinal Direction)���� ������ ��ȯ
public static class Direction2D 
{
    public static List<Vector2Int> cardinalDirectionsList = new List<Vector2Int>
    {
       new Vector2Int(0, 1), // Up
       new Vector2Int(1, 0), // Right
       new Vector2Int(0, -1), // Down
       new Vector2Int(-1, 0) // Left
    };

    public static Vector2Int GetRandomCardinalDirection() { return cardinalDirectionsList[Random.Range(0, cardinalDirectionsList.Count)]; }
}

