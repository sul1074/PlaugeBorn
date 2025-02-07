using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// CustomEditor를 사용하여 AbstractDungeonGenerator 또는 그 자식 클래스들의 인스펙터 UI를 커스터마이징
/// 이렇게 함으로써 플레이 중에 씬 내 배치된 버튼을 눌러서 맵을 생성하는 것이 아닌,
/// 플레이 중이 아니더라도, 인스펙터 창에서 버튼을 눌러 맵을 생성함.
/// 이로써 게임 플레이가 종료되더라도 생성된 맵은 씬에 그대로 남게 됨.
/// </summary>
[CustomEditor(typeof(AbstractDungeonGenerator), true)]
public class RandomDungeonGeneratorEditor : Editor
{
    AbstractDungeonGenerator generator; // DungeonGenerator 컴포넌트의 인스턴스를 저장할 변수

    private void Awake()
    {
        // 에디터에서 선택된 게임 오브젝트나 컴포넌트를 AbstractDungeonGenerator 타입으로 캐스팅해서 generator에 할당
        generator = (AbstractDungeonGenerator)target; 
    }

    // 인스펙터 GUI를 그릴 때 호출되는 함수
    public override void OnInspectorGUI()
    {
        // 기본적으로 제공되는 인스펙터 UI 요소들을 그려줌
        base.OnInspectorGUI();

        // "Create Dungeon" 버튼을 인스펙터에 추가
        if (GUILayout.Button("Create Dungeon"))
        {
            // 버튼이 클릭되면 GenerateDungeon 메서드를 호출하여 던전을 생성
            generator.GenerateDungeon();
        }
    }
}
