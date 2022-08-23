using UnityEngine;

public class Brettl : MonoBehaviour
{
    public byte ReferenceDigit { get; set; } = 0;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Zahlenwelten [Brettl]: OnTriggerEnter");
        if (other.gameObject.CompareTag("numberBalloon"))
        {
            NumberBalloon balloon = other.gameObject.GetComponent<NumberBalloon>();
            balloon.IsInTrigger = true;
            balloon.ReferenceNumber = this.ReferenceDigit;
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Zahlenwelten [Brettl]: OnTriggerExit");
        if (other.gameObject.CompareTag("numberBalloon"))
        {
            NumberBalloon balloon = other.gameObject.GetComponent<NumberBalloon>();
            balloon.IsInTrigger = false;
            balloon.ReferenceNumber = 0;
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