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
    private Brettl[] Brettln;

    void Start()
    {
        Brettln = new Brettl[level];
        int i = 0;
        foreach (GameObject digitBrettl in digitBrettln)
        {
            Debug.Log("X Start(): Brettln Length: " + Brettln.Length);
            Debug.Log("X Start(): Brettl ReferenceDigit: " + digitBrettl.GetComponent<Brettl>().referenceDigit);

            Brettln[i] = digitBrettl.GetComponent<Brettl>();
            i++;
        }
        SetNumbers();
    }

    public void SetNumbers()
    {
        int num = NumberGenerator.GetRandom(level);
        byte[] digits = num.ToString().ToCharArray().Select(x => x.ToString()).Select(byte.Parse).ToArray();
        Debug.Log("X SetNumbers(): Ganze Oasch Array: " + digits);
        for (int i = 0; i < digits.Length; i++)
        {
            Debug.Log("X SetNumbers(): Generated Digit: " + digits[i]);
            Brettln[i].referenceDigit = digits[i];
            Debug.Log("X SetNumbers(): Reference Digit: " + Brettln[i].referenceDigit);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}