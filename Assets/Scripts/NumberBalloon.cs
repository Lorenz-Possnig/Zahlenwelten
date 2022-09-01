using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public bool PlacedCorrectly { get; set; }

    public bool IsInTrigger { get; set; }

    public void OnManipulationStop()
    {
        if (IsInTrigger)
        {
            if (PlacedCorrectly)
            {
                this.CorrectNumberEvent();
            }
            else
            {
                this.WrongNumberEvent();
            }
        } else
        {
            this.LetGoEvent();
        }
    }

    public void OnManipulationStart()
    {
        Debug.Log("Zahlenwelten [NumberBalloon]: OnManipulationStart");
        Instantiate(this.gameObject, transform.position, transform.rotation, transform.parent);
        /// TODO: maybe give the newly created game object a new colour?
        /// Question is if it would be better so set the balloons colour in the start method instead
    }
    
 
    public void CorrectNumberEvent()
    {
        Debug.Log("Zahlenwelten [NumberBalloon]: Correct Number Event; Reference Number: " + Value);
        // snap into place here
        this.AudioSource.PlayOneShot(this.Success);
    }

    public void WrongNumberEvent()
    {
        Debug.Log("Zahlenwelten [NumberBalloon]: Wrong Number Event Reference Number: " + Value);
        gameObject.SetActive(false);
        this.AudioSource.PlayOneShot(this.Pop);
        Destroy(gameObject, 5); //destroy after 5 seconds
    }

    public void LetGoEvent()
    {
        StartCoroutine(floatAway());
    }

    private IEnumerator floatAway()
    {
        float inTime = 5f;
        Vector3 frompos = transform.position;
        Vector3 endpos = transform.up * 5f;

        for (float t = 5f; t >= 0; t += Time.deltaTime / inTime)
        {
            transform.position = Vector3.Lerp(frompos, endpos, t);
            yield return null;
        }

        Destroy(this);
    }

    /// <summary>
    /// set the audio source of GO
    /// TODO: possibly set the material to a prepared 
    /// </summary>
    /// <returns></returns>
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
}
