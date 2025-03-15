using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static ObjectPool;

public class ItemPotion : MonoBehaviour
{
    [Header("Mixture Attributes")]
    [SerializeField] public Appearance appearance;
    [SerializeField] public Uses uses;
    [SerializeField] private int numberOfTriesLeft = 4;
    
    [Header("Boolean Interactions")]
    [SerializeField] public bool AppearanceFound = false;
    [SerializeField] public bool UseFound = false;
    public bool interactable = true;
    [SerializeField] private bool isFull = false;
    [SerializeField] private float launchVelocity;

    [Header("Mixture-Tool Appearance Interactions")]
    [SerializeField] public bool testedWithFlashlight = false;
    [SerializeField] public bool testedWithCentrifuge = false;
    [SerializeField] public bool testedWithMicroscope = false;

    [Header("Mixture-Tool Use Interactions")]
    [SerializeField] public bool testedForFoodAndBeverage = false;
    [SerializeField] public bool testedForMedicine = false;
    [SerializeField] public bool testedForHouseCleaning = false;
    [SerializeField] public bool testedForAgriculture = false;
    [SerializeField] public bool testedForCosmetics = false;
    [SerializeField] public bool testedForPersonalHygiene = false;

    [Header("Mixture Appearance")]
    [SerializeField] private Sprite flashlightImage; // Image when tested with Flashlight
    [SerializeField] private Sprite centrifugeImage; // Image when tested with Centrifuge
    [SerializeField] private Sprite microscopeImage; // Image when tested with Microscope
    [SerializeField] private Image mixtureImage; // UI Image component

    public bool DoesItemClickOnShelf(ItemPotion potion, ItemPotion shelfChecker)
    {
        if (potion.appearance == shelfChecker.appearance && potion.uses == shelfChecker.uses) return true;

        return false;
    }

    // public void OnTriggerEnter(Collider other)
    // {
    //     if(this.CompareTag("Mixture") && other.CompareTag("Shelf"))
    //     {
    //         ItemPotion shelfChecker = other.GetComponent<ItemPotion>();
    //         // if (shelfChecker != null && numberOfTriesLeft > 0 && !shelfChecker.isFull && shelfChecker.interactable)
    //         if(shelfChecker != null && !shelfChecker.isFull && shelfChecker.interactable)
    //         {
    //             // Snap to the shelf's position and rotation
    //             this.transform.position = shelfChecker.transform.position;
    //             this.transform.rotation = shelfChecker.transform.rotation;
                

    //             Rigidbody rigidBody = this.transform.GetComponent<Rigidbody>();
    //             rigidBody.isKinematic = true;
    //             shelfChecker.isFull = true;
    //             shelfChecker.interactable = false;
    //             this.isFull = true;
    //             this.interactable = false;

    //             this.transform.SetParent(null);
                
    //             //if item does not match the shelf
    //             if(!DoesItemClickOnShelf(this, shelfChecker))
    //             {
    //                 Debug.Log("Potion does not match the shelf.");
                    
    //                 this.transform.position = this.transform.position - new Vector3(1f, 0, 0);

    //                 rigidBody.isKinematic = false;
    //                 rigidBody.velocity = Vector3.zero;
    //                 rigidBody.angularVelocity = Vector3.zero;

    //                 this.transform.SetParent(null);

    //                 // Vector3 pushDirection = (this.transform.position - shelfChecker.transform.position).normalized;
    //                 // rigidBody.AddForce(pushDirection * 10f, ForceMode.Impulse); // Adjust force if needed

    //                 //enable clicking of shelf and mixture
    //                 shelfChecker.isFull = false;
    //                 shelfChecker.interactable = true;
    //                 //disable clicking on these
    //                 this.interactable = true;
                    
    //             }
    //         }
    //             else if(numberOfTriesLeft <= 0)
    //             {
    //                 //remove potion from shelf slot
    //                 //TODO: add remove logic for potion
    //                 Debug.Log("No tries left, removing potion.");
    //                 Destroy(gameObject, 2f); // Delayed destroy for effect
    //             }
            
    //     }
    // }

    // public void OnTriggerExit(Collider other)
    // {
    //     numberOfTriesLeft -= 1;
    // }
}


