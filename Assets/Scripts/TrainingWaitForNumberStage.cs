using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingWaitForNumberStage : AbstractGameState
{
    private static int failuresInARow = 0;
    private const int allowedFailures = 2;

    private NewBrettlManager brettlManager;
    private bool hasFinished = false;
    private int success;
    private int fail;
    private int decreaseLevelStage;

    private DataEntryItem entryItem;


    public TrainingWaitForNumberStage(NewBrettlManager brettlManager, int success, int fail, int decrease) : base(success)
    {
        this.brettlManager = brettlManager;
        this.success = success;
        this.fail = fail;
        this.decreaseLevelStage = decrease;
    }

    public override int? GetNextStage() { 
        if (hasFinished)
        {
            if (entryItem.Correct)
            {
                return success;
            } else
            {
                failuresInARow++;
                if (failuresInARow >= allowedFailures)
                {
                    return decreaseLevelStage;
                } else
                {
                    return fail;
                }
            }
        }
        return null;
    }

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
            hasFinished = true;
            entryItem.Correct = false;
        }
    }
}
