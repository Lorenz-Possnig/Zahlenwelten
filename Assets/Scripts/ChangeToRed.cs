using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeToRed : MonoBehaviour
{
    public GameObject self;
    private Color newcolor = Color.red;

    public void ChangeToRedMethod()
    {
        Debug.Log("Change To Red Method");
        self.GetComponent<Renderer>().material.color = newcolor;
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
