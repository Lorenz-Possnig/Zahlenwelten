using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{

    public AbstractDoor[] _doors;

    public bool IsOpen { get => _doors.All(door => door.IsOpen); }
    public bool IsClosed { get => _doors.All(door => door.IsClosed); }

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
