using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * 버튼의 클릭 기능에 대한 코드 중복을 줄이기 위한 클래스.
 * 버튼 이름에 매핑되는 기능을 미리 정의해두어, 버튼 이름에 따른 적절한 기능을 함.
 * 따라서 버튼 오브젝트 이름을 정확하게 입력해야 함
 */

public class ButtonClickHandler : MonoBehaviour
{
    private Button button;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => OnButtonClick(gameObject.name));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnButtonClick(string buttonName)
    {
        switch (buttonName)
        {
            case "StartButton":
                SceneController.Instance.LoadScene("Main");
                break;
            case "ExitButton":
                Application.Quit();
                break;
            case "ShopButton":
                // SceneController.Instance.LoadScene("Shop");
                Debug.Log("상점 입장");
                break;
            case "TitleButton":
                SceneController.Instance.LoadScene("Title");
                break;
            case "ResumeButton":
                // PanelPanel에서 구현
                break;
            default:
                Debug.LogError("Button " + buttonName + " not found.");
                break;
        }
    }
}
