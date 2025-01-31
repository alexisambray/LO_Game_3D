using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    public float raycastRange = 5f;
    public LayerMask interactableLayer;
    public Transform holdPoint;
    public Image interactionIcon; // Assign UI Image in Inspector

    private GameObject heldObject;
    

    void Update()
    {
        ShowInteractionIcon(); // Check for interactable objects

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObject == null)
            {
                TryPickup();
            }
            else
            {
                DropItem();
            }
        }
    }

    void ShowInteractionIcon()
    {
        Debug.DrawRay(this.transform.position, this.transform.forward * raycastRange, Color.green);
        Ray ray = new Ray(this.transform.position, this.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastRange, interactableLayer))
        {
            interactionIcon.enabled = true; // Show icon
        }
        else
        {
            interactionIcon.enabled = false; // Hide icon
        }
    }

    void TryPickup()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastRange, interactableLayer))
        {
            Debug.Log($"Picked up: {hit.collider.name}");

            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                heldObject = hit.collider.gameObject;
                rb.isKinematic = true;
                rb.transform.position = holdPoint.position;
                rb.transform.SetParent(holdPoint);
            }
        }
    }

    void DropItem()
    {
        if (heldObject != null)
        {
            Rigidbody rb = heldObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                heldObject.transform.SetParent(null);
            }

            Debug.Log($"Dropped: {heldObject.name}");
            heldObject = null;
        }
    }
}
