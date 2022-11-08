using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionalGameStage : AbstractGameState
{

    Action _onIn;
    Action _onOut;

    public FunctionalGameStage(Action onIn, Action onOut, int next) : base(next)
    {
        _onIn = onIn;
        _onOut = onOut;
    }

    public override int? GetNextStage() => _executionResult;

    public override void OnTransitionIn()
    {
        if (_onIn != null)
        {
            _onIn();
        }
    }

    public override void OnTransitionOut()
    {
        if (_onOut != null)
        {
            _onOut();
        }
    }

    public override void Update() => NoOp();
}
