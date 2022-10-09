using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonVR : MonoBehaviour
{
    [SerializeField]
    private float _pressDepth = 0.02f;

    [SerializeField]
    private GameObject _button;

    [SerializeField]
    private UnityEvent _onPress;

    [SerializeField]
    private UnityEvent _onRelease;

    private float _originY;

    private GameObject _presser;
    private AudioSource _sound;
    private bool _isPressed;

    public bool IsActive { get; set; } = true;

    void Start()
    {
        _sound = GetComponent<AudioSource>();
        _isPressed = false;
        _originY = transform.localPosition.y;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_isPressed && IsActive)
        {
            _button.transform.localPosition = new Vector3(0, _originY - _pressDepth, 0);
            _presser = other.gameObject;
            _onPress.Invoke();
            _sound.Play();
            _isPressed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == _presser)
        {
            _button.transform.localPosition = new Vector3(0, _originY, 0);
            _onRelease.Invoke();
            _isPressed = false;
        }
    }
}
