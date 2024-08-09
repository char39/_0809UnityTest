using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelSelfDestroy : MonoBehaviour
{
    void Start()
    {
        Invoke("Destroy", 1.8f);
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
