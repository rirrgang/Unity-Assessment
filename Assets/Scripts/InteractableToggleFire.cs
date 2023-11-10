using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableToggleFire : MonoBehaviour, IInteractable
{
    public ParticleSystem fireEffect;

    void Start()
    {
        fireEffect.Stop();
    }

    public void Interact(IInteractor interactor)
    {
        if (fireEffect)
        {
            if (fireEffect.isPlaying)
            {
                fireEffect.Stop();
            }
            else
            {
                fireEffect.Play();
            }
        }
    }
}
