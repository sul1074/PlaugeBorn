using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField] private Tilemap floorTilemap;
    [SerializeField] private TileBase floorTile; // 바닥에 그릴 타일맵

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
    /// 타일맵에 그린 모든 타일을 제거하는 함수
    /// </summary>
    public void Clear()
    {
        floorTilemap.ClearAllTiles();
    }
}
