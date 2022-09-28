using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Float the GameObject up and down on Sinus wave
/// </summary>
public class BrettlMover : MonoBehaviour
{
    private const int TEN_THOUSAND = 10000;

    [SerializeField]
    private float _amplitude = 1;
    [SerializeField]
    private float _frequency = 1;

    private void Start()
    {
        StartCoroutine(Float());
    }

    private IEnumerator Float()
    {
        for(; ;)
        {
            var yPosition = _amplitude / TEN_THOUSAND * Mathf.Sin(Time.fixedTime * Mathf.PI * _frequency) + transform.position.y;
            transform.position = new Vector3(transform.position.x, yPosition, transform.position.z);
            yield return new WaitForEndOfFrame();
        }
    }
}
