using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void Interact(IInteractor interactor);
}

public interface IInteractor
{
    void PerformInteraction();
}
