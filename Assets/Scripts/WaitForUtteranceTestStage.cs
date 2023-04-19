using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForUtteranceTestStage : AbstractGameState
{
    private static int failuresInARow = 0;
    private const int allowedFailures = 2;
    private const int endGameGameStage = -5;

    private enum Result
    {
        SUCCESS,
        FAIL,
        ERROR
    };

    private int next;
    private int error;
    private Result? result;
    private ZahlensagenTestGameStateManager manager;
    private DataEntryItem entryItem;

    public WaitForUtteranceTestStage(ZahlensagenTestGameStateManager manager, int next, int error) : base(next)
    {
        this.next = next;
        this.error = error;
        this.manager = manager;
    }

    public override int? GetNextStage()
    {
        if (failuresInARow == allowedFailures)
        {
            return endGameGameStage;
        }
        if (result == null) return null;
        switch (result)
        {
            case Result.SUCCESS:
            case Result.FAIL:
                return next;
            case Result.ERROR:
                return error;
        }
        return null;
    }

    public override void OnTransitionIn()
    {
        DataSaver.Instance.CreateEntry();
        entryItem = new()
        {
            Comment = "Generic Error",
            Correct = false,
            End = DateTime.Now,
            Item = manager.CurrentNumber.ToString()
        };
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
            Debug.LogError("Error");
            entryItem.End = System.DateTime.Now;
            entryItem.Item = manager.CurrentNumber.ToString();
            manager.GotError = false;
            result = Result.ERROR;
            entryItem.Comment = "Received unspecified error from Wit";
        }
        else if (manager.GotAborting)
        {
            Debug.LogError("Aborting");
            entryItem.End = System.DateTime.Now;
            entryItem.Item = manager.CurrentNumber.ToString();
            manager.GotAborting = false;
            result = Result.ERROR;
            entryItem.Comment = "Received status 'Aborting' from Wit";
        }
        else if (manager.GotAborted)
        {
            Debug.LogError("Aborted");
            entryItem.End = System.DateTime.Now;
            entryItem.Item = manager.CurrentNumber.ToString();
            manager.GotAborted = false;
            result = Result.ERROR;
            entryItem.Comment = "Received status 'Aborted' from Wit";
        }
        else if (manager.GotStoppedListeningDueToTimeout)
        {
            Debug.LogError("Timeout");
            entryItem.End = System.DateTime.Now;
            entryItem.Item = manager.CurrentNumber.ToString();
            manager.GotStoppedListeningDueToTimeout = false;
            result = Result.ERROR;
            entryItem.Comment = "Received status 'StoppedListeningDueToTimeout' from Wit";
        }
        else if (manager.GotRequestCompleted) // this means a request was completed
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
                failuresInARow++;
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
