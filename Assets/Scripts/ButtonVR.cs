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

    void Start()
    {
        _sound = GetComponent<AudioSource>();
        _isPressed = false;
        _originY = transform.localPosition.y;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_isPressed)
        {
            _button.transform.localPosition = new Vector3 { x = 0, y = _originY - _pressDepth, z = 0 };
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
            _button.transform.localPosition = new Vector3 { x = 0, y = _originY, z = 0 };
            _onRelease.Invoke();
            _isPressed = false;
        }
    }

    public void Test()
    {
        GameObject test = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        test.transform.localScale = new Vector3 { x = 0.5f, y = 0.5f, z = 0.5f };
        test.AddComponent<Rigidbody>();
    }

}
