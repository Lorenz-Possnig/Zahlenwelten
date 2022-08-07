using UnityEngine;

public class Brettl : MonoBehaviour
{
    public byte referenceDigit;

    private void OnTriggerEnter(Collider other)
    {
        //.Log("OnTriggerEnter");
        if (other.gameObject.CompareTag("numberBalloon"))
        {
            //.Log("Is A Balloon");
            NumberBalloon numberBalloon = other.gameObject.GetComponent<NumberBalloon>();

            if (numberBalloon.value != referenceDigit)
                numberBalloon.Pop();
            else
                numberBalloon.Yey();
        }
    }

    void Awake()
    {
        //Debug.Log("X Awake Brettl Script " + referenceDigit);
        //.Log($"Awake: {referenceDigit}");
    }
    void Start()
    {
        //Debug.Log("X Start Brettl Script " + referenceDigit);
        //.Log($"Start: {referenceDigit}");
    }
    // Update is called once per frame
    void Update()
    {

    }
}