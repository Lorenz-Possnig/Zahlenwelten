using Oculus.Voice;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ZahlensagenGameStateManager : SimpleGameStateManager
{

    [SerializeField]
    private AppVoiceExperience _wit;

    [SerializeField]
    private AudioClip IchWerdeDirJetztEineZahlHinschreiben;

    [SerializeField]
    private AudioClip DannSprichDieZahlLaut;


    private int _recognizedNumber = 0;

    private bool _isListening = false;

    protected override void Awake()
    {
        base.Awake();
    }


    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if (!_audioSource.isPlaying && !_isWaiting)
        {
            switch (_currentGameStage)
            {
                case -1:
                    SetText("");
                    StartCoroutine(Wait(4));
                    _currentGameStage = 100;
                    break;
                case 100:
                    SetText("Hallo! Ich werde dir jetzt eine Zahl hinschreiben");
                    _audioSource.PlayOneShot(IchWerdeDirJetztEineZahlHinschreiben);
                    _currentGameStage = 200;
                    break;
                case 200:
                    SetText("Dann sprich die Zahl bitte laut und deutlich aus");
                    _audioSource.PlayOneShot(DannSprichDieZahlLaut);
                    _currentNumber = NumberGenerator.GetRandom(1);
                    _currentGameStage = 300;
                    break;
                // Level 1
                case 300:
                    SetText($"{_currentNumber}");
                    _currentGameStage = 400;
                    break;
                case 400:
                    WaitForUtterance(500, 420);
                    break;
                case 420:
                    ResetWit();
                    SetText("Probieren wir es noch einmal");
                    _audioSource.PlayOneShot(ProbierenWirEsNochEinmal);
                    _currentGameStage = 300;
                    break;
                case 500:
                    ResetWit();
                    SetText("Das hast du gut gemacht");
                    _audioSource.PlayOneShot(DasHastDuGutGemacht);
                    _currentGameStage = 600;
                    break;
                // Level 2
                case 600:
                    SetText("Wagen wir uns an eine größere Zahl heran!");
                    _audioSource.PlayOneShot(WagenWirUnsAn);
                    _currentNumber = NumberGenerator.GetRandom(2);
                    _currentGameStage = 700;
                    break;
                case 700:
                    SetText($"{_currentNumber}");
                    _currentGameStage = 800;
                    break;
                case 800:
                    WaitForUtterance(900, 810);
                    break;
                case 810:
                    ResetWit();
                    SetText("Probieren wir es noch einmal");
                    _audioSource.PlayOneShot(ProbierenWirEsNochEinmal);
                    _currentGameStage = 700;
                    break;
                case 900:
                    ResetWit();
                    SetText("Das hast du gut gemacht");
                    _audioSource.PlayOneShot(DasHastDuGutGemacht);
                    _currentGameStage = _completedLevel2 ? 1000 : 910;
                    break;
                case 910:
                    SetText("Machen wir noch so eine Zahl");
                    _audioSource.PlayOneShot(MachenWirNochSoEineZahl);
                    _completedLevel2 = true;
                    _currentNumber = NumberGenerator.GetRandom(2);
                    _currentGameStage = 700;
                    break;
                // Level 3
                case 1000:
                    SetText("Wagen wir uns an die ganz großen Zahlen!");
                    _audioSource.PlayOneShot(WagenWirUnsAnDieGanzGrossen);
                    _currentNumber = NumberGenerator.GetRandom(3);
                    _currentGameStage = 1100;
                    break;
                case 1100:
                    SetText($"{_currentNumber}");
                    _currentGameStage = 1200;
                    break;
                case 1200:
                    WaitForUtterance(1300, 1210);
                    break;
                case 1210:
                    ResetWit();
                    SetText("Probieren wir es noch einmal");
                    _audioSource.PlayOneShot(ProbierenWirEsNochEinmal);
                    _currentGameStage = 1100;
                    break;
                case 1300:
                    ResetWit();
                    SetText("Das hast du gut gemacht!");
                    _audioSource.PlayOneShot(DasHastDuGutGemacht);
                    _currentGameStage = _completedLevel3 >= 3 ? 9999 : 1310;
                    break;
                case 1310:
                    SetText("Machen wir noch so eine Zahl");
                    ResetWit();
                    _audioSource.PlayOneShot(MachenWirNochSoEineZahl);
                    _completedLevel3++;
                    _currentNumber = NumberGenerator.GetRandom(3);
                    _currentGameStage = 1100;
                    break;
                case 9999:
                    SceneManager.LoadSceneAsync("Menu");
                    break;
                default:
                    break;
            }
        }
    }

    public void SetRecognizedNumber(string[] numbers)
    {
        if (Int32.TryParse(numbers[0], out int number))
        {
            _recognizedNumber = number;
        }
    }

    public void OnStopListening()
    {
        ResetWit();
    }

    private void ResetWit()
    {
        _wit.Deactivate();
        _isListening = false;
        _recognizedNumber = 0;
    }

    private void WaitForUtterance(int success, int failure)
    {

        if (!_isListening)
        {
            Debug.Log("Zahlenwelten: Start Listening");
            _wit.Activate();
            _isListening = true;
        }

        if (!_isListening)
        {
            _currentGameStage = failure;
            Debug.Log("Zahlenwelten: Break Listening");
            return;
        }

        if (_recognizedNumber != 0 && _recognizedNumber == _currentNumber)
        {
            _currentGameStage = success;
            ResetWit();
        }
        else if (_recognizedNumber != 0 && _recognizedNumber != _currentNumber)
        {
            _currentGameStage = failure;
            ResetWit();
        }
    }
}
