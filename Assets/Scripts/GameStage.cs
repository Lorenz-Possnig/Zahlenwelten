using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStage
{
    public IEnumerator Coroutine { get; private set; }
    public int NextStage { get; private set; }

    public GameStage(int next, IEnumerator coroutine)
    {
        NextStage = next;
        Coroutine = coroutine;
    }

}
