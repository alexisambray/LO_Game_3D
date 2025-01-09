using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactText;

    public Transform GetTransform() { return transform; }

    public string GetInteractText() { return interactText; }

    public void Interact(Transform interactorTransform) { Debug.Log("Potion clicked!");  }

}
