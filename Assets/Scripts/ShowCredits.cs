using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enable/Disable the credits
/// </summary>
public class ShowCredits : MonoBehaviour
{
    public void ToggleCredits(GameObject _credits)
    {
        if (!_credits.activeSelf)
            _credits.SetActive(true);
        else
            _credits.SetActive(false);
    }
}
