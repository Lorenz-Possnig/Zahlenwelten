using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static BrettlManager;

public class WaitForNumbersStage : AbstractGameState
{
    private int _success;
    private int _fail;
    private bool? _failed;
    private BrettlManager _brettlManager;

    public WaitForNumbersStage(BrettlManager brettlManager, int success, int fail) : base(success)
    {
        _success = success;
        _fail = fail;
        _brettlManager = brettlManager;
    }

    public override int? GetNextStage() =>
        !_failed.HasValue ? null : _failed.Value ? _fail : _success;

    public override void OnTransitionIn()
    {
        _brettlManager.ResetBrettln();
        _brettlManager.SetBrettlnActive();
        _failed = null;
    }

    public override void OnTransitionOut()
    {
        _brettlManager.SetBrettlnInactive();
    }

    public override void Update()
    {
        if (!_failed.HasValue) {
            var state = _brettlManager.CheckBrettln();
            switch (state)
            {
                case BrettlState.WRONG:
                    _failed = true;
                    break;
                case BrettlState.CORRECT:
                    _failed = false;
                    break;
            }
        }
    }
}
