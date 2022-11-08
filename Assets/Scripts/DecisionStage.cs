using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionStage : AbstractGameState
{

    private Func<bool> _supplier;
    private int _success;
    private int _fail;

    public DecisionStage(Func<bool> supplier, int success, int fail) : base(success)
    {
        _supplier = supplier;
        _success = success;
        _fail = fail;
    }

    public override int? GetNextStage() =>
        _supplier() ? _success : _fail;

    public override void OnTransitionIn() => NoOp();

    public override void OnTransitionOut() => NoOp();

    public override void Update() => NoOp();
}
