using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInteractor : MonoBehaviour, IInteractor
{
    public float interactionDistance = 5.0f;
    public TextMeshProUGUI interactionPrompt; // Assign this in the Inspector
    public string interactionKey = "E"; // The key you press to interact

    public LayerMask layerMask;

    private IInteractable currentInteractable;

    private void Start()
    {
        // Hide the interaction prompt at the start
        if (interactionPrompt != null)
            interactionPrompt.gameObject.SetActive(false);
    }

    private void Update()
    {
        // Check for interactable objects in the player's line of sight
        CheckForInteractable();

        // If the player presses the interaction key and there's an interactable object, perform the interaction
        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            PerformInteraction();
        }
    }

    public void PerformInteraction()
    {
        // Perform the interaction with the current interactable object
        currentInteractable.Interact(this);
        // Optionally hide the prompt after interaction
        // interactionPrompt.gameObject.SetActive(false);
    }

    private void CheckForInteractable()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2)); // Center of the screen

        // Perform the raycast to check for interactable objects
        if (Physics.Raycast(ray, out hit, interactionDistance, layerMask))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                // If we hit a new interactable object, update the current interactable reference
                if (interactable != currentInteractable)
                {
                    currentInteractable = interactable;
                    // Display the interaction prompt
                    interactionPrompt.text = $"Press '{interactionKey}' to interact";
                    interactionPrompt.gameObject.SetActive(true);
                }
            }
            else
            {
                // If we're not looking at an interactable object, hide the prompt and clear the current interactable
                if (currentInteractable != null)
                {
                    currentInteractable = null;
                    interactionPrompt.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            // If the raycast didn't hit anything, hide the prompt and clear the current interactable
            if (currentInteractable != null)
            {
                currentInteractable = null;
                interactionPrompt.gameObject.SetActive(false);
            }
        }
    }
}