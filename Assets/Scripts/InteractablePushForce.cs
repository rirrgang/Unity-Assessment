using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePushForce : MonoBehaviour, IInteractable
{

    public ParticleSystem fireEffect;

    public GameObject mainCamera;
    public float force = 2f;
    public void Interact(IInteractor interactor)
    {
        Rigidbody rb = transform.gameObject.GetComponent<Rigidbody>();
        rb.velocity = mainCamera.transform.forward * force;
    }

}
