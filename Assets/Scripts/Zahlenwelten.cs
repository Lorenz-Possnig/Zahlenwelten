//using System.Collections.Generic;
//using UnityEngine;
//using System;
//using System.Linq;

//public static class Zahlenwelten
//{
//    public static void Log(object o) => Debug.Log($"Zahlenwelten: {o}");

//    public static void Log(string msg) => Zahlenwelten.Log(msg);

//    public static string AsString<T>(this IEnumerable<T> t) => String.Join(", ", t.Select(x => x.ToString()));

//    public static void ForEach<T>(this IEnumerable<T> t, Action<T> a)
//    {
//        foreach(var item in t)
//        {
//            a(item);
//        }
//    }

//}
