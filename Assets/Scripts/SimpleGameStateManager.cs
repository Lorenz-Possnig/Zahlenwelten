using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SimpleGameStateManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private TextMeshProUGUI SpeechBubble;

    [SerializeField]
    private AudioClip hello;

    public Brettl[] _brettln;

    private int _currentGameStage = 0;
    private int _currentNumber;

    private void Awake()
    {
        setupBrettln();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            switch (_currentGameStage)
            {
                // greeting
                case 0:
                    setText("Hallo! Ich zeige dir jetzt, wie du dich in unserer Zahlenwelt zurecht findest.");
                    audioSource.PlayOneShot(hello);
                    _currentGameStage = 100;
                    break;
                case 100:
                    setText("Wenn du nach links schaust, siehst du dort ein Regal aus Holz. Dort sind bunte Zahlenballone.");
                    _currentGameStage = 200;
                    break;
                case 200:
                    setText("Mit denen kannst du dir später Zahlen bauen, indem du die Ballons in der richtigen Reihenfolge auf die schwebenden Holzbretter in der Mitte ziehst.");
                    _currentGameStage = 300;
                    break;
                case 300:
                    setText("Wenn du nach rechts schaust, siehst du dort ein Schreibbrett - da werden wir dann deine richtigen Zahlen hinschreiben!");
                    _currentGameStage = 400;
                    break;
                case 400:
                    setText($"Probieren wir es mal mit der folgenden Zahl: {_currentNumber}!");
                    _currentGameStage = 500;
                    break;
                case 500:
                    setText($"Nimm bitte den Zahlenballon mit der {_currentNumber} aus dem Regal und lass ihn über dem Holzbrett in der Mitte los.");
                    setBrettlnActive();
                    _currentGameStage = 510;
                    break;
                case 510:
                    resetBrettl();
                    _currentGameStage = 600;
                    break;
                case 520:
                    setText("Probieren wir es noch einmal");
                    _currentGameStage = 500;
                    break;
                case 600:
                    waitForNumbers(700, 520);
                    break;
                case 700:
                    setText("Das hast du gut gemacht!");
                    enableBrettl(1);
                    resetBrettl();
                    setupBrettln();
                    setBrettlnInactive();
                    _currentGameStage = 800;
                    break;
                case 800:
                    setText("Probieren wir es mit einer größeren Zahl!");
                    _currentGameStage = 900;
                    break;
                case 900:
                    setText($"Probieren wir es mal mit der folgenden Zahl: {_currentNumber}!");
                    setBrettlnActive();
                    resetBrettl();
                    _currentGameStage = 1000;
                    break;
                case 1000:
                    waitForNumbers(1100, 1010);
                    break;
                case 1010:
                    setText("Probieren wir es nocheinmal");
                    _currentGameStage = 900;
                    break;
                case 1100:
                    setText("Gut gemacht");
                    setBrettlnInactive();
                    break;
                default:
                    break;
            }
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
                Debug.Log("Zahlenwelten [SimpleGameStateManager]: Brettln are empty");
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
        }
        foreach (var balloon in
            GameObject.FindGameObjectsWithTag("numberBalloon"))
            balloon.GetComponent<NumberBalloon>().DeleteIfMarked();
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
        Debug.Log("Zahlenwelten [GameStateManager]: Brettln Setup");
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
