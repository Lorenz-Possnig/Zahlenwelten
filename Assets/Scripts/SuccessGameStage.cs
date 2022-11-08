using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuccessGameStage : AbstractGameState
{
    private AudioSource _audioSource;
    private AudioClip _gutGemacht;
    private BrettlManager _brettlManager;
    private byte _brettlToActivate;

    public SuccessGameStage(BrettlManager brettlManager, AudioSource audioSource, AudioClip gutGemacht,
        int next, byte brettlToActivate) : base(next)
    {
        _brettlManager = brettlManager;
        _audioSource = audioSource;
        _gutGemacht = gutGemacht;
        _brettlToActivate = brettlToActivate;
    }

    public override int? GetNextStage() =>
        _audioSource.isPlaying ? null : _executionResult;

    public override void OnTransitionIn()
    {
        _audioSource.PlayOneShot(_gutGemacht);
    }

    public override void OnTransitionOut()
    {
        _brettlManager.EnableBrettl(_brettlToActivate);
        _brettlManager.ResetBrettln();
        _brettlManager.SetBrettlnInactive();
    }

    public override void Update()
    {
        throw new System.NotImplementedException();
    }
}
