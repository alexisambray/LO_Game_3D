using UnityEngine;

public interface IInteractable
{
    void Interact(Transform interactorTransform);
    public string GetInteractText();
    Transform GetTransform();


}