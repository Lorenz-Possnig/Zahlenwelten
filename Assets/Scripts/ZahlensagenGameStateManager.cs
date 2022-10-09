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

    public GameObject RecordingSphere;

    public NumberSpawner Spawner;

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
                    if (Application.internetReachability == NetworkReachability.NotReachable)
                    {
                        _currentGameStage = 10;
                    } else
                    {
                        StartCoroutine(Wait(4));
                        _currentGameStage = 100;
                    }
                    break;
                case 10:
                    SetText("Irgendetwas funktioniert hier gerade nicht. Bitte hole deinen Betreuer");
                    _currentGameStage = 11;
                    break;
                case 11:
                    //SceneManager.LoadScene("Menu");
                    GetComponent<SceneLoader>().LoadScene("Menu");
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
                    spawnNumbers(_currentNumber);
                    _currentGameStage = 400;
                    break;
                case 400:
                    WaitForUtterance(500, 420, 410);
                    break;
                case 410:
                    SetText("Das habe ich leider nicht verstanden. Bitte versuche es noch einmal");
                    _currentGameStage = 300;
                    break;
                case 420:
                    SetText("Probieren wir es noch einmal");
                    _audioSource.PlayOneShot(ProbierenWirEsNochEinmal);
                    ResetStage();
                    _currentGameStage = 300;
                    break;
                case 500:
                    SetText("Das hast du gut gemacht");
                    ResetStage();
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
                    spawnNumbers(_currentNumber);
                    _currentGameStage = 800;
                    break;
                case 800:
                    WaitForUtterance(900, 820, 810);
                    break;
                case 810:
                    SetText("Das habe ich leider nicht verstanden. Bitte versuche es noch einmal");
                    _currentGameStage = 700;
                    break;
                case 820:
                    SetText("Probieren wir es noch einmal");
                    ResetStage();
                    _audioSource.PlayOneShot(ProbierenWirEsNochEinmal);
                    _currentGameStage = 700;
                    break;
                case 900:
                    ResetStage();
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
                    spawnNumbers(_currentNumber);
                    _currentGameStage = 1200;
                    break;
                case 1200:
                    WaitForUtterance(1300, 1220, 1210);
                    break;
                case 1210:
                    SetText("Das habe ich leider nicht verstanden. Bitte versuche es noch einmal");
                    _currentGameStage = 1100;
                    break;
                case 1220:
                    SetText("Probieren wir es noch einmal");
                    ResetStage();
                    _audioSource.PlayOneShot(ProbierenWirEsNochEinmal);
                    _currentGameStage = 1100;
                    break;
                case 1300:
                    SetText("Das hast du gut gemacht!");
                    ResetStage();
                    _audioSource.PlayOneShot(DasHastDuGutGemacht);
                    _currentGameStage = _completedLevel3 >= 3 ? 9999 : 1310;
                    break;
                case 1310:
                    SetText("Machen wir noch so eine Zahl");
                    _audioSource.PlayOneShot(MachenWirNochSoEineZahl);
                    _completedLevel3++;
                    _currentNumber = NumberGenerator.GetRandom(3);
                    _currentGameStage = 1100;
                    break;
                case 9999:
                    //SceneManager.LoadSceneAsync("Menu");
                    GetComponent<SceneLoader>().LoadScene("Menu");
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

    private void spawnNumbers(int number) => Spawner.SpawnNumber(IntToDigits(number));

    private void ResetStage()
    {
        Spawner.DespawnNumbers();
        _recognizedNumber = 0;
    }

    // to catch the events invoked by WIT
    public bool GotResponse { get; set; }
    public bool GotError { get; set; }
    public bool GotAborting { get; set; }
    public bool GotAborted { get; set; }
    public bool GotRequestCompleted { get; set; }

    private void WaitForUtterance(int success, int failure, int notRecognized)
    {
        if (GotError)
        {
            SetText("Error");
            GotError = false;
            return; 
        }

        if (GotAborting)
        {
            SetText("Aborting");
            GotAborting = false;
            return;
        }

        if (GotAborted)
        {
            SetText("Aborted");
            GotAborted = false;
            return;
        }

        if (GotRequestCompleted) // this means a request was completed
        {
            SetText("Request Completed");
            if (_recognizedNumber != 0 && _recognizedNumber == _currentNumber)
            {
                _currentGameStage = success;
            }
            else if (_recognizedNumber != 0 && _recognizedNumber != _currentNumber)
            {
                _currentGameStage = failure;
            } else
            {
                _currentGameStage = notRecognized; // in any other case what was said was not a recognizable number (e.g. hello)
            }
            GotRequestCompleted = false;
            return;
        }

        /*if (!_isListening)
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
        }*/
    }

}
