using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberGenerator
{
    /// <summary>
    /// Get a random number with the amount of digits specified by level
    /// The retuned number will never include the digit 0
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public static int GetRandom(int level)
    {
        string number = "";
        for(int i = 0; i < level; i++)
        {
            number += Random.Range(1, 10).ToString();
        }
        return int.Parse(number);
    }


    public static int GetDigit() => Random.Range(1, 10);
}

