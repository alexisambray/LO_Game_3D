using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ObjectPool;

public class ItemPotion : MonoBehaviour
{
    [SerializeField] private Appearance appearance;
    [SerializeField] private Uses uses;

    public bool DoesItemClickOnShelf(ItemPotion potion, ItemPotion shelfChecker)
    {
        if (potion.appearance == shelfChecker.appearance && potion.uses == shelfChecker.uses) return true;

        return false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(this.CompareTag("Potion") && other.CompareTag("Shelf"))
        {
            ItemPotion shelfChecker = other.GetComponent<ItemPotion>();
            if (shelfChecker != null)
            {
                // Log positions and scales for debugging
                Debug.Log($"Potion Scale Before: {this.transform.localScale}");
                Debug.Log($"Shelf Scale: {shelfChecker.transform.localScale}");

                // Store the original scale and position
                Vector3 originalScale = this.transform.localScale;
                Vector3 originalPosition = this.transform.position;

                // Temporarily reset scale
                this.transform.localScale = Vector3.one;

                // Snap to the shelf's position and rotation
                this.transform.position = shelfChecker.transform.position;
                this.transform.rotation = shelfChecker.transform.rotation;

                // Restore the original scale
                this.transform.localScale = originalScale;

                // Adjust position to account for scale offset
                this.transform.position = CalculateCorrectedPosition(shelfChecker.transform.position, originalScale);


                Debug.Log($"Potion Scale Restored: {this.transform.localScale}");
                Debug.Log($"Potion Final Position: {this.transform.position}");


                Rigidbody rigidBody = this.transform.GetComponent<Rigidbody>();
                    rigidBody.isKinematic = true;
            }
                else
                {
                    Debug.Log("Potion does not match the shelf.");
                }
            
        }
    }

    private Vector3 CalculateCorrectedPosition(Vector3 snapPosition, Vector3 originalScale)
    {
        // Calculate the scaling offset using the difference from (1, 1, 1)
        Vector3 scaleDifference = originalScale - Vector3.one;

        // Adjust the position based on the pivot offset
        Vector3 correctedPosition = snapPosition;
        correctedPosition.x -= (scaleDifference.x * snapPosition.x) / 2.0f;
        correctedPosition.y -= (scaleDifference.y * snapPosition.y) / 2.0f;
        correctedPosition.z -= (scaleDifference.z * snapPosition.z) / 2.0f;

        return correctedPosition;
    }

}
        

