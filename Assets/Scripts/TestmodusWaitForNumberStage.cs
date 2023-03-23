using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestmodusWaitForNumberStage : AbstractGameState
{
    private static int failuresInARow = 0;
    private const int allowedFailures = 2;
    private const int endGameGameStage = -5;
    private NewBrettlManager brettlManager;

    private bool hasFinished = false;
    private int next;

    private DataEntryItem entryItem;

    public TestmodusWaitForNumberStage(NewBrettlManager brettlManager, int next) : base(next)
    {
        this.brettlManager = brettlManager;
        this.next = next;
    }

    public override int? GetNextStage() =>
        hasFinished ?
            failuresInARow == allowedFailures ? endGameGameStage : next
        : null;

    public override void OnTransitionIn()
    {
        entryItem = new DataEntryItem();
        hasFinished = false;
        brettlManager.EnableBrettln();
    }

    public override void OnTransitionOut()
    {
        DataSaver.Instance.Entry.ItemsZahlenlegen.Add(entryItem);
        brettlManager.DisableBrettln();
    }

    public override void Update()
    {
        var state = brettlManager.CheckBrettln();
        entryItem.End = System.DateTime.Now;
        entryItem.Item = brettlManager.Item;
        if (state == BrettlManager.BrettlState.CORRECT)
        {
            failuresInARow = 0;
            hasFinished = true;
            entryItem.Correct = true;
        }
        if (state == BrettlManager.BrettlState.WRONG)
        {
            failuresInARow++;
            hasFinished = true;
            entryItem.Correct = false;
        }
    }
}
