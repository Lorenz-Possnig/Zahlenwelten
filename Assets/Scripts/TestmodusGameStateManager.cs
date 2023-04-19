using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestmodusGameStateManager : SimpleGameStateManager
{
    #region AudioClips
    [SerializeField]
    private AudioClip HalloIchZeigeDir;

    [SerializeField]
    private AudioClip WennDuNachLinksSchaust;

    [SerializeField]
    private AudioClip MitDenenKannstDu;

    [SerializeField]
    private AudioClip ProbierenWirEsMalMit;

    [SerializeField]
    private AudioClip NimmBitteDenBallon;

    [SerializeField]
    private AudioClip AusDemRegal;

    [SerializeField]
    private AudioClip ProbierenWirDieZahl;

    #endregion AudioClips

    public NewBrettlManager brettlManager;

    private FixedNumberSupplier numberSupplier = new FixedNumberSupplier(new[]
        { 3, 6
        , 12, 15
        , 20, 30, 60
        , 28, 41, 94
        , 100, 600, 200
        , 206, 501, 803
        , 120, 430, 940
        , 213, 385, 642
    });
    // private FixedNumberSupplier numberSupplier = new FixedNumberSupplier(new[] { 1, 2, 3 });
    private const int seconds = 5;

    public SceneLoader sceneLoader;

    protected override void Awake()
    {
        base.Awake();
        DataSaver.Instance.CreateEntry(); // start total time recording
        var num = numberSupplier.getNext();
        _currentNumber = num;
        var digits = IntToDigits(num);
        brettlManager.SetLevel(1);
        brettlManager.ApplyDigits(digits);

        // Intro
        gameStates.Add(-1, gameStageFactory.AudioGameStage(HalloIchZeigeDir, 100));
        gameStates.Add(100, gameStageFactory.AudioStageWithDoorOpenAndClose(WennDuNachLinksSchaust, 200));
        gameStates.Add(200, gameStageFactory.AudioGameStage(MitDenenKannstDu, 300));
        gameStates.Add(300, gameStageFactory.AudioGameStage(ProbierenWirEsMalMit, 400));
        gameStates.Add(400, gameStageFactory.SayNumberStage(
            () => Numbers[_currentNumber],
            500));
        gameStates.Add(500, gameStageFactory.AudioGameStage(NimmBitteDenBallon, 600));
        gameStates.Add(600, gameStageFactory.SayNumberStage(() => Numbers[_currentNumber], 700));
        gameStates.Add(700, gameStageFactory.AudioStageWithDoorOpen(AusDemRegal, 800));
        gameStates.Add(800, StartRepeatNumbers(seconds, 810));
        gameStates.Add(810, gameStageFactory.OpenDoors(900));
        gameStates.Add(900, gameStageFactory.TestmodusWaitForNumberStage(brettlManager, 1010));

        // rest of test
        gameStates.Add(1010, new DecisionStage(numberSupplier.hasNext, 1100, 9000));
        gameStates.Add(1100, new FunctionalGameStage(
            () => {
                var newNum = numberSupplier.getNext();
                _currentNumber = newNum;
                var newDigits = IntToDigits(newNum);
                brettlManager.SetLevel(newDigits.Length);
                brettlManager.ApplyDigits(newDigits);
            },
            () => { },
            1200));
        gameStates.Add(1200, gameStageFactory.AudioStageWithDoorOpen(ProbierenWirDieZahl, 1300));
        gameStates.Add(1300, gameStageFactory.SayNumberStage(() => Numbers[_currentNumber], 1400));
        gameStates.Add(1400, gameStageFactory.TestmodusWaitForNumberStage(brettlManager, 1010));

        // ending => continue to zahlensagen
        gameStates.Add(9000, new FunctionalGameStage(
            () => { sceneLoader.LoadZahlensagenTest(); },
            () => { },
            -1));
        gameStates.Add(-5, new FunctionalGameStage(
            () => { sceneLoader.LoadMenu(); },
            () => { },
            -1));
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    private FunctionalGameStage StopRepeatNumbers(int next) =>
        new FunctionalGameStage(() => CancelInvoke(), () => { }, next);

    private FunctionalGameStage StartRepeatNumbers(int seconds, int next) =>
        new FunctionalGameStage(() => { InvokeRepeating(nameof(RepeatNumber), seconds, seconds); }, () => { }, next);


    private void RepeatNumber()
    {
        if (!_audioSource.isPlaying)
        {
            _audioSource.PlayOneShot(Numbers[_currentNumber]);
        }
    }

    private IEnumerator RepeatNumberAfterSeconds(int seconds)
    {
        for (int i = 1; ; i++)
        {
            if (i % 2 == 0)
            {
                _audioSource.PlayOneShot(Numbers[_currentNumber]);
            }
            else
            {
                yield return new WaitForSeconds(seconds);
            }
        }
    }
}
