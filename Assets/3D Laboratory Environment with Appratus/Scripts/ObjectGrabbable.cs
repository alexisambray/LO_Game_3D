using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ObjectGrabbable : MonoBehaviour
{

    private Rigidbody objectRigidbody;
    private Transform objectGrabPointTransform;

    private void Awake()
    {
     objectRigidbody = GetComponent<Rigidbody>();   
    }

    public void Grab(Transform objectGrabPointTransform){
        this.objectGrabPointTransform = objectGrabPointTransform;
        objectRigidbody.useGravity = false;
        objectRigidbody.isKinematic = false;

        //remove item from mixture slot
        this.transform.SetParent(null, false);
    }

    public void Drop(){
        Debug.Log(gameObject.name + " is dropping!");
        this.objectGrabPointTransform = null;
        objectRigidbody.isKinematic = false;

        if(objectRigidbody == null) { Debug.Log("It's empty again"); }
        else objectRigidbody.useGravity = true;
    }

    // private void FixedUpdate()
    // {
    //     if(objectGrabPointTransform != null)
    //     {
    //         float lerpSpeed = 10f;
    //         Vector3 newPosition = Vector3.Lerp(transform.position, objectGrabPointTransform.position, Time.deltaTime * lerpSpeed);
    //         objectRigidbody.MovePosition(newPosition);
    //     }
    // }

private void FixedUpdate()
{
    if (objectGrabPointTransform != null)
    {
        float lerpSpeed = 10f;
        Vector3 newPosition = Vector3.Lerp(transform.position, objectGrabPointTransform.position, Time.deltaTime * lerpSpeed);
        objectRigidbody.MovePosition(newPosition);

        // Check if object is close enough to the target position
        if (Vector3.Distance(transform.position, objectGrabPointTransform.position) < 0.01f)
        {
            objectRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            objectRigidbody.isKinematic = true; // Disable physics-based movement
            objectRigidbody.freezeRotation = true; // Prevent rotation
            objectRigidbody.transform.rotation = Quaternion.identity;
        }
    }
}

}
