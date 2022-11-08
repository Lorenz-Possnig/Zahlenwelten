using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractGameState : IGameState
{
    protected int _executionResult;

    protected AbstractGameState(int executionResult)
    {
        _executionResult = executionResult;
    }

    protected static void NoOp() { }
    public abstract int? GetNextStage();
    public abstract void OnTransitionIn();
    public abstract void OnTransitionOut();
    public abstract void Update();
}
