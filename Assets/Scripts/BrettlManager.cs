using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BrettlManager
{
    public Brettl[] _brettln;

    public BrettlManager(Brettl[] brettln)
    {
        _brettln = brettln;
    }

    public void ResetBrettln()
    {
        foreach (var brettl in _brettln)
        {
            brettl.WrongTry = false;
            brettl.Correct = false;
        }
        foreach (var balloon in
            GameObject.FindGameObjectsWithTag("numberBalloon"))
        {
            balloon.GetComponent<NumberBalloon>().DeleteIfMarked();
        }
    }

    /// <summary>
    /// Set the active state of the Brettl script on all currently active brettln in array _brettln
    /// </summary>
    /// <param name="b"></param>
    private void SetBrettlnActiveState(bool b)
    {
        foreach (var brettl in _brettln)
            if (brettl.gameObject.activeSelf)
                brettl.IsActive = b;
    }

    /// <summary>
    /// Activate the brettl script on all currently active brettln in array _brettln
    /// </summary>
    public void SetBrettlnActive()
    {
        SetBrettlnActiveState(true);
    }

    /// <summary>
    /// Deactivate the brettl script on all currently active brettln in array _brettln
    /// </summary>
    public void SetBrettlnInactive()
    {
        SetBrettlnActiveState(false);
    }

    /// <summary>
    /// Activate _brettln[index] in hierarchy
    /// </summary>
    /// <param name="index"></param>
    public void EnableBrettl(byte index)
    {
        _brettln[index].gameObject.SetActive(true);
    }

    public BrettlState CheckBrettln()
    {
        if (_brettln.Where(b => b.IsActive).All(b => b.Correct))
        {
            return BrettlState.CORRECT;
        }
        if (_brettln.Where(b => b.IsActive).Any(b => b.WrongTry))
        {
            return BrettlState.WRONG;
        }
        return BrettlState.EMPTY;
    }

    public enum BrettlState
    {
        CORRECT,
        WRONG,
        EMPTY
    }
}
