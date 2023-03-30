using Oculus.Voice;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZahlenSagenTraining : SimpleGameStateManager
{
    [SerializeField]
    private AppVoiceExperience _wit;

    [SerializeField]
    private AudioClip IchWerdeDirJetztEineZahlHinschreiben;

    [SerializeField]
    private AudioClip DannSprichDieZahlLaut;

    [SerializeField]
    private AudioClip LeiderNichtVerstanden;

    [SerializeField]
    private AudioClip NoInternetConnection;

    [SerializeField]
    private AudioClip ZahlenSagenIntro;

    public GameObject RecordingSphere;

    public NumberSpawner Spawner;

    public int RecognizedNumber { get; set; } = 0;
    public int CurrentNumber { get => _currentNumber; }

    //private bool _isListening = false;

    public SceneLoader sceneLoader;

    private RandomNumberSupplier numberSupplier = new RandomNumberSupplier();
    private DateTime StartTimestamp;

    private int level = 1;
    private int completedLevel1 = 0;
    private int completedLevel2 = 0;
    private int completedLevel3 = 0;
    private int completedLevel4 = 0;
    private int completedLevel5 = 0;

    protected override void Awake()
    {
        base.Awake();
        StartTimestamp = DateTime.Now;
        DataSaver.Instance.CreateEntry();
        numberSupplier.DigitsAmount = 1;
        var num = numberSupplier.getNext();
        _currentNumber = num;
        gameStates.Add(-5, new FunctionalGameStage(() => {
            sceneLoader.LoadMenu();
        }, () => { }, -1));
        gameStates.Add(-1, new DecisionStage(
            () => Application.internetReachability != NetworkReachability.NotReachable && _wit != null,
            100,
            10));
        gameStates.Add(10, gameStageFactory.AudioGameStage(NoInternetConnection, 11)); // no internet
        gameStates.Add(11, new FunctionalGameStage(() => { DataSaver.Instance.Save();  sceneLoader.LoadMenu(); }, () => { }, -1));
        gameStates.Add(100, gameStageFactory.AudioGameStage(ZahlenSagenIntro, 200));

        gameStates.Add(200, new DecisionStage(() =>
        {
            var now = DateTime.Now;
            return (now - StartTimestamp).Minutes >= 7;
        }, 9000, 300));
        gameStates.Add(300, new FunctionalGameStage(() => {
            if (completedLevel1 == 2)
            {
                level++;
                completedLevel1 = 0;
                numberSupplier.Reset();
            }
            if (completedLevel2 == 5)
            {
                level++;
                completedLevel2 = 0;
                numberSupplier.Reset();
            }
            if (completedLevel3 == 6)
            {
                level++;
                completedLevel3 = 0;
                numberSupplier.Reset();
            }
            if (completedLevel4 == 6)
            {
                level++;
                completedLevel4 = 0;
                numberSupplier.Reset();
            }
            if (completedLevel5 == 6)
            {
                level++;
                completedLevel5 = 0;
                numberSupplier.Reset();
            }
            numberSupplier.DigitsAmount = level;
            var newNum = numberSupplier.getNext();
            _currentNumber = newNum;
            spawnNumbers(_currentNumber);
        }, () => { }, 400));
        gameStates.Add(400, new WaitForUtteranceTraningStage(this, 410, 420, 430, 8000));
        gameStates.Add(410, gameStageFactory.AudioGameStage(DasHastDuGutGemacht, 411));
        gameStates.Add(411, new FunctionalGameStage(() => {
            if (level == 1)
            {
                completedLevel1++;
            }
            if (level == 2)
            {
                completedLevel2++;
            }
            if (level == 3)
            {
                completedLevel3++;
            }
            if (level == 4)
            {
                completedLevel4++;
            }
            if (level == 5)
            {
                completedLevel5++;
            }
        }, () => { }, 200));
        gameStates.Add(420, gameStageFactory.AudioGameStage(ProbierenWirEsNochEinmal, 400));
        gameStates.Add(430, gameStageFactory.AudioGameStage(LeiderNichtVerstanden, 400));

        // decrease Difficulty
        gameStates.Add(8000, new FunctionalGameStage(() =>
        {
            if (level == 1)
            {
                return;
            }
            completedLevel1 = 0;
            completedLevel2 = 0;
            level--;
            numberSupplier.Reset();
        }, () => { }, 8100));
        gameStates.Add(8100, gameStageFactory.AudioGameStage(ProbierenWirEsNochEinmal, 200));

        gameStates.Add(9000, new FunctionalGameStage(() => {
            DataSaver.Instance.Save();
            sceneLoader.LoadMenu();
        }, () => { }, -1));
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    // to catch the events invoked by WIT
    public bool GotResponse { get; set; }
    public bool GotError { get; set; }
    public bool GotAborting { get; set; }
    public bool GotAborted { get; set; }
    public bool GotRequestCompleted { get; set; }
    public bool GotStoppedListeningDueToTimeout { get; set; }

    private void spawnNumbers(int number) => Spawner.SpawnNumber(IntToDigits(number));

    public void SetRecognizedNumber(string[] numbers)
    {
        if (Int32.TryParse(numbers[0], out int number))
        {
            RecognizedNumber = number;
        }
    }
}
