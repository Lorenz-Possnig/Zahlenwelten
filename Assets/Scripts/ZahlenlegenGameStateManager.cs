using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Refactored to be a finite mealey automaton
/// End state of the automaton will only depend on the last state, not the transition to it
/// </summary>
public class ZahlenlegenGameStateManager : SimpleGameStateManager
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

    private Coroutine _coroutine;
    private const int SecondsLevel1 = 4;
    private const int SecondsLevel2 = 6;
    private const int SecondsLevel3 = 10;

    protected override void Awake()
    {
        base.Awake();
        SetupBrettln();
        
        // Intro
        gameStates.Add(-1, gameStageFactory.AudioGameStage(HalloIchZeigeDir, 100));
        gameStates.Add(100, gameStageFactory.AudioStageWithDoorOpenAndClose(WennDuNachLinksSchaust, 200));
        gameStates.Add(200, gameStageFactory.AudioGameStage(MitDenenKannstDu, 300));
        gameStates.Add(300, gameStageFactory.AudioGameStage(ProbierenWirEsMalMit, 400));
        gameStates.Add(400, gameStageFactory.SayNumberStage(() => Numbers[_currentNumber], 500));
        
        // Level 1 
        gameStates.Add(500, gameStageFactory.AudioGameStage(NimmBitteDenBallon, 600));
        gameStates.Add(600, gameStageFactory.SayNumberStage(() => Numbers[_currentNumber], 700));
        gameStates.Add(700, gameStageFactory.AudioStageWithDoorOpen(AusDemRegal, 800));
        gameStates.Add(800, new FunctionalGameStage(() => {}, () => {
            _coroutine = StartCoroutine(RepeatNumberAfterSeconds(SecondsLevel1));
        }, 810));
        gameStates.Add(810, gameStageFactory.OpenDoors(900));
        gameStates.Add(900, gameStageFactory.WaitForNumbersStage(1000, 910));
        gameStates.Add(910, new FunctionalGameStage(() => StopCoroutine(_coroutine), () => { }, 920));
        gameStates.Add(920, gameStageFactory.AudioGameStage(ProbierenWirEsNochEinmal, 500));
        gameStates.Add(1000, new FunctionalGameStage(() => StopCoroutine(_coroutine), () => { }, 1001));
        gameStates.Add(1001, gameStageFactory.SuccessStage(1, 1010));

        // Level 2
        gameStates.Add(1010, new FunctionalGameStage(SetupBrettln, () => { }, 1100));
        gameStates.Add(1100, gameStageFactory.AudioGameStage(WagenWirUnsAn, 1200));
        gameStates.Add(1200, gameStageFactory.AudioStageWithDoorOpen(ProbierenWirDieZahl, 1210));
        gameStates.Add(1210, gameStageFactory.SayNumberStage(() => Numbers[_currentNumber], 1220)); // TODO: hier kommt irgendwie die falsche zahl
        gameStates.Add(1220, new FunctionalGameStage(() => { }, () => {
            _coroutine = StartCoroutine(RepeatNumberAfterSeconds(SecondsLevel2));
        }, 1300));
        gameStates.Add(1300, gameStageFactory.WaitForNumbersStage(1400, 1310));
        gameStates.Add(1310, new FunctionalGameStage(() => StopCoroutine(_coroutine), () => { }, 1320));
        gameStates.Add(1320, gameStageFactory.AudioGameStage(ProbierenWirEsNochEinmal, 1200));
        gameStates.Add(1400, gameStageFactory.AudioGameStage(DasHastDuGutGemacht, 1401));
        gameStates.Add(1401, new FunctionalGameStage(() => StopCoroutine(_coroutine), () => { }, 1410));
        gameStates.Add(1410, new DecisionStage(() => _completedLevel2, 1440, 1420));
        gameStates.Add(1420, new FunctionalGameStage(() => { 
            _completedLevel2 = true;
            SetupBrettln();
        }, () => { }, 1430));
        gameStates.Add(1430, gameStageFactory.AudioGameStage(MachenWirNochSoEineZahl, 1200));

        // Level 3
        gameStates.Add(1440, gameStageFactory.SuccessStage(2, 1500));
        gameStates.Add(1500, new FunctionalGameStage(SetupBrettln, () => { }, 1600));
        gameStates.Add(1600, gameStageFactory.AudioGameStage(WagenWirUnsAnDieGanzGrossen, 1700));
        gameStates.Add(1700, gameStageFactory.AudioStageWithDoorOpen(ProbierenWirDieZahl, 1800));
        gameStates.Add(1800, gameStageFactory.SayNumberStage(() => Numbers[_currentNumber], 1810));
        gameStates.Add(1810, new FunctionalGameStage(() => { }, () => {
            _coroutine = StartCoroutine(RepeatNumberAfterSeconds(SecondsLevel2));
        }, 1900));
        gameStates.Add(1900, gameStageFactory.WaitForNumbersStage(2000, 1910));
        gameStates.Add(1910, new FunctionalGameStage(() => { StopCoroutine(_coroutine); }, () => { }, 1920));
        gameStates.Add(1920, gameStageFactory.AudioGameStage(ProbierenWirEsNochEinmal, 1700));
        gameStates.Add(2000, gameStageFactory.AudioGameStage(DasHastDuGutGemacht, 2001));
        gameStates.Add(2001, new FunctionalGameStage(() => StopCoroutine(_coroutine), () => { }, 2010));
        gameStates.Add(2010, new DecisionStage(() => _completedLevel3 >= 3, 9999, 2020));
        gameStates.Add(2020, new FunctionalGameStage(() =>
        {
            _completedLevel3++;
            SetupBrettln();
        }, () => { }, 2030));
        gameStates.Add(2030, gameStageFactory.AudioGameStage(MachenWirNochSoEineZahl, 1700));

        // return to menu
        gameStates.Add(9999, new FunctionalGameStage(() => { SceneManager.LoadSceneAsync("Menu"); }, () => { }, -2));
    }


    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    #region PrivateMethods

    private IEnumerator RepeatNumberAfterSeconds(int seconds)
    {
        for(int i = 1; ; i++)
        {
            if (i % 2 == 0)
            {
                _audioSource.PlayOneShot(Numbers[_currentNumber]);
            } else
            {
                yield return new WaitForSeconds(seconds);
            }
        }
    }

    /// <summary>
    /// Get a new random number which length corresponds to amount of brettln active in hierarchy and assign it's digits to the brettln
    /// </summary>
    private void SetupBrettln()
    {
        int level = _brettln.Count(b => b.gameObject.activeSelf);
        _currentNumber = NumberGenerator.GetRandom(level);
        byte[] digits = IntToDigits(_currentNumber);

        for (int i = 0; i < level; i++)
        {
            _brettln[i].ReferenceDigit = digits[i];
        }
    }

    #endregion PrivateMethods
}
