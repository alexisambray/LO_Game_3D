using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSystem : MonoBehaviour
{
    public GameObject pickUpText;
    public GameObject flashlightOnPlayer;
    public GameObject pickUpBtn;
    public GameObject dropBtn;

    void Start()
    {
        pickUpText.SetActive(false);
        flashlightOnPlayer.SetActive(false);
        pickUpBtn.SetActive(false);
        dropBtn.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !GameManager.Instance.isHoldingItem)
        {
            pickUpText.SetActive(true);
            pickUpBtn.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            pickUpText.SetActive(false);
            pickUpBtn.SetActive(false);
        }
    }

    public void PickUpItem()
    {
        this.gameObject.SetActive(false);
        flashlightOnPlayer.SetActive(true);
        pickUpText.SetActive(false);
        pickUpBtn.SetActive(false);
        dropBtn.SetActive(true);
        GameManager.Instance.isHoldingItem = true;
    }

    public void DropItem()
    {
        if (GameManager.Instance.isHoldingItem)
        {
            this.gameObject.SetActive(true);
            flashlightOnPlayer.SetActive(false);
            dropBtn.SetActive(false) ;
            GameManager.Instance.isHoldingItem = false;
        }
    }
}