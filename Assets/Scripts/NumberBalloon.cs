using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class NumberBalloon : MonoBehaviour
{
    #region Properties

    public byte Value { get => _value; set => _value = value; }

    #endregion Properties

    #region Fields

    [SerializeField]
    private byte _value;
    [SerializeField]
    public AudioClip _failureSound;
    [SerializeField]
    public AudioClip _successSound;
    [SerializeField]
    public AudioSource _audioSource;
    [SerializeField]
    [Tooltip("The grabbable parent of the balloon to be destroyed")]
    public GameObject _grabbableParent;

    [SerializeField, Interface(typeof(IInteractableView))]
    private MonoBehaviour _interactableView;

    [SerializeField]
    private float _floatSpeed;

    #endregion Fields

    public UnityEvent DuplicateEvent;

    public UnityEvent OnGrabStop;

    private IInteractableView InteractableView;

    private InteractableState prevState;

    private bool _placedCorrectly = false;

    private bool _markedForDeletion = false;

    #region Events

    /// <summary>
    /// Called when a number is placed correctly.
    /// 
    /// Make it unable to be grabbed
    /// Mark for deletion
    /// Play success sound
    /// Duplicate at original position
    /// </summary>
    public void CorrectNumberEvent()
    {
        this._placedCorrectly = true;
        DisableGrab();
        MarkForDeletion();
        this._audioSource.PlayOneShot(_successSound);
        DuplicateEvent.Invoke();
    }

    /// <summary>
    /// Called when the number is placed incorrectly.
    /// A number may enter a brettl again after it has been placed correctly since the
    /// brettl floats but the numbers don't
    /// </summary>
    public void WrongNumberEvent()
    {
        if (!_placedCorrectly)
        {
            DuplicateEvent.Invoke();
            _audioSource.PlayOneShot(_failureSound);
            Destroy(transform.parent.gameObject, 0.023f);
        }
    }

    /// <summary>
    /// Called when a balloon is let go after it has been grabbed
    /// </summary>
    public void LetGoEvent()
    {
        if (!_placedCorrectly)
            StartCoroutine(FloatAwayCoroutine());
    }

    #endregion Events

    #region PublicMethods

    /// <summary>
    /// Mark the balloon for deletion, to get rid of it once all numbers has been filled out
    /// </summary>
    public void MarkForDeletion()
    {
        _markedForDeletion = true;
    }

    /// <summary>
    /// Delete the grabbable if it has been marked
    /// </summary>
    public void DeleteIfMarked()
    {
        if (_markedForDeletion)
            Destroy(this.transform.parent.gameObject);
    }

    #endregion PublicMethods

    #region PrivateMethods

    /// <summary>
    /// Enable / disable grabbing of the balloon
    /// </summary>
    /// <param name="b"></param>
    private void SetGrabbable(bool b)
    {
        _grabbableParent.gameObject.GetComponent<Grabbable>().enabled = b;
        _grabbableParent.gameObject.GetComponent<HandGrabInteractable>().enabled = b;
    }

    /// <summary>
    /// Enable grabbing
    /// </summary>
    public void EnableGrab() => SetGrabbable(true);

    /// <summary>
    /// Disable grabbing
    /// </summary>
    public void DisableGrab() => SetGrabbable(false);

    /// <summary>
    /// Move the balloon up along the y axis
    /// </summary>
    /// <returns></returns>
    private IEnumerator FloatAwayCoroutine()
    {
        float inTime = 5f;

        for (float t = 0.06f; t >= 0; t += Time.deltaTime / inTime)
        {
            transform.position += Vector3.up * _floatSpeed;
            yield return null;
        }

        Destroy(transform.parent.gameObject, 0.023f);
        yield return null;
    }

    /// <summary>
    /// Handle a change in grab state
    /// Only the change from select to hover or normal is needed and implemented
    /// </summary>
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
        InteractableView = _interactableView as IInteractableView;
        prevState = InteractableView.State;
    }

    #endregion PrivateMethods

    void Update()
    {
        HandleGrabStateChange();
    }
}
