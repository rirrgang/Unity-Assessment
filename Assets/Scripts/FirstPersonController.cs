using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public float interactionDistance = 3.0f;
    public float walkSpeed = 5.0f;
    public float runSpeed = 10.0f;
    public float jumpForce = 5.0f;
    public bool isGrounded;
    private Rigidbody rb;
    private CapsuleCollider col;
    public Camera playerCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        // Ensure the Rigidbody doesn't rotate
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, col.bounds.extents.y + 0.1f);

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Use the camera's directions for movement
        Vector3 cameraForward = playerCamera.transform.forward;
        Vector3 cameraRight = playerCamera.transform.right;

        // Flatten the movement vectors on the XZ plane
        cameraForward.y = 0;
        cameraRight.y = 0;

        // Normalize the vectors to have a uniform speed in all directions
        cameraForward.Normalize();
        cameraRight.Normalize();

        // Calculate movement direction relative to camera's orientation
        Vector3 movement = cameraRight * moveHorizontal + cameraForward * moveVertical;

        // Run or walk
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        // Move the player
        rb.MovePosition(rb.position + movement * currentSpeed * Time.deltaTime);

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
