using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static BrettlManager;

public class NewBrettlManager : MonoBehaviour
{

    public Brettl[] SortedBrettln;
    private int level;
    public string Item { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLevel(int level)
    {
        for(int i = 0; i < level; i++)
        {
            SortedBrettln[i].gameObject.SetActive(true);
        }
        for(int i = level; i < SortedBrettln.Length; i++)
        {
            SortedBrettln[i].gameObject.SetActive(false);
        }
        this.level = level;
    }

    public void ApplyDigits(byte[] digits)
    {
        if (digits.Length != level)
        {
            throw new System.Exception("Digits length and level do not match");
        }
        Item = digits.Select(b => b.ToString()).Aggregate((a, b) => a + b);
        for(int i = 0; i < level; i++)
        {
            var brettl = SortedBrettln[i];
            var digit = digits[i];
            brettl.ReferenceDigit = digit;
        }
    }

    public void EnableBrettln() {
        SetBrettlnActiveState(true);
        SortedBrettln[0].StartBlink();
    }

    public void DisableBrettln()
    {
        SetBrettlnActiveState(false);
        foreach (var brettl in SortedBrettln)
        {
            brettl.WrongTry = false;
            brettl.Correct = false;
            brettl.StopBlink();
        }
        foreach (var balloon in GameObject.FindGameObjectsWithTag("numberBalloon"))
        {
            balloon.GetComponent<NumberBalloon>().DeleteIfMarked();
        }
    }

    private void SetBrettlnActiveState(bool state)
    {
        foreach(var brettl in SortedBrettln)
        {
            brettl.active = state;
        }
    }

    public BrettlState CheckBrettln()
    {
        var activeBrettln = SortedBrettln.Where(b => b.gameObject.activeSelf);
        if (activeBrettln.All(b => b.Correct))
            return BrettlState.CORRECT;
        if (activeBrettln.Any(b => b.WrongTry))
            return BrettlState.WRONG;
        return BrettlState.EMPTY;
    }
}
