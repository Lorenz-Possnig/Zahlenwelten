using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLeft : AbstractDoor, Door
{
    public override void Close()
    {
        if (!IsClosed && !IsMoving && this.gameObject.activeSelf)
        {
            StartCoroutine(DecreaseRotation());
            IsClosed = true;
            IsOpen = false;
        }
    }

    public override void Open()
    {
        if (!IsOpen && !IsMoving && this.gameObject.activeSelf)
        {
            StartCoroutine(IncreaseRotation());
            IsOpen = true;
            IsClosed = false;
        }
    }
}
