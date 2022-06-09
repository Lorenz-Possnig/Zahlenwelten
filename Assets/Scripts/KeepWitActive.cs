using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.WitAi;

public class KeepWitActive : MonoBehaviour
{
    [SerializeField] private Wit wit;
    // Start is called before the first frame update
    void Start()
    {
        if (!wit) wit = GetComponent<Wit>();
        wit.ActivateImmediately();
        Debug.Log("Activated Wit");
    }

    // Update is called once per frame
    void Update()
    {
        if (!wit.isActiveAndEnabled) {
            Debug.Log("Activating Wit");
            wit.Activate();
        }
    }

    public void LogResponse() {
        Debug.Log("Response Received");
    }
}
