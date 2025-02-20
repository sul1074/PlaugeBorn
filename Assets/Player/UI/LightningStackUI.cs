using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightningStackUI : MonoBehaviour // 벽력일섬 스택 표시 UI 스크립트
{
    [SerializeField] private Image[] stackImages;
    private int maxStacks = 3;

    void Awake()
    {
        foreach (Image img in stackImages)
        {
            img.enabled = false;
        }
    }

    public void UpdateStacks(int currentStack)
    {
        for (int i = 0; i < maxStacks; i++)
        {
            stackImages[i].enabled = i < currentStack; // 현재 스택 개수만큼 활성화
        }
    }
}
