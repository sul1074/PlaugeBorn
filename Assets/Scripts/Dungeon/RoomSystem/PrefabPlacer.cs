using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PrefabPlacer : MonoBehaviour
{
    [SerializeField]
    private GameObject itemPrefab;

    /// <summary>
    /// enemyPlacementData 리스트를 순회하면서, 각 적을 원하는 개수만큼 배치
    /// </summary>
    public List<GameObject> PlaceEnemies(List<EnemyPlacementData> enemyPlacementData, ItemPlacementHelper itemPlacementHelper)
    {
        List<GameObject> placedObjects = new List<GameObject>();

        foreach (EnemyPlacementData placementData in enemyPlacementData)
        {
            for (int i = 0; i < placementData.Quantity; i++)
            {
                // 배치 가능한 좌표를 생성
                Vector2? possiblePlacementSpot = itemPlacementHelper. GetItemPlacementPosition(
                    PlacementType.OpenSpace,
                    100,
                    placementData.EnemySize,
                    false
                    );

                // 좌표가 생성 되었으면, 리스트에 추가
                if (possiblePlacementSpot.HasValue)
                { 
                    placedObjects.Add(CreateObject(placementData.EnemyPrefab, possiblePlacementSpot.Value + new Vector2(0.5f, 0.5f))); //Instantiate(placementData.enemyPrefab,possiblePlacementSpot.Value + new Vector2(0.5f, 0.5f), Quaternion.identity)
                }
            }
        }
        return placedObjects;
    }

    /// <summary>
    /// 아이템을 크기 순서대로 내림차순 정렬 후, 배치
    /// </summary>
    public List<GameObject> PlaceAllItems(List<ItemPlacementData> itemPlacementData, ItemPlacementHelper itemPlacementHelper)
    {
        List<GameObject> placedObjects = new List<GameObject>();

        IEnumerable<ItemPlacementData> sortedList = new List<ItemPlacementData>(itemPlacementData).OrderByDescending(placementData => placementData.ItemData.Size.x * placementData.ItemData.Size.y);

        foreach (ItemPlacementData placementData in sortedList)
        {
            // 아이템을 Quantity 만큼 배치
            for (int i = 0; i < placementData.Quantity; i++) 
            {
                // 해당 아이템을 배치할 수 있는 타일 좌표를 계산
                Vector2? possiblePlacementSpot = itemPlacementHelper.GetItemPlacementPosition(
                    placementData.ItemData.PlacementType,
                    100,
                    placementData.ItemData.Size,
                    placementData.ItemData.AddOffset);

                // 배치할 수 있는 좌표가 생성 되었으면 prefab을 해당 좌표에 인스턴스화
                if (possiblePlacementSpot.HasValue)
                {
                    placedObjects.Add(PlaceItem(placementData.ItemData, possiblePlacementSpot.Value));
                }
            }
        }
        return placedObjects;
    }
    /// <summary>
    /// CreateObject()를 호출해 아이템을 배치(인스턴스화)한 후, 아이템 정보를 초기화.
    /// </summary>
    private GameObject PlaceItem(ItemData item, Vector2 placementPosition)
    {
        GameObject newItem = CreateObject(itemPrefab, placementPosition);
        //GameObject newItem = Instantiate(itemPrefab, placementPosition, Quaternion.identity);
        newItem.GetComponent<Item>().Init(item);

        return newItem;
    }

    /// <summary>
    /// 프리펩을 넘겨받은 좌표에 생성
    /// </summary>
    public GameObject CreateObject(GameObject prefab, Vector3 placementPosition)
    {
        if (prefab == null) return null;

        GameObject newItem;

        // 게임이 실행 중이면 Instantiate()로
        if (Application.isPlaying)
        {
            newItem = Instantiate(prefab, placementPosition, Quaternion.identity);
        }
        // 에디터에서 실행 중이면, PrefabUtility.InstantiatePrefab()로 
        else
        {
            newItem = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            newItem.transform.position = placementPosition;
            newItem.transform.rotation = Quaternion.identity;
        }

        return newItem;
    }
}
