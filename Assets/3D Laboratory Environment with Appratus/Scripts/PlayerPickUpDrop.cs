using System.Collections;
using System.Collections.Generic;
// using Microsoft.Unity.VisualStudio.Editor;
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
            button.onClick.AddListener(() => DropHeldObject());
        }
    }
    private void Update()
    {
    //  if(Input.GetMouseButtonDown(0)){
    //     float pickUpDistance = 2f;
    //     if(objectGrabbable == null){
    //         //pickup
    //         if(Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickUpDistance)){
    //             if(raycastHit.transform.TryGetComponent(out objectGrabbable)){
    //                 objectGrabbable.Grab(objectGrabPointTransform);
    //             }
    //         }
    //     }
    //     // else{
    //     //     objectGrabbable.Drop();
    //     //     objectGrabbable = null;
    //     // }

    //  }   
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
