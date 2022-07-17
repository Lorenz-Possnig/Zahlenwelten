using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    [SerializeField]
    private Material mat1;
    [SerializeField]
    private Material mat2;
    [SerializeField]
    private GameObject self;

    public void SwapMaterial()
    {
        Debug.Log("Calling SwapMaterial()");

        if (self.GetComponent<MeshRenderer>().material.color != mat1.color)
        {
            self.GetComponent<MeshRenderer>().material.color = mat1.color;
        }
        else
        {
            self.GetComponent<MeshRenderer>().material.color = mat2.color;
        }
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
