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

    private Brettl[] _brettln;

    private int _currentGameStage = 0;
    private int _currentNumber;

    private IEnumerator _enumerator;
    private Coroutine _coroutine;

    private bool _init = true;

    private void Awake()
    {
        findBrettln();
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
            _init = false; 
            switch (_currentGameStage)
            {
                // greeting
                case 0:
                    SetText("Hallo! Ich zeige dir jetzt, wie du dich in unserer Zahlenwelt zurecht findest.");
                    audioSource.PlayOneShot(hello);
                    _currentGameStage = 100;
                    break;
                case 100:
                    SetText("Wenn du nach links schaust, siehst du dort ein Regal aus Holz. Dort sind bunte Zahlenballone.");
                    _currentGameStage = 200;
                    break;
                case 200:
                    SetText("Mit denen kannst du dir später Zahlen bauen, indem du die Ballons in der richtigen Reihenfolge auf die schwebenden Holzbretter in der Mitte ziehst.");
                    _currentGameStage = 300;
                    break;
                case 300:
                    SetText("Wenn du nach rechts schaust, siehst du dort ein Schreibbrett - da werden wir dann deine richtigen Zahlen hinschreiben!");
                    _currentGameStage = 400;
                    break;
                case 400:
                    SetText($"Probieren wir es mal mit der folgenden Zahl: {_currentNumber}!");
                    _currentGameStage = 500;
                    break;
                case 500:
                    SetText($"Nimm bitte den Zahlenballon mit der {_currentNumber} aus dem Regal und lass ihn über dem Holzbrett in der Mitte los.");
                    _currentGameStage = 510;
                    foreach (var brettl in _brettln)
                        brettl.IsActive = true;
                    break;
                case 510:
                    ResetBrettl();
                    _currentGameStage = 600;
                    break;
                case 600:
                    var brettlState = checkBrettln();
                    switch (brettlState)
                    {
                        case BrettlState.WRONG:
                            SetText($"Das war leider falsch");
                            _currentGameStage = 500;
                            break;
                        case BrettlState.CORRECT:
                            _currentGameStage = 700;
                            break;
                        case BrettlState.EMPTY:
                            break;
                    }
                    break;
                case 700:
                    SetText("Das hast du gut gemacht!");
                    break;
                default:
                    break;
            }
        }
    }

    private void ResetBrettl()
    {
        foreach (var brettl in _brettln)
        {
            brettl.WrongTry = false;
            brettl.Correct = false;
        }
    }

    private void SetText(string text)
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
        if (_brettln.All(b => b.Correct)) { 
            return BrettlState.CORRECT; 
        }
        if (_brettln.Any(b => b.WrongTry)) { 
            return BrettlState.WRONG; 
        }
        return BrettlState.EMPTY;
    }

    private void findBrettln()
    {
        Debug.Log("Zahlenwelten [GameStateManager]: Find Brettln");
        _brettln = GameObject.FindGameObjectsWithTag("brettl")
            .Select(go => go.GetComponent<Brettl>())
            .ToArray();
    }

    private void setupBrettln()
    {
        Debug.Log("Zahlenwelten [GameStateManager]: Brettln Setup");
        int level = _brettln.Count();
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
