using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class SimpleGameStateManager : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private TextMeshProUGUI SpeechBubble;

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
    private AudioClip ProbierenWirEsNochEinmal;

    [SerializeField]
    private AudioClip DasHastDuGutGemacht;

    [SerializeField]
    private AudioClip WagenWirUnsAn;

    [SerializeField]
    private AudioClip ProbierenWirDieZahl;

    [SerializeField]
    private AudioClip MachenWirNochSoEineZahl;

    [SerializeField]
    private AudioClip WagenWirUnsAnDieGanzGrossen;
    #endregion Fields

    public AudioClip[] Numbers;

    public FadeToBlack fade;

    public Brettl[] _brettln;

    private int _currentGameStage = 0;
    private int _currentNumber;

    private bool _completedLevel2 = false;
    private byte _completedLevel3 = 0;

    private bool _isWaiting = false;

    private void Awake()
    {
        setupBrettln();
    }

    void Update()
    {
        if (!audioSource.isPlaying && !_isWaiting)
        {
            switch (_currentGameStage)
            {
                // Intro
                case -1:
                    _isWaiting = true;
                    StartCoroutine(Wait(4));
                    _currentGameStage = 0;
                    break;
                case 0: 
                    setText("Hallo! Ich zeige dir jetzt, wie du dich in unserer Zahlenwelt zurecht findest.");
                    audioSource.PlayOneShot(HalloIchZeigeDir);
                    _currentGameStage = 100;
                    break;
                case 100:
                    setText("Wenn du nach links schaust, siehst du dort ein Regal aus Holz. Dort sind bunte Zahlenballone.");
                    _currentGameStage = 200;
                    break;
                case 200:
                    setText("Mit denen kannst du dir Zahlen bauen, indem du die Ballons in der richtigen Reihenfolge auf die schwebenden Holzbretter in der Mitte ziehst.");
                    _currentGameStage = 400;
                    break;
                // First Level
                case 400:
                    setText($"Probieren wir es mal mit der folgenden Zahl");
                    audioSource.PlayOneShot(ProbierenWirEsMalMit);
                    _currentGameStage = 410;
                    break;
                case 410:
                    audioSource.PlayOneShot(Numbers[_currentNumber]);
                    _currentGameStage = 500;
                    break;
                case 500:
                    setText($"Nimm bitte den Zahlenballon aus dem Regal und halte ihn über das Holzbrett.");
                    audioSource.PlayOneShot(NimmBitteDenBallon);
                    setBrettlnActive();
                    _currentGameStage = 501;
                    break;
                case 501:
                    audioSource.PlayOneShot(Numbers[_currentNumber]);
                    _currentGameStage = 502;
                    break;
                case 502:
                    audioSource.PlayOneShot(AusDemRegal);
                    _currentGameStage = 510;
                    break;
                case 510:
                    resetBrettl();
                    _currentGameStage = 600;
                    break;
                case 520:
                    setText("Probieren wir es noch einmal.");
                    audioSource.PlayOneShot(ProbierenWirEsNochEinmal);
                    _currentGameStage = 500;
                    break;
                case 600:
                    waitForNumbers(700, 520);
                    break;
                case 700:
                    setText("Das hast du gut gemacht!");
                    audioSource.PlayOneShot(DasHastDuGutGemacht);
                    enableBrettl(1);
                    resetBrettl();
                    setupBrettln();
                    setBrettlnInactive();
                    _currentGameStage = 800;
                    break;
                // Second Level
                case 800:
                    setText("Wagen wir uns an eine größere Zahl heran!");
                    audioSource.PlayOneShot(WagenWirUnsAn);
                    resetBrettl();
                    _currentGameStage = 900;
                    break;
                case 900:
                    setText($"Probieren wir die Zahl:");
                    setBrettlnActive();
                    audioSource.PlayOneShot(ProbierenWirDieZahl);
                    _currentGameStage = 910;
                    break;
                case 910:
                    audioSource.PlayOneShot(Numbers[_currentNumber]);
                    _currentGameStage = 1000;
                    break;
                case 1000:
                    waitForNumbers(1100, 1010);
                    break;
                case 1010:
                    setText("Probieren wir es nocheinmal");
                    audioSource.PlayOneShot(ProbierenWirEsNochEinmal);
                    setBrettlnActive();
                    resetBrettl();
                    _currentGameStage = 900;
                    break;
                case 1100:
                    setText("Das hast du gut gemacht");
                    audioSource.PlayOneShot(DasHastDuGutGemacht);
                    setBrettlnInactive();
                    _currentGameStage = _completedLevel2 ? 1300 : 1200;
                    break;
                case 1200:
                    setText("Machen wir noch so eine Zahl");
                    audioSource.PlayOneShot(MachenWirNochSoEineZahl);
                    resetBrettl();
                    setupBrettln();
                    _completedLevel2 = true;
                    _currentGameStage = 900;
                    break;
                // Third level
                case 1300:
                    setText("Wagen wir uns an die ganz großen Zahlen!");
                    audioSource.PlayOneShot(WagenWirUnsAnDieGanzGrossen);
                    enableBrettl(2);
                    resetBrettl();
                    setupBrettln();
                    setBrettlnActive();
                    _currentGameStage = 1400;
                    break;
                case 1400:
                    audioSource.PlayOneShot(ProbierenWirDieZahl);
                    _currentGameStage = 1410;
                    break;
                case 1410:
                    audioSource.PlayOneShot(Numbers[_currentNumber]);
                    _currentGameStage = 1500;
                    break;
                case 1500:
                    waitForNumbers(1600, 1510);
                    break;
                case 1510:
                    setText("Probieren wir es noch einmal");
                    audioSource.PlayOneShot(ProbierenWirEsNochEinmal);
                    resetBrettl();
                    setBrettlnActive();
                    _currentGameStage = 1410;
                    break;
                case 1600:
                    setText("Gut gemacht!");
                    audioSource.PlayOneShot(DasHastDuGutGemacht);
                    setBrettlnActive();
                    _completedLevel3++;
                    _currentGameStage = _completedLevel3 >= 3 ? 9999 : 1610;
                    break;
                case 1610:
                    setText("Machen wir noch so eine Zahl");
                    audioSource.PlayOneShot(MachenWirNochSoEineZahl);
                    resetBrettl();
                    setupBrettln();
                    _currentGameStage = 1400;
                    break;
                case 1700:
                    setText("Das hast du sehr gut gemacht!");
                    audioSource.PlayOneShot(DasHastDuGutGemacht);
                    _currentGameStage = 9999;
                    break;
                case 9999:
                    Application.Quit();
                    break;
                default:
                    break;
            }
        }
    }

    private IEnumerator Wait(float seconds)
    {
        for(int i = 0; i < 2; i++)
        {
            if(i == 0)
            {
                yield return new WaitForSeconds(seconds);
            }
            _isWaiting = false;
            yield return null;
        }
    }
    private void waitForNumbers(int success, int fail)
    {
        var brettlState = checkBrettln();
        switch (brettlState)
        {
            case BrettlState.WRONG:
                _currentGameStage = fail;
                break;
            case BrettlState.CORRECT:
                _currentGameStage = success;
                break;
            case BrettlState.EMPTY:
                //Debug.Log("Zahlenwelten [SimpleGameStateManager]: Brettln are empty");
                break;
        }
    }

    private void setBrettlnActiveState(bool b)
    {
        foreach (var brettl in _brettln)
            if (brettl.gameObject.activeSelf)
                brettl.IsActive = b;
    }

    private void setBrettlnActive() => setBrettlnActiveState(true);

    private void setBrettlnInactive() => setBrettlnActiveState(false);

    private void enableBrettl(byte index)
    {
        _brettln[index].gameObject.SetActive(true);
    }

    private void resetBrettl()
    {
        foreach (var brettl in _brettln)
        {
            brettl.WrongTry = false;
            brettl.Correct = false;
            //foreach(Transform child in brettl.transform) --> not working, left for future reference
            //{
            //    Destroy(child.gameObject);
            //}
        }
        foreach (var balloon in
            GameObject.FindGameObjectsWithTag("numberBalloon"))
        {
            balloon.GetComponent<NumberBalloon>().DeleteIfMarked();
        }
    }

    private void setText(string text)
    {
        SpeechBubble.text = text;
    }

    private enum BrettlState
    {
        CORRECT,
        WRONG,
        EMPTY
    }

    private BrettlState checkBrettln()
    {
        if (_brettln.Where(b => b.IsActive).All(b => b.Correct)) { 
            return BrettlState.CORRECT; 
        }
        if (_brettln.Where(b => b.IsActive).Any(b => b.WrongTry)) { 
            return BrettlState.WRONG; 
        }
        return BrettlState.EMPTY;
    }

    private void setupBrettln()
    {
        //Debug.Log("Zahlenwelten [GameStateManager]: Brettln Setup");
        int level = _brettln.Where(b => b.gameObject.activeSelf).Count();
        _currentNumber = NumberGenerator.GetRandom(level);
        byte[] digits = _currentNumber
            .ToString()
            .ToCharArray()
            .Select(x => x.ToString())
            .Select(byte.Parse)
            .ToArray();

        for (int i = 0; i < level; i++)
            _brettln[i].ReferenceDigit = digits[i];
    }
}
