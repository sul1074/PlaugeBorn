using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    // 던전 바닥과 벽을 그릴 타일맵
    [SerializeField] 
    private Tilemap floorTilemap, wallTilemap;

    [SerializeField]
    private TileBase floorTile, wallTop, wallSideRight, wallSideLeft, wallBottom, wallFull;

    [SerializeField]
    private TileBase wallInnerCornerDownLeft, wallInnerCornerDownRight,
        wallDiagonalCornerDownRight, wallDiagonalCornerDownLeft, wallDiagonalCornerUpRight, wallDiagonalCornerUpLeft;

    /// <summary>
    /// floorPositions에 담긴 좌표들에 타일을 그리도록 하는 함수. 외부에서 좌표 집합만 가지고 호출할 수 있도록 하기 위해 모듈화
    /// </summary>
    /// <param name="floorPositions">타일을 배치할 좌표 집합</param>
    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        PaintTiles(floorPositions, floorTilemap, floorTile);
    }

    /// <summary>
    /// 타일맵에 positions에 담긴 좌표 집합안의 모든 좌표들에 대해 타일을 그리는 함수
    /// </summary>
    /// <param name="positions">타일을 배치할 좌표 집합</param>
    /// <param name="tilemap">타일을 그릴 타일맵</param>
    /// <param name="tile">그릴 타일</param>
    private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        foreach(Vector2Int pos in positions)
        {
            PaintSingleTile(tilemap, tile, pos);
        }
    }

    /// <summary>
    /// pos 좌표에 대해 단일 타일을 그리는 함수
    /// </summary>
    /// <param name="tilemap">타일을 그릴 타일맵</param>
    /// <param name="tile">그릴 타일</param>
    /// <param name="pos">타일을 배치할 좌표</param>
    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int pos)
    {
        Vector3Int tilePos = tilemap.WorldToCell((Vector3Int)pos);
        tilemap.SetTile(tilePos, tile);
    }

    /// <summary>
    /// WallTypesHelper에 정의된 정보와 전달받은 2진수 타입을 비교하여, 2진수 정보에 맞는 방향의 기본 직교 방향 벽을 생성
    /// </summary>
    internal void PaintSingleWallTile(Vector2Int pos, string binaryType)
    {
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;

        if (WallTypesHelper.wallTop.Contains(typeAsInt))
            tile = wallTop;
        else if (WallTypesHelper.wallSideRight.Contains(typeAsInt))
            tile = wallSideRight;
        else if (WallTypesHelper.wallSideLeft.Contains(typeAsInt))
            tile = wallSideLeft;
        else if (WallTypesHelper.wallBottm.Contains(typeAsInt))
            tile = wallBottom;
        else if (WallTypesHelper.wallFull.Contains(typeAsInt))
            tile = wallFull;

        if (tile != null)
            PaintSingleTile(wallTilemap, tile, pos);
    }

    /// <summary>
    /// 타일맵에 그린 모든 타일을 제거하는 함수
    /// </summary>
    public void Clear()
    {
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
    }

    /// <summary>
    /// /// WallTypesHelper에 정의된 정보와 전달받은 2진수 타입을 비교하여, 2진수 정보에 맞는 대각 방향 벽을 생성
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
        else if (WallTypesHelper.wallBottmEightDirections.Contains(typeAsInt))
            tile = wallBottom;

        if (tile != null)
            PaintSingleTile(wallTilemap, tile, pos);
    }
}
