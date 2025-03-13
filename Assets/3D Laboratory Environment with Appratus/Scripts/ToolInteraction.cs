using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
// using UnityEditor;
// using UnityEditor.Toolbars;
using UnityEngine;
using static ObjectPool;


public class ToolInteraction : MonoBehaviour
{
    [SerializeField] public WorkTools tools;
    [SerializeField] public LoadingBar loadingBar;
    [SerializeField] public bool isFull = false;
    [SerializeField] public bool interactable = true;

    ItemPotion itemPotion;
    public Transform toolSlot;

    public int timeForToolToExecute;

    public bool ToolSelection(ItemPotion potion,  ToolInteraction workstation)
    {
        switch(workstation.tools)
        {
            case WorkTools.Flashlight:
                potion.testedWithFlashlight = true;
                return true;
            
            
            case WorkTools.Centrifuge:
                potion.testedWithCentrifuge = true;
                return true;

            case WorkTools.Microscope:
                potion.testedWithMicroscope = true;
                return true;
        }
        return false;
    }

    public void ToolWhenClicked(ItemPotion potion)
    {

        Rigidbody objectRigidbody = potion.GetComponent<Rigidbody>();
        if(objectRigidbody == null)
        {
            Debug.Log("Object Rigidbody not found");
        }
        if(isFull == true)
        {
            Debug.Log("Tool is in Use!");
            return;
        }

        if(this.isFull) return;

        this.itemPotion = potion;   
        objectRigidbody.isKinematic = true;

        this.itemPotion.transform.SetParent(toolSlot, false);
        this.itemPotion.transform.localPosition = Vector3.zero;
        this.itemPotion.transform.rotation = Quaternion.identity;

        //makes the tool workbench full and the potion not interactable until it is finished
        isFull = true;
        potion.interactable = false;
        this.interactable = false;
        //TODO: Implement Loading Bar Animation. Enable when clicked
        // loadingBar.transform.parent.gameObject.SetActive(true);
        loadingBar.gameObject.SetActive(true);
        loadingBar.AnimateBar(timeForToolToExecute, OnLoadingComplete);
        
        if(ToolSelection(this.itemPotion, this))
        {
            //TODO: Implement Logic when item is found
            this.itemPotion.AppearanceFound = true;
        }
        
        //loadingBar.gameObject.SetActive(false);
    }

private void OnLoadingComplete()
{
    Debug.Log("Loading bar finished!");

    if (ToolSelection(this.itemPotion, this))
    {
        this.itemPotion.AppearanceFound = true;
    }

    //this.isFull = false; // Workstation is now available again
    this.itemPotion.interactable = true; // Allow interaction again
    this.interactable = true;
}




}
