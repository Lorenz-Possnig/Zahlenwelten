using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrettlMover : MonoBehaviour
{
    public float ascHeigth = 1;
    public float speed = 1;

    private void Start()
    {
        StartCoroutine(Float());
    }
    private IEnumerator Float()
    {
        for(; ;)
        {
            var yPosition = ascHeigth/10000 * Mathf.Sin(Time.fixedTime * Mathf.PI * speed) + transform.position.y;
            transform.position = new Vector3(transform.position.x, yPosition, transform.position.z);
            yield return new WaitForEndOfFrame();
        }
    }
}
