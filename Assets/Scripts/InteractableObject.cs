using UnityEngine;

public class InteractableObject : MonoBehaviour, IInteractable
{

    public void Interact(IInteractor interactor)
    {
        // Define what happens when interaction occurs
        Debug.Log(gameObject.name + " has been interacted with by " + interactor.ToString());

        // change color of the gameobjects material
        Renderer renderer = GetComponent<Renderer>();
        if (renderer)
        {
            renderer.material.color = Color.red;
            // Generate a random color
            Color randomColor = Random.ColorHSV();

            // Change the color of the game object's material
            renderer.material.color = randomColor;
        }
    }
}