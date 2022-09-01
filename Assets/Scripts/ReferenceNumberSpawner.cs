using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ReferenceNumberSpawner : MonoBehaviour
{
    [SerializeField]
    private int level = 1;
    [SerializeField]
    private GameObject[] digitBrettln;

    /// <summary>
    /// Get the "Brettl" component from the GameObjects entered in the Inspector
    /// and set the randomly generated numbers as the ReferenceNumber of the components
    /// </summary>
    /// <returns></returns>
    void Start()
    {/*
        Debug.Log("Zahlenwelten [ReferenceNumberSpawner]: Start");
        byte[] digits = NumberGenerator.GetRandom(level)
            .ToString()
            .ToCharArray()
            .Select(x => x.ToString())
            .Select(byte.Parse)
            .ToArray();
        for (int i = 0; i < digitBrettln.Length; i++)
        {
            Debug.Log("Zahlenwelten [ReferenceNumberSpawner]: Set number " + digits[i]);
            digitBrettln[i].GetComponent<Brettl>().ReferenceDigit = digits[i];
        }*/
    }

    // Update is called once per frame
    void Update()
    {

    }
}