using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using System;

public class GameStateManager : MonoBehaviour
{    

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    //private Dictionary<int, GameStage> gameStateGraph = new Dictionary<int, GameStage>();
    private Dictionary<int, IEnumerator> gameStateGraph = new Dictionary<int, IEnumerator>();

    [SerializeField]
    private TextMeshProUGUI SpeechBubble;

    [SerializeField]
    private AudioClip hello;

    private Brettl[] _brettln;

    private int _currentStage = 100;
    private Task _currentTask;

    private int _currentNumber;

    private Coroutine _currentCoroutine;
    private IEnumerator _currentEnumerator;
    private int _nextStage;

    private void Awake()
    {
        findBrettln();
        setupBrettln();

        gameStateGraph.Add(100, TextStage(200, "1"));
        gameStateGraph.Add(200, TextStage(100, "2"));

        /*gameStateGraph.Add(100, new GameStage(200, () => TextStageWithAudio("Hallo! Ich zeige dir jetzt, wie du dich in unserer Zahlenwelt zurecht findest.", hello, after: 2f)));
        gameStateGraph.Add(200, new GameStage(300, () => TextStage("Wenn du nach links schaust, siehst du dort ein Regal aus Holz. Dort sind bunte Zahlenballone.", after: 1f)));
        gameStateGraph.Add(300, new GameStage(400, () => TextStage("Mit denen kannst du dir später Zahlen bauen, indem du die Ballons in der richtigen Reihenfolge auf die schwebenden Holzbretter in der Mitte ziehst.", after: 1f)));
        gameStateGraph.Add(400, new GameStage(500, () => TextStage("Wenn du nach rechts schaust, siehst du dort ein Schreibbrett - da werden wir dann deine richtigen Zahlen hinschreiben!", after: 1f)));
        gameStateGraph.Add(500, new GameStage(600, () => TextStage($"Probieren wir es mal mit der folgenden Zahl: {_currentNumber}!", after: 1f)));
        gameStateGraph.Add(600, new GameStage(610, () => TextStage($"Nimm bitte den Zahlenballon mit der {_currentNumber} aus dem Regal und lass ihn über dem Holzbrett in der Mitte los.", after: 1f)));
        gameStateGraph.Add(610, new GameStage(620, () => SetBrettlnActiveStage()));
        gameStateGraph.Add(620, new GameStage(700, () => ResetBrettlnState()));
        gameStateGraph.Add(700, new GameStage(200, () => PlaceNumbersStage(800))); // if users places wrong number go to stage 800 else 900
        gameStateGraph.Add(800, new GameStage(810, () => TextStage("Das hat nicht so ganz geklappt.")));
        gameStateGraph.Add(810, new GameStage(600, () => TextStage("Probieren wir es noch einmal")));
        gameStateGraph.Add(1000, new GameStage(1000, () => TextStage("Nice!")));*/
    }

    void Start()
    {
        //_currentTask = new Task(gameStateGraph[_currentStage].Coroutine);
        //_currentTask.Start();
        _currentEnumerator = gameStateGraph[_currentStage];
        _currentCoroutine = StartCoroutine(_currentEnumerator);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log($"Zahlenwelten [GameStateLog]: {_currentStage}");
        // when a task has finished
        /*if (_currentTask != null && !_currentTask.Running)
        {
            var oldStage = _currentStage;
            _currentTask.Stop();
            _currentStage = gameStateGraph[_currentStage].NextStage;
            _currentTask = new Task(gameStateGraph[_currentStage].Coroutine);
            _currentTask.Start();
        }*/
        if (!_currentEnumerator.MoveNext()) // if there are no more iterations the current coroutine is done
        {
            _currentEnumerator = gameStateGraph[_nextStage];
            _currentCoroutine = StartCoroutine(_currentEnumerator);
        }
    }

    private IEnumerator ResetBrettlnState()
    {
        var i = true;
        while (i)
        {
            foreach(var brettl in _brettln)
            {
                brettl.WrongTry = false;
                brettl.Correct = false;
            }
            i = false;
            yield return null;
        }
    }

    private IEnumerator SetBrettlnActiveStage()
    {
        var i = true;
        while (i)
        {
            foreach (var brettl in _brettln)
            {
                brettl.IsActive = true;
            }
            i = false;
            yield return null;
        }
    }
    /*
    private IEnumerator PlaceNumbersStage(int fail)
    {
        for (;;)
        {
            if (_brettln.Any(b => b.WrongTry))
            {
                Debug.Log("Zahlenwelten [GameStateManager]: wrong try");
                _currentTask = new Task(gameStateGraph[fail].Coroutine);
                yield break;
            }

            if (_brettln.All(b => b.Correct))
            {
                _currentTask.Stop();
                yield break;
            }

            yield return new WaitForEndOfFrame();
        }
    }*/

    private IEnumerator MultiStageText(IEnumerable<String> texts, float delay = 0.0f, float after = 0.0f)
    {
        foreach (string text in texts)
        {
            SpeechBubble.text = "";
            foreach (char c in text)
            {
                SpeechBubble.text += c;
                yield return new WaitForSeconds(delay);
            }
            yield return new WaitForSeconds(after);
        }
        yield return new WaitForSeconds(after);
    }

    private IEnumerator TextStage(string text, float delay = 0.0f, float after = 0.0f)
    {
        SpeechBubble.text = "";
        foreach (char c in text)
        {
            SpeechBubble.text += c;
            yield return new WaitForSeconds(delay);
        }
        yield return new WaitForSeconds(after);
    }

    private IEnumerator TextStage(int nextStage, string text, float delay = 0.0f, float after = 0.0f)
    {
        _nextStage = nextStage;
        SpeechBubble.text = "";
        foreach (char c in text)
        {
            SpeechBubble.text += c;
            yield return new WaitForSeconds(delay);
        }
        yield return new WaitForSeconds(after);
    }

    private IEnumerator TextStageWithAudio(string text, AudioClip clip, float delay = 0.0f, float after = 0.0f)
    {
        SpeechBubble.text = "";
        audioSource.PlayOneShot(clip);
        foreach (char c in text)
        {
            SpeechBubble.text += c;
            yield return new WaitForSeconds(delay);
        }

        while (audioSource.isPlaying)
        {
            yield return new WaitForSeconds(1);
        }

        yield return new WaitForSeconds(after);
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
