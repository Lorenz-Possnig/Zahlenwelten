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
    private Dictionary<int, GameStage> gameStateGraph = new Dictionary<int, GameStage>();

    [SerializeField]
    private TextMeshProUGUI SpeechBubble;

    [SerializeField]
    private AudioClip hello;

    private Brettl[] _brettln;

    private int _currentStage = 100;
    private Task _currentTask;

    private int _currentNumber;

    private void Awake()
    {
        findBrettln();
        setupBrettln();

        gameStateGraph.Add(100, new GameStage(200, TextStageWithAudio("Hallo! Ich zeige dir jetzt, wie du dich in unserer Zahlenwelt zurecht findest.", hello, after: 5f)));
        gameStateGraph.Add(200, new GameStage(300, TextStage("Wenn du nach links schaust, siehst du dort ein Regal aus Holz. Dort sind bunte Zahlenballone.", after: 1f)));
        gameStateGraph.Add(300, new GameStage(400, TextStage("Mit denen kannst du dir später Zahlen bauen, indem du die Ballons in der richtigen Reihenfolge auf die schwebenden Holzbretter in der Mitte ziehst.", after: 1f)));
        gameStateGraph.Add(400, new GameStage(500, TextStage("Wenn du nach rechts schaust, siehst du dort ein Schreibbrett - da werden wir dann deine richtigen Zahlen hinschreiben!", after: 1f)));
        gameStateGraph.Add(500, new GameStage(600, TextStage($"Probieren wir es mal mit der folgenden Zahl: {_currentNumber}!", after: 1f)));
        gameStateGraph.Add(600, new GameStage(700, TextStage($"Nimm bitte den Zahlenballon mit der {_currentNumber} aus dem Regal und lass ihn über dem Holzbrett in der Mitte los.", after: 1f)));
        gameStateGraph.Add(700, new GameStage(900, PlaceNumbersStage(800))); // if users places wrong number go to stage 800 else 900
        gameStateGraph.Add(800, new GameStage(700, MultiStageText(new[] { "Das hat nicht so ganz geklappt.", "Probieren wir es nochmal!" })));
        gameStateGraph.Add(900, new GameStage(1000, TextStage("Nice!")));
    }

    void Start()
    {
        _currentTask = new Task(gameStateGraph[_currentStage].Coroutine);
        _currentTask.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_currentTask.Running)
        {
            _currentStage = gameStateGraph[_currentStage].NextStage;
            _currentTask = new Task(gameStateGraph[_currentStage].Coroutine);
            _currentTask.Start();
        }
    }

    private IEnumerator PlaceNumbersStage(int fail)
    {
        for (;;)
        {
            if (_brettln.Any(b => b.WrongTry))
            {
                Debug.Log("Zahlenwelten [GameStateManager]: wrong try");
                _currentTask = new Task(gameStateGraph[fail].Coroutine);
            }

            if (_brettln.All(b => b.Correct))
            {
                _currentTask.Stop();
            }

            yield return new WaitForEndOfFrame();
        }
    }

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
            yield return new WaitForSeconds(0);
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
