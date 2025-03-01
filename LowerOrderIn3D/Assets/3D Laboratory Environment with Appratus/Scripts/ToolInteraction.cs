using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;
using static ObjectPool;


public class ToolInteraction : MonoBehaviour
{
    [SerializeField] public WorkTools tools;
    [SerializeField] public bool isFull = false;

    ItemPotion itemPotion;
    [SerializeField] public LoadingBar loadingBar;

    public bool ToolSelection(ItemPotion potion,  ToolInteraction workstation)
    {
        switch(workstation.tools)
        {
            case WorkTools.Flashlight:
                if(potion.appearance == Appearance.Colloid) {return true;}
            break;
            
            case WorkTools.Centrifuge:
                if(potion.appearance == Appearance.Suspension || potion.appearance == Appearance.TrueSolution)
                    {return true;}
            break;

            case WorkTools.Microscope:
                if(potion.appearance == Appearance.Suspension) {return true;}
            break;
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


        this.itemPotion = potion;   
        objectRigidbody.isKinematic = true;
        
        isFull = true;
        //TODO: Implement Loading Bar Animation. Enable when clicked
        // loadingBar.transform.parent.gameObject.SetActive(true);
        loadingBar.gameObject.SetActive(true);
        loadingBar.AnimateBar();
        
        if(ToolSelection(this.itemPotion, this))
        {
            //TODO: Implement Logic when item is found
            this.itemPotion.AppearanceFound = true;
        }
        
        //loadingBar.gameObject.SetActive(false);
    }





}
