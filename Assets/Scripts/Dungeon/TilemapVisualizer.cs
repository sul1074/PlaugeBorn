using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField] private Tilemap floorTilemap; // ���� �ٴ��� �׸� Ÿ�ϸ�
    [SerializeField] private Tilemap wallTilemap; // ���� ���� �׸� Ÿ�ϸ�

    [SerializeField] private TileBase floorTile; // ���� �ٴڿ� ��ġ�� Ÿ��
    [SerializeField] private TileBase wallTile; // ���� ���� ��ġ�� Ÿ��

    /// <summary>
    /// floorPositions�� ��� ��ǥ�鿡 Ÿ���� �׸����� �ϴ� �Լ�. �ܺο��� ��ǥ ���ո� ������ ȣ���� �� �ֵ��� �ϱ� ���� ���ȭ
    /// </summary>
    /// <param name="floorPositions">Ÿ���� ��ġ�� ��ǥ ����</param>
    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        PaintTiles(floorPositions, floorTilemap, floorTile);
    }

    /// <summary>
    /// Ÿ�ϸʿ� positions�� ��� ��ǥ ���վ��� ��� ��ǥ�鿡 ���� Ÿ���� �׸��� �Լ�
    /// </summary>
    /// <param name="positions">Ÿ���� ��ġ�� ��ǥ ����</param>
    /// <param name="tilemap">Ÿ���� �׸� Ÿ�ϸ�</param>
    /// <param name="tile">�׸� Ÿ��</param>
    private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        foreach(Vector2Int pos in positions)
        {
            PaintSingleTile(tilemap, tile, pos);
        }
    }

    /// <summary>
    /// pos ��ǥ�� ���� ���� Ÿ���� �׸��� �Լ�
    /// </summary>
    /// <param name="tilemap">Ÿ���� �׸� Ÿ�ϸ�</param>
    /// <param name="tile">�׸� Ÿ��</param>
    /// <param name="pos">Ÿ���� ��ġ�� ��ǥ</param>
    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int pos)
    {
        Vector3Int tilePos = tilemap.WorldToCell((Vector3Int)pos);
        tilemap.SetTile(tilePos, tile);
    }
    internal void PaintSingleWallTile(Vector2Int pos)
    {
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;

        if (WallTypesHelper.wallTop.Contains(typeAsInt))
            tile = wallTop;
        else if (WallTypesHelper.wallSideRight.Contains(typeAsInt))
            tile = wallSideRight;
        else if (WallTypesHelper.wallSideLeft.Contains(typeAsInt))
            tile = wallSideLeft;
        else if (WallTypesHelper.wallBottom.Contains(typeAsInt))
            tile = wallBottom;
        else if (WallTypesHelper.wallFull.Contains(typeAsInt))
            tile = wallFull;

        if (tile != null)
            PaintSingleTile(wallTilemap, tile, pos);
    }

    /// <summary>
    /// Ÿ�ϸʿ� �׸� ��� Ÿ���� �����ϴ� �Լ�
    /// </summary>
    public void Clear()
    {
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
    }

    /// <summary>
    /// /// WallTypesHelper�� ���ǵ� ������ ���޹��� 2���� Ÿ���� ���Ͽ�, 2���� ������ �´� �밢 ���� ���� ����
    /// </summary>
    internal void PaintSingleCornerWall(Vector2Int pos, string binaryType)
    {
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;

        if (WallTypesHelper.wallInnerCornerDownLeft.Contains(typeAsInt))
            tile = wallInnerCornerDownLeft;
        else if (WallTypesHelper.wallInnerCornerDownRight.Contains(typeAsInt))
            tile = wallInnerCornerDownRight;
        else if (WallTypesHelper.wallDiagonalCornerDownLeft.Contains(typeAsInt))
            tile = wallDiagonalCornerDownLeft;
        else if (WallTypesHelper.wallDiagonalCornerDownRight.Contains(typeAsInt))
            tile = wallDiagonalCornerDownRight;
        else if (WallTypesHelper.wallDiagonalCornerUpRight.Contains(typeAsInt))
            tile = wallDiagonalCornerUpRight;
        else if (WallTypesHelper.wallDiagonalCornerUpLeft.Contains(typeAsInt))
            tile = wallDiagonalCornerUpLeft;
        else if (WallTypesHelper.wallFullEightDirections.Contains(typeAsInt))
            tile = wallFull;
        else if (WallTypesHelper.wallBottomEightDirections.Contains(typeAsInt))
            tile = wallBottom;

        if (tile != null)
            PaintSingleTile(wallTilemap, tile, pos);
    }
}
