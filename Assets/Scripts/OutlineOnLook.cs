using UnityEngine;
using UnityEngine.UI;
public class OutlineOnLook : MonoBehaviour
{
    public Camera playerCamera;
    public float maxDistance = 10f;
    public LayerMask layerMask;
    public Material outlineFillMaterial;
    public Material outlineMaskMaterial;
    private Renderer currentRenderer;
    private Material originalMaterial;
    private Material[] originalMaterials;
    private Material[] outlineMaterials;
    private bool isOutlined = false;

    public GameObject[] uiObjects;
    public Image uiCrosshair;
    public Sprite dotTexture;
    public Sprite grabTexture;


    void setVisibilityUI(bool isVisible)
    {
        foreach (GameObject uiObject in uiObjects)
        {
            uiObject.SetActive(isVisible);
        }
    }

    void Start()
    {
        // Pre-create the array for the outline materials to avoid doing it each time in update
        outlineMaterials = new Material[3] { null, outlineFillMaterial, outlineMaskMaterial };
        setVisibilityUI(false);
    }

    void LateUpdate()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance, layerMask))
        {
            Renderer hitRenderer = hit.collider.GetComponent<Renderer>();

            // Only update materials if we hit a new renderer
            if (currentRenderer != hitRenderer)
            {
                ResetOutline();
                if (hitRenderer != null)
                {
                    SetOutline(hitRenderer);
                }
            }
        }
        else if (isOutlined)
        {
            // If we're not looking at the previous object anymore, reset its material
            ResetOutline();
        }
    }

    private void SetOutline(Renderer hitRenderer)
    {
        // Store the original materials and set the outline
        originalMaterials = hitRenderer.materials;
        outlineMaterials[0] = hitRenderer.material;
        hitRenderer.materials = outlineMaterials;
        currentRenderer = hitRenderer;
        isOutlined = true;
        setVisibilityUI(true);
        uiCrosshair.sprite = grabTexture;
    }

    private void ResetOutline()
    {
        // Reset the previous object's materials if it exists
        if (currentRenderer != null)
        {
            currentRenderer.materials = originalMaterials;
            currentRenderer = null;
        }
        isOutlined = false;
        setVisibilityUI(false);
        uiCrosshair.sprite = dotTexture;
    }
}