using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    void Awake()
    {
        var other = GameObject.FindGameObjectsWithTag("music");
        if (other.Length > 1)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(gameObject);
    }
}
