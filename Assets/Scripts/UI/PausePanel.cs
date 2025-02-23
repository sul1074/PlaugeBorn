using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PausePanel : PanelBase
{
    [SerializeField] private Button resumeButton;

    // Awake에서 이벤트를 등록해주고 Start에서 오브젝트 비활성화 시키기
    private void Awake()
    {
        EventManager.OnPausePressed += TogglePanel;
    }
    private void Start()
    {
        // resumeButton 클릭 시 게임 재개
        resumeButton.onClick.AddListener(OnResumeButtonClick);
        gameObject.SetActive(false);
    }

    private void OnResumeButtonClick()
    {
        // 패널 닫기 및 게임 재개
        TogglePanel();
        Time.timeScale = 1f; 
    }

    protected override void OnShow()
    {
        Time.timeScale = 0f;
    }

    protected override void OnHide()
    {
        Time.timeScale = 1f;
    }
}
