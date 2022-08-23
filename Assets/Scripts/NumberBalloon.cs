using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberBalloon : MonoBehaviour
{
    [SerializeField]
    private byte value;
    
    // set by the brettl GO on trigger enter; reset to 0 on trigger exit
    public byte ReferenceNumber { get; set; } = 0;
    // set by the brettl GO on trigger enter; reset on trigger exit
    public bool IsInTrigger { get; set; } = false;

    [SerializeField]
    private AudioClip Pop;

    [SerializeField]
    private AudioClip Success;
    
    private AudioSource _audioSource;

    public void OnManipulationStop()
    {
        if (IsInTrigger)
        {
            if (this.ReferenceNumber == this.value)
            {
                this.CorrectNumberEvent();
            }
            else
            {
                this.IncorrectNumberEvent();
            }
        }
        else
        {
            this.StoppedManipulationOutsideTriggerEvent();
        }
    }

    public void OnManipulationStart()
    {
        Debug.Log("Zahlenwelten [NumberBalloon]: OnManipulationStart");
        Instantiate(this.gameObject);
        /// TODO: maybe give the newly created game object a new colour?
        /// Question is if it would be better so set the balloons colour in the start method instead
    }
    
    /// <summary>
    /// When a number is placed incorrectly, "pops" the balloon.
    /// I.e. plays a "pop" sound and destroys the GO
    /// </summary>
    /// <returns></returns>
    private void IncorrectNumberEvent()
    {
        Debug.Log("Zahlenwelten [NumberBalloon]: Incorrect Number Event; Reference Number: " + value);
        this._audioSource.PlayOneShot(this.Pop);
        Destroy(this.gameObject);
    }
    
    /// <summary>
    /// When a number is correct, it should snap into place
    /// and play a "Yay"/Success sound.
    /// TODO: Snap into place
    /// </summary>
    /// <returns></returns>
    private void CorrectNumberEvent()
    {
        Debug.Log("Zahlenwelten [NumberBalloon]: Correct Number Event; Reference Number: " + value);
        // snap into place here
        this._audioSource.PlayOneShot(this.Success);
    }

    /// <summary>
    /// When you let go of the balloon outside of the trigger,
    /// it should float away like a helium balloon and despawn when out of sight.
    /// </summary>
    /// <returns></returns>
    // TODO: make the Balloon float away
    private void StoppedManipulationOutsideTriggerEvent()
    {
        Debug.Log("Zahlenwelten [NumberBalloon]: StoppedManipulationOutsideTriggerEvent");
        this.gameObject.SetActive(false);
        // float away here
        Destroy(this.gameObject, 5); //destroy after 5 seconds
    }

    /// <summary>
    /// set the audio source of GO
    /// TODO: possibly set the material to a prepared 
    /// </summary>
    /// <returns></returns>
    void Start()
    {
        this._audioSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
