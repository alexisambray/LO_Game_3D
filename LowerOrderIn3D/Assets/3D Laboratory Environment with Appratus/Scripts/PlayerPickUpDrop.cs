using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPickUpDrop : MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private Transform objectGrabPointTransform;
    [SerializeField] private LayerMask pickUpLayerMask;

    [SerializeField] private GameObject DropUIButton;
    private ObjectGrabbable objectGrabbable;

    private Button button;


    private void Start()
    {
         button = DropUIButton.GetComponent<Button>();
        if(button != null)
        {
            button.onClick.AddListener(() => objectGrabbable.Drop());
        }
    }
    private void Update()
    {
     if(Input.GetMouseButtonDown(0)){
        float pickUpDistance = 2f;
        if(objectGrabbable == null){
            if(Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickUpDistance)){
                if(raycastHit.transform.TryGetComponent(out objectGrabbable)){
                    objectGrabbable.Grab(objectGrabPointTransform);
                    DropUIButton.SetActive(true);

                    // button = DropUIButton.GetComponent<Button>();
                    // button.onClick.RemoveAllListeners();

                    // button.onClick.AddListener(() => objectGrabbable.Drop());
                    button.onClick.RemoveAllListeners();
                    ObjectGrabbable currentObject = objectGrabbable; // Capture the exact object
                    button.onClick.AddListener(() => currentObject.Drop()); // Assign the correct object's Drop()
                }
            }
        }
        // else{
        //     objectGrabbable.Drop();
        //     objectGrabbable = null;
        //     DropUIButton.SetActive(false);
        // }

     }   
    }

    public void DropHeldObject()
    {
        if(objectGrabbable != null)
        {
            objectGrabbable.Drop();
            objectGrabbable = null;
            DropUIButton.SetActive(false);
        }
    }
}
