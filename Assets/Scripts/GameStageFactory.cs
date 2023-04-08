using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStageFactory
{
    private Action _noOp = () => { };

    private AudioSource _audioSource;
    private DoorManager _doorManager;
    private AudioClip[] _numbers;
    private BrettlManager _brettlManager;
    private AudioClip _gutGemacht;

    public GameStageFactory(AudioSource audioSource, DoorManager doorManager,
        AudioClip[] numbers, BrettlManager brettlManager, AudioClip gutGemacht)
    {
        _audioSource = audioSource;
        _doorManager = doorManager;
        _numbers = numbers;
        _brettlManager = brettlManager;
        _gutGemacht = gutGemacht;
    }

    public AudioGameStage AudioGameStage(AudioClip clip, int next) =>
        new AudioGameStage(_audioSource, clip, next);

    public AudioGameStage AudioStageWithDoorOpenAndClose(AudioClip clip, int next) => _doorManager == null ?
            new AudioGameStage(_audioSource, clip, next, _noOp, _noOp) :
            new AudioGameStage(_audioSource, clip, next, () => _doorManager.Open(), () => _doorManager.Close());

    public AudioGameStage AudioStageWithDoorClose(AudioClip clip, int next) => _doorManager == null ?
        new AudioGameStage(_audioSource, clip, next, _noOp, _noOp) :
        new AudioGameStage(_audioSource, clip, next, _noOp, () => _doorManager.Close());

    public AudioGameStage AudioStageWithDoorOpen(AudioClip clip, int next) => _doorManager == null ?
        new AudioGameStage(_audioSource, clip, next, _noOp, _noOp) :
        new AudioGameStage(_audioSource, clip, next, () => _doorManager.Open(), _noOp);

    public AudioGameStage SayNumberStage(int number, int next) =>
        new AudioGameStage(_audioSource, _numbers[number], next);

    public AudioGameStage SayNumberStage(Func<AudioClip> getter, int next) =>
        new AudioGameStage(_audioSource, getter, next);
    
    public SuccessGameStage SuccessStage(byte brettlToActivate, int next) =>
        new SuccessGameStage(_brettlManager, _audioSource, _gutGemacht, next, brettlToActivate);

    public WaitForNumbersStage WaitForNumbersStage(int success, int fail) =>
        new WaitForNumbersStage(_brettlManager, success, fail);

    public TestmodusWaitForNumberStage TestmodusWaitForNumberStage(NewBrettlManager brettlManager, int next) =>
        new TestmodusWaitForNumberStage(brettlManager, next);

    public FunctionalGameStage OpenDoors(int next) => _doorManager == null ?
        new FunctionalGameStage(_noOp, _noOp, next) :
        new FunctionalGameStage(() => _doorManager.Open(), _noOp, next);

    public FunctionalGameStage Close(int next) => _doorManager == null ?
        new FunctionalGameStage(_noOp, _noOp, next) :
        new FunctionalGameStage(() => _doorManager.Close(), _noOp, next);

    public WaitForUtteranceTestStage WaitForUtteranceStage(ZahlensagenTestGameStateManager manager, int next, int error) =>
        new WaitForUtteranceTestStage(manager, next, error);
}
