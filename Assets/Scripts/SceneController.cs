using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneController
{
    public static void LoadScene(string sceneName)
    {
        try { SceneManager.LoadScene(sceneName); }
        catch { Debug.LogError("Scene " + sceneName + " not found."); }
    }
}
