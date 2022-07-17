using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeToGreen : MonoBehaviour
{
    public GameObject self;
    private Color newcolor = Color.green;

    public void ChangeToGreenMethod()
    {
        Debug.Log("Change To Green Method");
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
