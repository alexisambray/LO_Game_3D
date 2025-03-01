using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static ObjectPool;

public class ItemPotion : MonoBehaviour
{
    [Header("Mixture Attributes")]
    [SerializeField] public Appearance appearance;
    [SerializeField] public Uses uses;
    [SerializeField] private int numberOfTriesLeft = 3;
    
    [Header("Boolean Interactions")]
    [SerializeField] public bool AppearanceFound = false;
    [SerializeField] public bool UseFound = false;
    public bool interactable = true;
    [SerializeField] private bool isFull = false;
    [SerializeField] private float launchVelocity;

    public void Awake()
    {
        //TODO: Randomize appearance and use. It should not be the same as before.
    }
    public bool DoesItemClickOnShelf(ItemPotion potion, ItemPotion shelfChecker)
    {
        if (potion.appearance == shelfChecker.appearance && potion.uses == shelfChecker.uses) return true;

        return false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(this.CompareTag("Mixture") && other.CompareTag("Shelf"))
        {
            ItemPotion shelfChecker = other.GetComponent<ItemPotion>();
            // if (shelfChecker != null && numberOfTriesLeft > 0 && !shelfChecker.isFull && shelfChecker.interactable)
            if(shelfChecker != null && !shelfChecker.isFull && shelfChecker.interactable)
            {
                // Snap to the shelf's position and rotation
                this.transform.position = shelfChecker.transform.position;
                this.transform.rotation = shelfChecker.transform.rotation;
                

                Rigidbody rigidBody = this.transform.GetComponent<Rigidbody>();
                rigidBody.isKinematic = true;
                shelfChecker.isFull = true;
                shelfChecker.interactable = false;
                this.isFull = true;
                this.interactable = false;
                
                //if item does not match the shelf
                if(!DoesItemClickOnShelf(this, shelfChecker))
                {
                    Debug.Log("Potion does not match the shelf.");
                    rigidBody.isKinematic = false;
                    rigidBody.AddRelativeForce(new Vector3
                                            (launchVelocity, 0, 0));
                                            numberOfTriesLeft -= 1;
                    //enable clicking of shelf and mixture
                    shelfChecker.isFull = false;
                    shelfChecker.interactable = true;
                    //disable clicking on these
                    this.interactable = true;
                    
                }
            }
                else if(numberOfTriesLeft == 0)
                {
                    //remove potion from shelf slot
                    //TODO: add remove logic for potion
                    //Debug.Log("Potion is destroyed");
                    //Destroy(this, 5);
                    other.GetComponent<ItemPotion>().isFull = false;
                }
            
        }
    }



}


