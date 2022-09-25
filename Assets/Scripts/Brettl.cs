using UnityEngine;

public class Brettl : MonoBehaviour
{
    public byte ReferenceDigit { get; set; } = 0;

    public bool IsActive = false;

    public bool Correct = false;

    public bool WrongTry = false;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Zahlenwelten [Brettl]: OnTriggerEnter");
        if (other.gameObject.CompareTag("numberBalloon") && IsActive)
        {
            NumberBalloon balloon = other.gameObject.GetComponent<NumberBalloon>();

            if (this.ReferenceDigit == balloon.Value)
            {
                //balloon.DisableGrab();
                balloon.CorrectNumberEvent();
                IsActive = false;
                Correct = true;
            } else
            {
                // why was this here?
                balloon.WrongNumberEvent();
                WrongTry = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Zahlenwelten [Brettl]: OnTriggerExit");
        if (other.gameObject.CompareTag("numberBalloon"))
        {
            Correct = false;
        }
    }

    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {

    }
}