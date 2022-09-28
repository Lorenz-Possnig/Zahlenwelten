using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCredits : MonoBehaviour
{
    //[SerializeField]
    //private GameObject _credits;

    public void ToggleCredits(GameObject _credits)
    {
        if (!_credits.activeSelf)
            _credits.SetActive(true);
        else
            _credits.SetActive(false);
    }
}
