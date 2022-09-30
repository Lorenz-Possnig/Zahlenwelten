using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{

    public AbstractDoor[] _doors;

    public void Open()
    {
        foreach (var door in _doors)
            door.Open();
    }

    public void Close()
    {
        foreach (var door in _doors)
            door.Close();
    }

}
