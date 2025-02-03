using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * 씬 로드 등의 기능은 수정될 필요성이 낮은 기능들.
 * 해당 기능에 대한 코드 중복성을 줄이고 싶음.
 * 그래서 씽글톤 오브젝트로 만들어서 전역적으로 접근해서 사용할 수 있도록 함.
 */

public class SceneController : MonoBehaviour
{
    private static SceneController _instance;
    // 생성자 private로 선언하여 외부에서 객체 생성하지 못하게 막음.
    private SceneController() { }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(_instance.gameObject);
        }
        else // 이미 싱글톤 오브젝트가 존재하는데
        {
            if (_instance != this) // 서로 다른 객체라면 하나만 존재해야 할 싱글톤 오브젝트가 두 개 이상이라는 뜻
            {
                Destroy(this.gameObject); // 그래서 삭제해주어야 함.
            }
        }
    }

    public static SceneController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("SceneController").AddComponent<SceneController>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    public void LoadScene(string sceneName)
    {
        try { SceneManager.LoadScene(sceneName); }
        catch { Debug.LogError("Scene " + sceneName + " not found."); }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
