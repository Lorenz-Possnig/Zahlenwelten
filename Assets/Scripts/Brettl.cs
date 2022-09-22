using UnityEngine;

public class Brettl : MonoBehaviour
{
    public byte ReferenceDigit { get; set; } = 0;

    public bool Correct { get; set; } = false;

    public bool WrongTry { get; set; } = false;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Zahlenwelten [Brettl]: OnTriggerEnter");
        if (other.gameObject.CompareTag("numberBalloon"))
        {
            NumberBalloon balloon = other.gameObject.GetComponent<NumberBalloon>();

            if (this.ReferenceDigit == balloon.Value)
            {
                Correct = true;
            } else
            {
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