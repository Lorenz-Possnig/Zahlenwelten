using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallonDestroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("numberBalloon"))
            Destroy(other.gameObject.transform.parent.gameObject);        
    }
}
