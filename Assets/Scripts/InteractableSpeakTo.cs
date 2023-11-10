using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractableSpeakTo : MonoBehaviour, IInteractable
{

    public GameObject TextUI;


    public void Interact(IInteractor interactor)
    {
        TextUI.SetActive(true);
    }
}
