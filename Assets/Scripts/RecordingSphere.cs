using Oculus.Voice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordingSphere : MonoBehaviour
{
    private Collider _toucher;
    public Material Red;
    public Material Transparent;
    private Renderer _renderer;
    private bool _isTouched = false;
    public AudioSource _backgroundMusic;

    public AppVoiceExperience wit;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_isTouched)
        {
            _toucher = other;
            if (_backgroundMusic)
                _backgroundMusic.volume = .5f;
            //_renderer.material.color = _red;
            _renderer.material = Red;
            _isTouched = true;
            wit.Activate();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == _toucher)
        {
            //_renderer.material.color = _transparent;
            if (_backgroundMusic)
                _backgroundMusic.volume = 1;
            _renderer.material = Transparent;
            _isTouched = false;
            wit.Deactivate();
        }
    }
}
