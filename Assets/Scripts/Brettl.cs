using UnityEngine;

public class Brettl : MonoBehaviour
{
    public byte referenceDigit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("numberBalloon"))
        {
            NumberBalloon numberBalloon = other.gameObject.GetComponent<NumberBalloon>();

            if (numberBalloon.value != referenceDigit)
                numberBalloon.Pop();
            else
                numberBalloon.Yey();
        }
    }

    void Awake()
    {
        Debug.Log("X Awake Brettl Script " + referenceDigit);
    }
    void Start()
    {
        Debug.Log("X Start Brettl Script " + referenceDigit);
    }
    // Update is called once per frame
    void Update()
    {

    }
}