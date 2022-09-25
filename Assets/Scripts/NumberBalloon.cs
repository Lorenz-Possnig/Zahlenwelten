using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NumberBalloon : MonoBehaviour
{
    [SerializeField]
    public byte Value;

    [SerializeField]
    private AudioClip Pop;

    [SerializeField]
    private AudioClip Success;

    [SerializeField]
    private AudioSource AudioSource;

    [SerializeField]
    private GameObject GrabbableParent;

    [SerializeField, Interface(typeof(IInteractableView))]
    private MonoBehaviour interactableView;

    [SerializeField]
    private float floatSpeed;

    public UnityEvent DuplicateEvent;

    public UnityEvent OnGrabStop;

    private IInteractableView InteractableView;

    private InteractableState prevState;

    private bool _placedCorrectly = false;

    private bool _markedForDeletion = false;

    public void MarkForDeletion()
    {
        _markedForDeletion = true;
    }

    public void DeleteIfMarked()
    {
        if (_markedForDeletion)
            Destroy(this.transform.parent.gameObject);
    }

    public void OnTriggerEnter(Collider other)
    {
        /*Debug.Log("Zahlenwelten [NumberBalloon]: OnTriggerEnter");
        if (other.CompareTag("brettl"))
        {
            Brettl b = other.GetComponent<Brettl>();
            if (b.ReferenceDigit == Value)
            {
                this.CorrectNumberEvent();
            } else
            {
                this.WrongNumberEvent();
            }
        } else if (other.CompareTag("hand"))
        {
            Debug.Log("Zahlenwelten [NumberBalloon]: Duplicate");
            //StartCoroutine(this.Duplicate());
        }*/
    }

    public void OnTriggerExit(Collider other)
    {
        Debug.Log("Zahlenwelten [NumberBalloon]: OnTriggerExit");
        //this.LetGoEvent();
    }
    
 
    public void CorrectNumberEvent()
    {
        Debug.Log("Zahlenwelten [NumberBalloon]: Correct Number Event; Reference Number: " + Value);
        // snap into place here
        this._placedCorrectly = true;
        DisableGrab();
        MarkForDeletion();
        this.AudioSource.PlayOneShot(this.Success);
        DuplicateEvent.Invoke();
    }

    public void WrongNumberEvent()
    {
        Debug.Log("Zahlenwelten [NumberBalloon]: Wrong Number Event Reference Number: " + Value);
        DuplicateEvent.Invoke();
        this.AudioSource.PlayOneShot(this.Pop);
        
        Destroy(this.transform.parent.gameObject, 0.023f);
    }


    public void LetGoEvent()
    {
        if (!_placedCorrectly)
            StartCoroutine(FloatAwayCoroutine());
    }

    private void SetGrabbable(bool b)
    {
        this.GrabbableParent.gameObject.GetComponent<Grabbable>().enabled = b;
        this.GrabbableParent.gameObject.GetComponent<HandGrabInteractable>().enabled = b;
    }

    public void EnableGrab() => SetGrabbable(true);

    public void DisableGrab() => SetGrabbable(false);


    private IEnumerator FloatAwayCoroutine()
    {
        float inTime = 5f;
        Vector3 frompos = transform.position;
        Vector3 endpos = transform.up * 5f;

        for (float t = 0.06f; t >= 0; t += Time.deltaTime / inTime)
        {
            transform.position += Vector3.up * floatSpeed;
            yield return null;
        }

        Destroy(this.transform.parent.gameObject, 0.023f);
        yield return null;
    }

    private void HandleGrabStateChange()
    {
        var currentState = InteractableView.State;
        // from select to hover || normal
        if (prevState == InteractableState.Select &&
            (currentState == InteractableState.Normal || currentState == InteractableState.Hover))
            OnGrabStop.Invoke();
            
        prevState = InteractableView.State;
    }

    private void Awake()
    {
        InteractableView = interactableView as IInteractableView;
        prevState = InteractableView.State;
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        HandleGrabStateChange();
    }
}
