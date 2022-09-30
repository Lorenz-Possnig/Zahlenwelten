using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Door
{
    public void Open();

    public void Close();

    public bool IsOpen { get; }
    public bool IsClosed { get; }
    public bool IsMoving { get; }
}

public abstract class AbstractDoor : MonoBehaviour
{
    [SerializeField]
    protected float _rotationSpeed = 1f;
    [SerializeField]
    protected float _highRotation;
    [SerializeField]
    protected float _lowRotation;

    public bool IsOpen { get; protected set; } = false;
    public bool IsClosed { get; protected set; } = true;
    public bool IsMoving { get; protected set; } = false;

    public abstract void Close();
    public abstract void Open();

    protected IEnumerator ChangeRotation(float targetRotation)
    {
        IsMoving = true;
        for (int r = 0; r < _rotationSpeed; r += 1)
        {
            transform.localEulerAngles = new Vector3(0,
                Mathf.LerpAngle(transform.localEulerAngles.y, targetRotation, 5f / _rotationSpeed),
                0);
            yield return null;
        }
        IsMoving = false;
    }

    protected IEnumerator IncreaseRotation() => ChangeRotation(_highRotation);
    protected IEnumerator DecreaseRotation() => ChangeRotation(_lowRotation);
}