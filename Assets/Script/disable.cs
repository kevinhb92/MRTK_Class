using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit;

public class disable : MonoBehaviour
{
    public GameObject Before;
    public Interactable disableButton;

    void Start()
    {
        // Add a callback function to the disable button's "OnClick" event
        disableButton.OnClick.AddListener(DisableObject);
    }

    void DisableObject()
    {
        Before.SetActive(false);
    }
}