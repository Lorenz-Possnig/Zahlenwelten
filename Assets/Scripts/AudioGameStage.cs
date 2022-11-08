using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioGameStage : AbstractGameState
{

    private readonly AudioSource _audioSource;
    private AudioClip _audioClip;
    private readonly Action _onIn = null;
    private readonly Action _onOut = null;
    private readonly Func<AudioClip> _getter = null;

    public AudioGameStage(AudioSource audioSource, AudioClip audioClip, int nextGameStage) : base(nextGameStage)
    {
        _audioSource = audioSource;
        _audioClip = audioClip;
    }

    public AudioGameStage(AudioSource audioSource, AudioClip audioClip, int nextGameStage, Action onIn, Action onOut) : base(nextGameStage)
    {
        _audioSource = audioSource;
        _audioClip = audioClip;
        _onIn = onIn;
        _onOut = onOut;
    }
    
    public AudioGameStage(AudioSource audioSource, Func<AudioClip> audioClipGetter, int nextGameStage) : base(nextGameStage)
    {
        _audioSource = audioSource;
        _audioClip = null;
        _getter = audioClipGetter;
    }

    public override int? GetNextStage() => _audioSource.isPlaying ? null : _executionResult;

    public override void OnTransitionIn() {
        if (_getter != null)
        {
            _audioClip = _getter();
        }
        _audioSource.PlayOneShot(_audioClip);
        if (_onIn != null)
        {
            _onIn();
        }
    }

    public override void OnTransitionOut()
    {
        if (_onOut != null)
        {
            _onOut();
        }
    }

    public override void Update() => NoOp();
}