using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberGenerator
{
    //private const Dictionary<int, string> numMapping = new Dictionary<int, string>{
    //    1 : "eins",
    //    2 : "zwai",
    //    3 : "drrei",
    //    4 : "fia",
    //    5 : "fünf",
    //    6 : "sechs",
    //    7 : "sieben",
    //    8 : "acht",
    //    9 : "neun",
    //};

    /*
     * 1.) Abhängig vom Level richtige Number generieren
     * 2.) zu Voice recognizeable string machen
    */ 

    /*public static int GetRandom(int level)
    {
        Debug.Log($"Generating random number for level: {level}");
        int number = Random.Range(1,10);      

        if (level == 2)
            number += Random.Range(1,10) * 10;

        if (level == 3)
            number += Random.Range(1, 10) * 100;

        return number;
    }*/

    public static int GetRandom(int level)
    {
        string number = "";
        for(int i = 0; i < level; i++)
        {
            number += Random.Range(1, 10).ToString();
        }
        return int.Parse(number);
    }

}

