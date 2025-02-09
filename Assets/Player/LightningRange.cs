using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningRange : MonoBehaviour
{
    public GameObject dashColliderObj;
    void Start()
    {
        dashColliderObj.SetActive(false);
    }

}
