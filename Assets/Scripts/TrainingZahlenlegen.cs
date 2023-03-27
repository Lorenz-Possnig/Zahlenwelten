using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingZahlenlegen : SimpleGameStateManager
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

    private RandomNumberSupplier numberSupplier = new RandomNumberSupplier();
    public SceneLoader sceneLoader;
    private DateTime startTime;
    private const int seconds = 5;

    private int level = 1;
    private int completedLevel1 = 0;
    private int completedLevel2 = 0;

    protected override void Awake()
    {
        base.Awake();
        startTime = DateTime.Now;
        DataSaver.Instance.CreateEntry(); // start total time recording
        brettlManager.SetLevel(1);
        numberSupplier.DigitsAmount = 1;
        var num = numberSupplier.getNext();
        _currentNumber = num;
        var digits = IntToDigits(num);
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
        gameStates.Add(900, new TrainingWaitForNumberStage(brettlManager, 910, 920, 8000));
        gameStates.Add(910, gameStageFactory.AudioGameStage(DasHastDuGutGemacht, 911));
        gameStates.Add(911, StopRepeatNumbers(1000));
        gameStates.Add(920, gameStageFactory.AudioGameStage(ProbierenWirEsNochEinmal, 930));
        gameStates.Add(930, StopRepeatNumbers(800));

        // check if we should continue
        gameStates.Add(1000, new DecisionStage(() => {
            var now = DateTime.Now;
            var passedMinutes = (now - startTime).Minutes;
            return passedMinutes >= 7;
        }, 9000, 1010));

        // check if we should increase difficulty
        gameStates.Add(1010, new FunctionalGameStage(() => {
            if (completedLevel1 == 2)
            {
                level = 2;
                completedLevel1 = 0;
                numberSupplier.Reset();
            }
            if (completedLevel2 == 5)
            {
                level = 3;
                completedLevel2 = 0;
                numberSupplier.Reset();
            }
        }, () => { }, 1100));

        // setup numbers
        gameStates.Add(1100, new FunctionalGameStage(() =>
        {
            brettlManager.SetLevel(level);
            numberSupplier.DigitsAmount = level;
            var newNum = numberSupplier.getNext();
            _currentNumber = newNum;
            var newDigits = IntToDigits(newNum);
            brettlManager.ApplyDigits(newDigits);
        }, () => { }, 1200));
        
        // game loop
        gameStates.Add(1200, gameStageFactory.AudioGameStage(ProbierenWirDieZahl, 1201));
        gameStates.Add(1201, gameStageFactory.SayNumberStage(() => Numbers[_currentNumber], 1210));
        gameStates.Add(1210, StartRepeatNumbers(seconds, 1300));
        gameStates.Add(1300, new TrainingWaitForNumberStage(brettlManager, 1310, 1320, 8000));
        gameStates.Add(1310, gameStageFactory.AudioGameStage(DasHastDuGutGemacht, 1311));
        gameStates.Add(1311, StopRepeatNumbers(1312));
        gameStates.Add(1312, new FunctionalGameStage(() => {
            if (level == 1)
            {
                completedLevel1++;
            }
            if (level == 2)
            {
                completedLevel2++;
            }
        }, () => { }, 1000));
        gameStates.Add(1320, gameStageFactory.AudioGameStage(ProbierenWirEsNochEinmal, 1300));

        // decrease difficulty
        gameStates.Add(8000, new FunctionalGameStage(() => {
            if (level == 1)
            {
                return;
            }
            level--;
            numberSupplier.Reset();
        }, () => { }, 1000));

        // ending => continue to zahlensagen
        gameStates.Add(9000, new FunctionalGameStage(
            () => { sceneLoader.LoadZahlensagenTraining(); },
            () => { },
            -1));
        
        // ending => end session
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
        _audioSource.PlayOneShot(Numbers[_currentNumber]);
    }
}
