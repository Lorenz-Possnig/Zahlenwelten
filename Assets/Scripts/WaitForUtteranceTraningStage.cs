using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForUtteranceTraningStage : AbstractGameState
{
    private static int failuresInARow = 0;
    private const int allowedFailures = 2;
    

    private int next;
    private int error;
    private int decrease;
    private int fail;
    private ZahlenSagenTraining manager;

    private enum Result
    {
        SUCCESS,
        FAIL,
        ERROR
    };
    private Result? result;
    //private bool hasFinished;
    private DataEntryItem entryItem;


    public WaitForUtteranceTraningStage(ZahlenSagenTraining manager, int next, int fail, int error, int decrease) : base(next)
    {
        this.next = next;
        this.error = error;
        this.manager = manager;
        this.decrease = decrease;
        this.fail = fail;
    }

    public override int? GetNextStage()
    {
        if (result == null) return null;
        switch(result)
        {
            case Result.SUCCESS:
                return next;
            case Result.ERROR:
                return error;
            case Result.FAIL:
                failuresInARow++;
                if (failuresInARow >= allowedFailures)
                {
                    failuresInARow = 0;
                    return decrease;
                } else
                {
                    return fail;
                }
        }
        return null;
    }

    public override void OnTransitionIn()
    {
        entryItem = new DataEntryItem();
        result = null;
    }

    public override void OnTransitionOut()
    {
        DataSaver.Instance.Entry.ItemsZahlensagen.Add(entryItem);
    }

    public override void Update()
    {
        if (manager.GotError)
        {
            Debug.LogError("[WaitForUtteranceTrainingStage] Error");
            manager.GotError = false;
            result = Result.ERROR;
            entryItem.Comment = "Received unspecified error from Wit";
            return;
        }

        if (manager.GotAborting)
        {
            Debug.LogError("[WaitForUtteranceTrainingStage] Aborting");
            manager.GotAborting = false;
            result = Result.ERROR;
            entryItem.Comment = "Received status 'Aborting' from Wit";
            return;
        }

        if (manager.GotAborted)
        {
            Debug.LogError("[WaitForUtteranceTrainingStage] Aborted");
            manager.GotAborted = false;
            result = Result.ERROR;
            entryItem.Comment = "Received status 'Aborted' from Wit";
            return;
        }

        if (manager.GotStoppedListeningDueToTimeout)
        {
            Debug.LogError("[WaitForUtteranceTrainingStage] Timeout");
            manager.GotStoppedListeningDueToTimeout = false;
            result = Result.ERROR;
            entryItem.Comment = "Received status 'StoppedListeningDueToTimeout' from Wit";
            return;
        }

        if (manager.GotRequestCompleted) // this means a request was completed
        {
            entryItem.End = System.DateTime.Now;
            entryItem.Item = manager.CurrentNumber.ToString();
            if (manager.RecognizedNumber != 0 && manager.RecognizedNumber == manager.CurrentNumber)
            {
                failuresInARow = 0;
                result = Result.SUCCESS;
                entryItem.Correct = true;
            }
            else if (manager.RecognizedNumber != 0 && manager.RecognizedNumber != manager.CurrentNumber)
            {
                result = Result.FAIL;
                entryItem.Correct = false;
            }
            else
            {
                result = Result.ERROR; // in any other case what was said was not a recognizable number (e.g. hello)
                entryItem.Correct = false;
                entryItem.Comment = "Utterance was not recognized as a number";
            }
            manager.GotRequestCompleted = false;
        }
    }
}
