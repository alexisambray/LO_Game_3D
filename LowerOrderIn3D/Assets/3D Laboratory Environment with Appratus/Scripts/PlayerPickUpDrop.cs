using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerPickUpDrop : MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private Transform objectGrabPointTransform;
    [SerializeField] private LayerMask pickUpLayerMask;

    [SerializeField] private GameObject DropUIButton;
    private ObjectGrabbable objectGrabbable;
    private void Update()
    {
     if(Input.GetMouseButtonDown(0)){
        float pickUpDistance = 2f;
        if(objectGrabbable == null){
            if(Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickUpDistance)){
                if(raycastHit.transform.TryGetComponent(out objectGrabbable)){
                    objectGrabbable.Grab(objectGrabPointTransform);
                    DropUIButton.SetActive(true);
                }
            }
        }
        else{
            objectGrabbable.Drop();
            objectGrabbable = null;
            DropUIButton.SetActive(false);
        }

     }   
    }
}
