using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsPlayer : MonoBehaviour
{
    public Transform mainCamera; // Assign the player's transform in the inspector

    void Update()
    {
        if (mainCamera != null)
        {
            Vector3 rotation = Quaternion.LookRotation(mainCamera.forward).eulerAngles;
            rotation.x = 0f;
            rotation.z = 0f;

            transform.rotation = Quaternion.Euler(rotation);
        }
    }
}
