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

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Zahlenwelten [NumberBalloon]: OnTriggerEnter");
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
            StartCoroutine(this.Duplicate());
        }
    }

    private IEnumerator Duplicate()
    {
        var go = this.gameObject;
        var pos = transform.position;
        var rot = transform.rotation;
        var par = transform.parent;
        for (int i = 0; i < 2; i++)
        {
            if (i == 1)
                Instantiate(go, pos, rot, par);
            yield return new WaitForSeconds(2);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        Debug.Log("Zahlenwelten [NumberBalloon]: OnTriggerExit");
        this.LetGoEvent();
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
