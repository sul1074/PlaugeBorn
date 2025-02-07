using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
 * PanelBase를 상속받아서 Panel의 show, hide 시의 에니메이션이나 사운드 커스텀할 수 있도록 함.
 */

public abstract class PanelBase : MonoBehaviour
{
    // 외부에서 호출될 수 있도록
    public void TogglePanel()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            OnShow();
        }
        else
        {
            gameObject.SetActive(false);
            OnHide();
        }
    }

    protected abstract void OnShow();
    protected abstract void OnHide();
}
