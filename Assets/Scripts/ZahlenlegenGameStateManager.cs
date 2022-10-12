using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Game State Manager for the Zahlenlegen Szene
/// Handles repeatable steps in the game using stages
/// Essentially changes the update method depending on an integer
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

    public AudioClip[] Numbers;

    public Brettl[] _brettln;

    [SerializeField]
    private DoorManager _doorManager;


    protected override void Awake()
    {
        base.Awake();
        SetupBrettln();
    }


    protected override void Start()
    {
        base.Start();
    }


    private Coroutine _coroutine;
    private const int SecondsLevel1 = 4;
    private const int SecondsLevel2 = 6;
    private const int SecondsLevel3 = 10;

    protected override void Update()
    {
        base.Update();
        #region GameStates
        if (!_audioSource.isPlaying && !_isWaiting)
        {
            switch (_currentGameStage)
            {
                // Intro
                case -1:
                    _isWaiting = true;
                    SetText(""); // clear lorem ipsum
                    StartCoroutine(Wait(4));
                    _currentGameStage = 0;
                    break;
                case 0:
                    SetText("Hallo! Ich zeige dir jetzt, wie du dich in unserer Zahlenwelt zurecht findest.");
                    _audioSource.PlayOneShot(HalloIchZeigeDir);
                    _currentGameStage = 100;
                    break;
                case 100:
                    SetText("Wenn du nach links schaust, siehst du dort ein Regal aus Holz. Dort sind bunte Zahlenballone.");
                    _audioSource.PlayOneShot(WennDuNachLinksSchaust);
                    _doorManager.Open();
                    _currentGameStage = 200;
                    break;
                case 200:
                    SetText("Du dir Zahlen bauen, indem du die Ballons in der richtigen Reihenfolge auf die Bretter in der Mitte ziehst.");
                    _audioSource.PlayOneShot(MitDenenKannstDu);
                    _currentGameStage = 400;
                    break;
                // First Level
                case 400:
                    SetText($"Probieren wir es mal mit der folgenden Zahl");
                    _audioSource.PlayOneShot(ProbierenWirEsMalMit);
                    _currentGameStage = 410;
                    break;
                case 410:
                    _audioSource.PlayOneShot(Numbers[_currentNumber]);
                    _currentGameStage = 500;
                    break;
                case 500:
                    SetText($"Nimm bitte den Zahlenballon aus dem Regal und halte ihn über das Holzbrett.");
                    _audioSource.PlayOneShot(NimmBitteDenBallon);
                    _currentGameStage = 501;
                    break;
                case 501:
                    _audioSource.PlayOneShot(Numbers[_currentNumber]);
                    _currentGameStage = 502;
                    break;
                case 502:
                    _audioSource.PlayOneShot(AusDemRegal);
                    _currentGameStage = 510;
                    break;
                case 510:
                    SetBrettlnActive();
                    ResetBrettl();
                    _coroutine = StartCoroutine(RepeatNumberAfterSeconds(SecondsLevel1));
                    _currentGameStage = 600;
                    break;
                case 520:
                    StopCoroutine(_coroutine);
                    SetText("Probieren wir es noch einmal.");
                    _audioSource.PlayOneShot(ProbierenWirEsNochEinmal);
                    _currentGameStage = 500;
                    break;
                case 600:
                    WaitForNumbers(700, 520);
                    break;
                case 700:
                    StopCoroutine(_coroutine);
                    SetText("Das hast du gut gemacht!");
                    _audioSource.PlayOneShot(DasHastDuGutGemacht);
                    _doorManager.Close();
                    EnableBrettl(1);
                    ResetBrettl();
                    SetupBrettln();
                    SetBrettlnInactive();
                    _currentGameStage = 800;
                    break;
                // Second Level
                case 800:
                    SetText("Wagen wir uns an eine größere Zahl heran!");
                    _audioSource.PlayOneShot(WagenWirUnsAn);
                    ResetBrettl();
                    _currentGameStage = 900;
                    break;
                case 900:
                    _doorManager.Open();
                    SetText($"Probieren wir die Zahl:");
                    _audioSource.PlayOneShot(ProbierenWirDieZahl);
                    _currentGameStage = 910;
                    break;
                case 910:
                    _audioSource.PlayOneShot(Numbers[_currentNumber]);
                    _currentGameStage = 920;
                    break;
                case 920:
                    SetBrettlnActive();
                    _coroutine = StartCoroutine(RepeatNumberAfterSeconds(SecondsLevel2));
                    _currentGameStage = 1000;
                    break;
                case 1000:
                    WaitForNumbers(1100, 1010);
                    break;
                case 1010:
                    SetBrettlnInactive();
                    SetText("Probieren wir es nocheinmal");
                    StopCoroutine(_coroutine);
                    _audioSource.PlayOneShot(ProbierenWirEsNochEinmal);
                    ResetBrettl();
                    _currentGameStage = 900;
                    break;
                case 1100:
                    SetText("Das hast du gut gemacht");
                    StopCoroutine(_coroutine);
                    _doorManager.Close();
                    _audioSource.PlayOneShot(DasHastDuGutGemacht);
                    SetBrettlnInactive();
                    _currentGameStage = _completedLevel2 ? 1300 : 1200;
                    break;
                case 1200:
                    SetText("Machen wir noch so eine Zahl");
                    _audioSource.PlayOneShot(MachenWirNochSoEineZahl);
                    ResetBrettl();
                    SetupBrettln();
                    _completedLevel2 = true;
                    _currentGameStage = 900;
                    break;
                // Third level
                case 1300:
                    SetText("Wagen wir uns an die ganz großen Zahlen!");
                    _audioSource.PlayOneShot(WagenWirUnsAnDieGanzGrossen);
                    EnableBrettl(2);
                    ResetBrettl();
                    SetupBrettln();
                    _currentGameStage = 1400;
                    break;
                case 1400:
                    _audioSource.PlayOneShot(ProbierenWirDieZahl);
                    _currentGameStage = 1410;
                    break;
                case 1410:
                    _audioSource.PlayOneShot(Numbers[_currentNumber]);
                    _currentGameStage = 1420;
                    break;
                case 1420:
                    _doorManager.Open();
                    SetBrettlnActive();
                    _coroutine = StartCoroutine(RepeatNumberAfterSeconds(SecondsLevel3));
                    _currentGameStage = 1500;
                    break;
                case 1500:
                    WaitForNumbers(1600, 1510);
                    break;
                case 1510:
                    SetText("Probieren wir es noch einmal");
                    StopCoroutine(_coroutine);
                    _audioSource.PlayOneShot(ProbierenWirEsNochEinmal);
                    ResetBrettl();
                    _doorManager.Close();
                    SetBrettlnInactive();
                    _currentGameStage = 1410;
                    break;
                case 1600:
                    SetBrettlnInactive();
                    StopCoroutine(_coroutine);
                    _doorManager.Close();
                    SetText("Gut gemacht!");
                    _audioSource.PlayOneShot(DasHastDuGutGemacht);
                    _completedLevel3++;
                    _currentGameStage = _completedLevel3 >= 3 ? 9999 : 1610;
                    break;
                case 1610:
                    SetText("Machen wir noch so eine Zahl");
                    _audioSource.PlayOneShot(MachenWirNochSoEineZahl);
                    ResetBrettl();
                    SetupBrettln();
                    _currentGameStage = 1400;
                    break;
                case 9999:
                    SceneManager.LoadSceneAsync("Menu");
                    break;
                default:
                    break;
            }
        }
        #endregion GameStates
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
    /// Wait for the user to place all digits correctly
    /// </summary>
    /// <param name="success"></param>
    /// <param name="fail"></param>
    private void WaitForNumbers(int success, int fail)
    {
        var brettlState = CheckBrettln();
        switch (brettlState)
        {
            case BrettlState.WRONG:
                _currentGameStage = fail;
                break;
            case BrettlState.CORRECT:
                _currentGameStage = success;
                break;
            case BrettlState.EMPTY:
                break;
        }
    }

    /// <summary>
    /// Set the active state of the Brettl script on all currently active brettln in array _brettln
    /// </summary>
    /// <param name="b"></param>
    private void SetBrettlnActiveState(bool b)
    {
        foreach (var brettl in _brettln)
            if (brettl.gameObject.activeSelf)
                brettl.IsActive = b;
    }

    /// <summary>
    /// Activate the brettl script on all currently active brettln in array _brettln
    /// </summary>
    private void SetBrettlnActive()
    {
        SetBrettlnActiveState(true);
    }

    /// <summary>
    /// Deactivate the brettl script on all currently active brettln in array _brettln
    /// </summary>
    private void SetBrettlnInactive()
    {
        SetBrettlnActiveState(false);
    }

    /// <summary>
    /// Activate _brettln[index] in hierarchy
    /// </summary>
    /// <param name="index"></param>
    private void EnableBrettl(byte index)
    {
        _brettln[index].gameObject.SetActive(true);
    }

    /// <summary>
    /// Reset all objects in _brettln to their original state
    /// and delete balloons which are marked for deletion
    /// </summary>
    private void ResetBrettl()
    {
        foreach (var brettl in _brettln)
        {
            brettl.WrongTry = false;
            brettl.Correct = false;
        }
        foreach (var balloon in
            GameObject.FindGameObjectsWithTag("numberBalloon"))
        {
            balloon.GetComponent<NumberBalloon>().DeleteIfMarked();
        }
    }

    /// <summary>
    /// Represents the combined state of all currently active brettln
    /// </summary>
    private enum BrettlState
    {
        CORRECT,
        WRONG,
        EMPTY
    }

    /// <summary>
    /// Get the combined state of all currently active brettln
    /// </summary>
    /// <returns></returns>
    private BrettlState CheckBrettln()
    {
        if (_brettln.Where(b => b.IsActive).All(b => b.Correct))
        {
            return BrettlState.CORRECT;
        }
        if (_brettln.Where(b => b.IsActive).Any(b => b.WrongTry))
        {
            return BrettlState.WRONG;
        }
        return BrettlState.EMPTY;
    }

    /// <summary>
    /// Get a new random number which length corresponds to amount of brettln active in hierarchy and assign it's digits to the brettln
    /// </summary>
    private void SetupBrettln()
    {
        //Debug.Log("Zahlenwelten [GameStateManager]: Brettln Setup");
        int level = _brettln.Where(b => b.gameObject.activeSelf).Count();
        _currentNumber = NumberGenerator.GetRandom(level);
        byte[] digits = IntToDigits(_currentNumber);

        for (int i = 0; i < level; i++)
            _brettln[i].ReferenceDigit = digits[i];
    }

    #endregion PrivateMethods
}
