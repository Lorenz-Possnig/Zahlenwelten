using UnityEngine;

public class Brettl : MonoBehaviour
{
    public byte ReferenceDigit { get; set; } = 0;
    public bool IsActive = false;
    public bool Correct = false;
    public bool WrongTry = false;

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Zahlenwelten [Brettl]: OnTriggerEnter");
        if (other.gameObject.CompareTag("numberBalloon"))
        {
            NumberBalloon balloon = other.gameObject.GetComponent<NumberBalloon>();

            if (this.ReferenceDigit == balloon.Value && IsActive)
            {
                balloon.CorrectNumberEvent();
                IsActive = false;
                Correct = true;
            } else
            {

                balloon.WrongNumberEvent();
                if (IsActive)
                    WrongTry = true;
            }
        }
    }
}