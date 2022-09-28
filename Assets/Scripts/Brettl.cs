using UnityEngine;

public class Brettl : MonoBehaviour
{
    public byte ReferenceDigit { get; set; } = 0;
    public bool IsActive { get; set; } = false;
    public bool Correct { get; set; } = false;
    public bool WrongTry { get; set; } = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.NUMBER_BALOON_TAG))
        {
            NumberBalloon balloon = other.gameObject.GetComponent<NumberBalloon>();

            if (IsActive)
            {
                if (ReferenceDigit == balloon.Value)
                {
                    balloon.CorrectNumberEvent();
                    IsActive = false;
                    Correct = true;
                }
                else
                {
                    balloon.WrongNumberEvent();
                    if (IsActive)
                        WrongTry = true;
                }
            }
        }
    }
}