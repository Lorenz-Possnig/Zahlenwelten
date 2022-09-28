using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Destroy a balloon when it hits collider
/// </summary>
public class BallonDestroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.NUMBER_BALOON_TAG))
            Destroy(other.gameObject.transform.parent.gameObject);
    }
}
