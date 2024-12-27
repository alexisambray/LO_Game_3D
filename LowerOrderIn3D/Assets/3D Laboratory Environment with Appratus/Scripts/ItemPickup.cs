using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemPickUp : MonoBehaviour
{
    [SerializeField] private string ItemTag = "Potion";

    public Item Item;

    void PickUp()
    {
        InventoryManager.Instance.Add(Item);
        Destroy(gameObject);
    }

    private void OnMouseDown()
    {
        if (gameObject.CompareTag(ItemTag))
        {
            PickUp();
        }
    }
}
