using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameState
{
    public int? GetNextStage();

    public void OnTransitionIn();

    public void OnTransitionOut();

    public void Update();
}
