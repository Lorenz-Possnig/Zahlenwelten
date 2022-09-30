using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRight : AbstractDoor
{

    public override void Close()
    {
        if (!IsClosed && !IsMoving)
        {
            StartCoroutine(IncreaseRotation());
            IsClosed = true;
            IsOpen = false;
        }
    }

    public override void Open()
    {
        if (!IsOpen && !IsMoving)
        {
            StartCoroutine(DecreaseRotation());
            IsOpen = true;
            IsClosed = false;
        }
    }
}
