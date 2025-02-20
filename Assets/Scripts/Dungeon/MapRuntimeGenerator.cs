using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MapRuntimeGenerator : MonoBehaviour
{
    [SerializeField]
    private UnityEvent OnStart;
    // Start is called before the first frame update
    void Start()
    {
        OnStart?.Invoke();
    }
}