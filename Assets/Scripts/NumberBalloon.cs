using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberBalloon : MonoBehaviour
{
    [SerializeField]
    public byte value;

    public void Pop()
    {
        //Debug.Log("X Pop " + value);
        //.Log($"Pop: {value}");
        this.GetComponent<ChangeToRed>().ChangeToRedMethod();
        //this.gameObject.SetActive(false);
    }
    public void Yey()
    {
        //Debug.Log("X YEY " + value);
        //.Log($"Yey: {value}");
        this.GetComponent<ChangeToGreen>().ChangeToGreenMethod();
    }

    private void GetLost()
    {
        //Debug.Log("X GetLost " + value);
        //.Log($"GetLost: {value}");
        this.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("X Start NumberBalloon Script" + value);
        //.Log($"Start: {value}");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
