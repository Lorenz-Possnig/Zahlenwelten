using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Base class for GameStateManagers, holds some common variables and methods
/// </summary>
public class SimpleGameStateManager : MonoBehaviour
{

    [SerializeField]
    protected float _charDelay = 0;

    [SerializeField]
    protected float _afterTextDelay = 0;

    #region AudioClips

    [SerializeField]
    protected AudioSource _audioSource;

    [SerializeField]
    protected TextMeshProUGUI _speechBubble;

    [SerializeField]
    protected AudioClip ProbierenWirEsNochEinmal;
    
    [SerializeField]
    protected AudioClip DasHastDuGutGemacht;
    
    [SerializeField]
    protected AudioClip WagenWirUnsAn;
    
    [SerializeField]
    protected AudioClip MachenWirNochSoEineZahl;
    
    [SerializeField]
    protected AudioClip WagenWirUnsAnDieGanzGrossen;

    #endregion AudioClips

    protected int _currentGameStage = -1; // DEBUG: default -1, zahlenlegen: 400
    protected bool _isWaiting = false;
    protected int _currentNumber;

    protected bool _completedLevel2 = false;
    protected byte _completedLevel3 = 1;

    protected bool _shouldStartWrite = true;

    protected virtual void Start()
    {
        
    }

    protected virtual void Awake()
    {
        
    }

    protected virtual void Update()
    {
        
    }

    /// <summary>
    /// Show text on the speech bubble
    /// </summary>
    /// <param name="text"></param>
    protected void SetText(string text)
    {
        /*if (_shouldStartWrite)
        {
            _speechBubble.text = "";
            StartCoroutine(WriteText(text));
        }*/
        if (_speechBubble.text != text)
        {
            _speechBubble.text = text;
        }
    }

    private IEnumerator WriteText(string text)
    {
        _shouldStartWrite = false;
        _isWaiting = true;
        foreach(char c in text)
        {
            _speechBubble.text += c;
            yield return new WaitForSeconds(_charDelay);
        }
        yield return new WaitForSeconds(_afterTextDelay);
        _isWaiting = false;
        _shouldStartWrite = true;
        yield return null;
    }

    /// <summary>
    /// Wait for specified number of seconds
    /// </summary>
    /// <param name="seconds"></param>
    /// <returns></returns>
    protected IEnumerator Wait(float seconds)
    {
        _isWaiting = true;
        for (int i = 0; i < 2; i++)
        {
            if (i == 0)
            {
                yield return new WaitForSeconds(seconds);
            }
            _isWaiting = false;
            yield return null;
        }
    }
}
