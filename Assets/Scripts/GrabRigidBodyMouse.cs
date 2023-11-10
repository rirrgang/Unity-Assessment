using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabRigidBodyMouse : MonoBehaviour
{
    public float forceAmount = 500;
    public LineRenderer lineRenderer;

    public float springDamper = 100f;
    public float springMassScale = 10f;
    public float springMaxDistance = 1f;
    private SpringJoint springJoint;

    Rigidbody selectedRigidbody;
    Camera targetCamera;
    Vector3 originalScreenTargetPosition;
    Vector3 originalRigidbodyPos;
    public float selectionDistance;

    // Start is called before the first frame update
    void Start()
    {
        targetCamera = GetComponent<Camera>();
        // Ensure there's a LineRenderer to use
        if (!lineRenderer)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;

        // Configure the LineRenderer
        lineRenderer.positionCount = 2;
    }

    void Update()
    {
        if (!targetCamera)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            // Check if we are hovering over Rigidbody, if so, select it
            selectedRigidbody = GetRigidbodyFromMouseClick();
            if (selectedRigidbody)
            {
                if (!springJoint)
                {
                    GameObject go = new GameObject("Rigidbody dragger");
                    Rigidbody body = go.AddComponent<Rigidbody>();
                    body.isKinematic = true;
                    springJoint = go.AddComponent<SpringJoint>();
                    springJoint.autoConfigureConnectedAnchor = false;
                    springJoint.connectedAnchor = selectedRigidbody.transform.position;
                    springJoint.anchor = go.transform.position;
                    springJoint.spring = forceAmount;
                    springJoint.damper = springDamper;
                    springJoint.massScale = springMassScale;
                    springJoint.maxDistance = springMaxDistance;
                    springJoint.connectedBody = selectedRigidbody;
                }
                lineRenderer.enabled = true;
            }
        }
        if (Input.GetMouseButtonUp(0) && selectedRigidbody)
        {
            // Release selected Rigidbody if there any
            Destroy(springJoint.gameObject); // Destroy the temporary GameObject
            selectedRigidbody = null;
            lineRenderer.enabled = false;
        }

    }

    void LateUpdate()
    {
        if (springJoint)
        {
            // Update the position of the joint GameObject to follow the mouse
            springJoint.transform.position = targetCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, selectionDistance));
            // Update the line renderer to draw the line from the joint to the rigidbody
            lineRenderer.SetPosition(0, springJoint.transform.position);
            lineRenderer.SetPosition(1, selectedRigidbody.transform.position);
        }
    }

    Rigidbody GetRigidbodyFromMouseClick()
    {
        RaycastHit hitInfo = new RaycastHit();
        Ray ray = targetCamera.ScreenPointToRay(Input.mousePosition);
        //Ray ray = new Ray(targetCamera.transform.position, targetCamera.transform.forward);
        bool hit = Physics.Raycast(ray, out hitInfo);
        if (hit)
        {
            if (hitInfo.collider.gameObject.GetComponent<Rigidbody>())
            {
                //check if gameobject has no tag of player
                if (!hitInfo.collider.gameObject.CompareTag("Player"))
                {
                    //selectionDistance = Vector3.Distance(ray.origin, hitInfo.point);
                    selectionDistance = 3f;
                    originalScreenTargetPosition = targetCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, selectionDistance));
                    //originalScreenTargetPosition = targetCamera.ScreenToWorldPoint(new Vector3(0f, 0f, selectionDistance));
                    originalRigidbodyPos = hitInfo.collider.transform.position;
                    return hitInfo.collider.gameObject.GetComponent<Rigidbody>();
                }

            }
        }

        return null;
    }
}