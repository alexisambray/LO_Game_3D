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

                // Snap to the shelf's position and rotation
                this.transform.position = shelfChecker.transform.position;
                this.transform.rotation = shelfChecker.transform.rotation;

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



}


