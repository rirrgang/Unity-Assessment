using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 thirdPersonOffset = new Vector3(0f, 5f, 0f);
    public Vector3 firstPersonOffset = new Vector3(0f, 0.75f, 0f);
    public float sensitivity = 500f;
    public float transitionDuration = 0.25f; // Duration of the smooth transition

    private float currentX = 0f;
    private float currentY = 0f;
    public float Y_ANGLE_MIN = -90f;
    public float Y_ANGLE_MAX = 90f;

    public bool isFirstPerson = false;
    private bool isTransitioning = false; // To check if a transition is already happening


    void Start()
    {
        currentX = transform.rotation.eulerAngles.y;
        currentY = transform.rotation.eulerAngles.x;
        toggleMouseCursor();
    }

    void calculateCameraMovement()
    {
        // Always read the mouse input but apply it based on the camera mode.
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        // Apply mouse movement to camera rotation if in first person
        // or if the right mouse button is held down in third person.
        if (isFirstPerson || (!isFirstPerson && Input.GetMouseButton(1)))
        {
            currentX += mouseX;
            currentY -= mouseY;
            currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse4) && !isTransitioning) // Check if not already transitioning
        {
            StartCoroutine(TransitionCamera()); // Start the transition
        }

        calculateCameraMovement();
    }

    IEnumerator TransitionCamera()
    {
        isTransitioning = true; // Transition is starting

        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;
        Vector3 endPosition;
        Quaternion endRotation;

        if (isFirstPerson)
        {
            // Transitioning to third-person
            endPosition = target.position - transform.forward * thirdPersonOffset.z; // Apply backward offset only
            endRotation = Quaternion.LookRotation(target.position - endPosition);
        }
        else
        {
            // Transitioning to first-person
            endPosition = target.position + target.TransformDirection(firstPersonOffset);
            endRotation = Quaternion.Euler(currentY, currentX, 0);
        }

        isFirstPerson = !isFirstPerson; // Toggle the view
        toggleMouseCursor();

        float timeElapsed = 0f;
        while (timeElapsed < transitionDuration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, timeElapsed / transitionDuration);
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, timeElapsed / transitionDuration);
            timeElapsed += Time.deltaTime;
            yield return null; // Wait until next frame
        }

        transform.position = endPosition; // Ensure the position is set precisely at the end of transition
        transform.rotation = endRotation; // Ensure the rotation is set precisely at the end of transition

        isTransitioning = false; // Transition is complete
    }

    void toggleMouseCursor()
    {
        if (isFirstPerson)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }

        // Hide the cursor
        Cursor.visible = !isFirstPerson;
    }

    void LateUpdate()
    {
        if (!isTransitioning)
        {
            // Only update camera position and rotation if not transitioning
            if (isFirstPerson)
            {
                // First-person view: Set camera to first-person offset
                transform.position = target.position + target.TransformDirection(firstPersonOffset);
                transform.rotation = Quaternion.Euler(currentY, currentX, 0);
            }
            else
            {
                // Third-person view: Calculate rotation and apply offset
                Vector3 dir = new Vector3(0, 0, -thirdPersonOffset.magnitude);
                Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
                transform.position = target.position + rotation * dir;
                transform.LookAt(target.position);
            }
        }
    }
}