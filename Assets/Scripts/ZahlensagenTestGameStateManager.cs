using Oculus.Voice;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZahlensagenTestGameStateManager : SimpleGameStateManager
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

    protected override void Awake()
    {
        base.Awake();
        gameStates.Add(-5, new FunctionalGameStage(() => {
            sceneLoader.LoadMenu();
        }, () => { }, -1));
        gameStates.Add(-1, new DecisionStage(
            () => Application.internetReachability != NetworkReachability.NotReachable && _wit != null,
            100,
            10));
        gameStates.Add(10, gameStageFactory.AudioGameStage(NoInternetConnection, 11)); // no internet
        gameStates.Add(11, new FunctionalGameStage(() => {
            sceneLoader.LoadMenu();
        }, () => { }, -1));
        gameStates.Add(100, gameStageFactory.AudioGameStage(ZahlenSagenIntro, 200));
        gameStates.Add(200, new FunctionalGameStage(() =>
        {
            _currentNumber = numberSupplier.getNext();
            spawnNumbers(_currentNumber);
        }, () => { }, 300));
        gameStates.Add(300, gameStageFactory.WaitForUtteranceStage(this, 400, 310));
        gameStates.Add(310, new FunctionalGameStage(() => { sceneLoader.LoadMenu(); }, () => {}, -1)); // technical error
        gameStates.Add(400, new DecisionStage(numberSupplier.hasNext, 500, 9000));
        gameStates.Add(500, gameStageFactory.AudioGameStage(MachenWirNochSoEineZahl, 200));

        gameStates.Add(9000, new FunctionalGameStage(() => {
            DataSaver.Instance.Save();
            sceneLoader.LoadMenu();
        }, () => { }, -1));
    }

    private void spawnNumbers(int number) => Spawner.SpawnNumber(IntToDigits(number));

    private void ResetStage()
    {
        Spawner.DespawnNumbers();
        RecognizedNumber = 0;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    private void OnDestroy()
    {
    }

    public void SetRecognizedNumber(string[] numbers)
    {
        if (Int32.TryParse(numbers[0], out int number))
        {
            RecognizedNumber = number;
        }
    }

    // to catch the events invoked by WIT
    public bool GotResponse { get; set; }
    public bool GotError { get; set; }
    public bool GotAborting { get; set; }
    public bool GotAborted { get; set; }
    public bool GotRequestCompleted { get; set; }
    public bool GotStoppedListeningDueToTimeout { get; set; }

}
