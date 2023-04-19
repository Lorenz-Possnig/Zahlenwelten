using System.Collections;
using UnityEngine;

public class Brettl : MonoBehaviour
{
    public byte ReferenceDigit { get => referenceDigit; set => referenceDigit = value; }
    public byte referenceDigit = 0;
    public bool active = true;
    public bool IsActive { get => active; set => active = value; }
    public bool correct = false;
    public bool Correct { get => correct; set => correct = value; }
    public bool wrongTry = false;
    public bool WrongTry { get => wrongTry; set => wrongTry = value; }

    public bool IsEmpty() => !correct && !wrongTry;

    public Brettl Predecessor;
    public Brettl Successor;

    [SerializeField]
    private ParticleSystem _psSuccess;

    [SerializeField]
    private ParticleSystem _psFail;

    [SerializeField]
    private Color StartColor;
    [SerializeField]
    private Color EndColor;
    [SerializeField]
    private float CycleTime;

    private bool goingForward = false;
    public bool isBlinking;
    private Material Material;
    private bool coroutineStarted = false;

    public void StartBlink()
    {
        goingForward = false;
        isBlinking = true;
    }

    public void StopBlink()
    {
        isBlinking = false;
        StopAllCoroutines();
        coroutineStarted = false;
        if (Material != null)
        {
            Material.color = StartColor;
        }
    }

    public void Awake()
    {
        goingForward = true;
        isBlinking = false;
        var path = "wood";
        var obj = Resources.Load(path);
        var mat = obj as Material;
        Material = Instantiate(mat); //this.gameObject.GetComponent<Renderer>().material;
        this.gameObject.GetComponent<Renderer>().material = Material;
    }

    public void Update()
    {
        if (isBlinking && !coroutineStarted)
        {
            if (goingForward)
                StartCoroutine(Blink(StartColor, EndColor, CycleTime, Material));
            else
                StartCoroutine(Blink(EndColor, StartColor, CycleTime, Material));
        }
    }

    private IEnumerator Blink(Color startColor, Color endColor, float cycleTime, Material mat)
    {
        coroutineStarted = true;
        for(float currentTime = 0; currentTime < cycleTime; currentTime += Time.deltaTime)
        {
            float t = currentTime / cycleTime;
            Color currentColor = Color.Lerp(startColor, endColor, t);
            mat.color = currentColor;
            yield return null;
        }
        goingForward = !goingForward;
        coroutineStarted = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.NUMBER_BALOON_TAG))
        {
            NumberBalloon balloon = other.gameObject.GetComponent<NumberBalloon>();

            if (IsActive)
            {
                var color = other.GetComponent<Renderer>().material.color;
                var errors = false;
                if (Predecessor != null && Predecessor.IsEmpty())
                {
                    errors = true;
                }
                if (ReferenceDigit != balloon.Value)
                {
                    errors = true;
                }
                if (errors)
                {
                    EmitFailure(color);
                    balloon.WrongNumberEvent();
                    WrongTry = true;
                } else
                { // number was correct
                    balloon.Brettl = this;
                    StopBlink();
                    if (Successor != null)
                    {
                        Successor.StartBlink();
                    }
                }
            }
        }
    }

    void Emit(ParticleSystem ps, Color color, int amount)
    {
        ps.startColor = color;
        ps.GetComponent<Renderer>().material.color = color;
        ps.Emit(amount);
    }

    public void EmitSuccess(Color color) => Emit(_psSuccess, color, 100);

    public void EmitFailure(Color color) => Emit(_psFail, color, 50);

}