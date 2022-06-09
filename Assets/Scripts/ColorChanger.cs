using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    private Material originalMat;
    public Material selectedMat;

    private Renderer renderer;
    /*
   private void SetColor(Transform transform, Color color)
   {
       transform.GetComponent<Renderer>().material.color = color;
   }

   public void UpdateColor(string[] values)
   {
       var colorString = values[0];
       var shapeString = values[1];

       if (ColorUtility.TryParseHtmlString(colorString, out var color))
       {
           if (!string.IsNullOrEmpty(shapeString))
           {
               var shape = transform.Find(shapeString);
               if (shape) SetColor(shape, color);
           }
           else
           {
               for (int i = 0; i < transform.childCount; i++)
               {
                   SetColor(transform.GetChild(i), color);
               }
           }
       }
   }
    */
    public void Start()
    {
        renderer = transform.GetComponent<Renderer>();
        originalMat = renderer.material;
    }

    public void Select()
    {
        if (renderer.material == selectedMat) {
            renderer.material = originalMat;
        } else {
            renderer.material = selectedMat;
        }
    }
}