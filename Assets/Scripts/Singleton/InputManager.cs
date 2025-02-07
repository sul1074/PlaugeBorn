using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
 * 디버깅용. 구조 확정 X
 */

public class InputManager : MonoBehaviour
{
    public static event Action OnPausePressed;
    public static event Action OnStatsPressed;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnPausePressed?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            OnStatsPressed?.Invoke();
        }
    }
}
