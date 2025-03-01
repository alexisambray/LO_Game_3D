using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool isHoldingItem { get; set; }

    public TMP_Text dayText;
    [SerializeField] private int dayCounter = 0;

    public GameObject mixture;

    public Transform mixtureSlot1, mixtureSlot2, mixtureSlot3;

    public int mixtureGenerated = 0;
    public Transform player;

    public Camera playerCamera;

    private ObjectGrabbable objectGrabbable;
    [SerializeField] private LayerMask pickUpLayerMask;
    [SerializeField] private Transform objectGrabPointTransform;

    [SerializeField] private GameObject DropUIButton;
    private Button dropButton;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        dropButton = DropUIButton.GetComponent<Button>();     
    }

    private void Start()
    {
        StartDay();
    }

    public void InstantiatePrefab(){
        // GameObject mixture1 = Instantiate(mixture, mixtureSlot1);
        // GameObject mixture2 = Instantiate(mixture, mixtureSlot2);
        // GameObject mixture3 = Instantiate(mixture, mixtureSlot3);

        // mixture1.transform.localPosition = Vector3.zero;
        // mixture2.transform.localPosition = Vector3.zero;
        // mixture3.transform.localPosition = Vector3.zero;

        // mixture1.transform.localRotation = Quaternion.identity;
        // mixture2.transform.localRotation = Quaternion.identity;
        // mixture3.transform.localRotation = Quaternion.identity;

        GameObject mixture1 = Instantiate(mixture);
        mixture1.transform.SetParent(mixtureSlot1, false); // Keeps prefab’s local transform
        mixture1.transform.localPosition = Vector3.zero;
        mixture1.transform.localRotation = Quaternion.identity;

        GameObject mixture2 = Instantiate(mixture);
        mixture2.transform.SetParent(mixtureSlot2, false); // Keeps prefab’s local transform
        mixture2.transform.localPosition = Vector3.zero;
        mixture2.transform.localRotation = Quaternion.identity;

        GameObject mixture3 = Instantiate(mixture);
        mixture3.transform.SetParent(mixtureSlot3, false); // Keeps prefab’s local transform
        mixture3.transform.localPosition = Vector3.zero;
        mixture3.transform.localRotation = Quaternion.identity;

        
    }   

    public void StartDay()
    {
        dayCounter++;
        UpdateDayUI();
        InstantiatePrefab();


    }

    private void UpdateDayUI()
    {
        if (dayText != null)
        {
            dayText.text = $"Day: {dayCounter}";
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
           //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
           Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
           RaycastHit hit;
           float pickUpDistance = 4f;
           if (Physics.Raycast(ray, out hit, pickUpDistance))
           {
            //Debug.DrawRay(player.position, player.forward * 10000f, Color.green);
            Debug.Log("Hit " + hit.collider.gameObject.name + " with " + hit.collider.gameObject.tag);
               if (hit.collider.gameObject.CompareTag("Box"))
               {
                    if(hit.collider.gameObject.transform.GetChild(1).gameObject.activeSelf == true)
                    {
                        //if unopened, open the box and make it go away
                        Animator animator = hit.collider.gameObject.GetComponentInChildren<Animator>(false);
                        if(animator.GetBool("isClicked") == false){ animator.SetBool("isClicked", true); }
                        else //remove box
                        { 
                            GameObject box = hit.collider.gameObject;   //transform.Find("Mixture(Clone)/Cardboard Box");
                            if(box.transform.Find("Cardboard Box"))
                            {
                                //Transform childBox = box.transform.Find("Cardboard Box");
                                box.transform.GetChild(1).gameObject.SetActive(false);
                                //box.SetActive(false);
                            }
                        }
                    }

                   else { return; }
                }
                   else if(hit.collider.gameObject.CompareTag("Mixture"))
                   {
                        if(objectGrabbable == null){
                            ItemPotion itemPotion = hit.transform.GetComponent<ItemPotion>();
                            if(hit.transform.TryGetComponent(out objectGrabbable) && itemPotion.interactable){
                                DropUIButton.SetActive(true);
                                objectGrabbable.Grab(objectGrabPointTransform);

                                
                                dropButton.onClick.RemoveAllListeners();
                                dropButton.onClick.AddListener(() => 
                                {
                                    if(objectGrabbable != null)
                                    {
                                        objectGrabbable.Drop();
                                        objectGrabbable = null;
                                        DropUIButton.SetActive(false);
                                    }
                                });
                                

                            //     button.onClick.RemoveAllListeners();
                            //     ObjectGrabbable currentObject = objectGrabbable; // Capture the exact object
                            //     button.onClick.AddListener(() => currentObject.Drop()); // Assign the correct object's Drop()
                            }
                        }
                        // else{
                        //     objectGrabbable.Drop();
                        //     objectGrabbable = null;
                        // }
                   }

                   else if(hit.collider.gameObject.CompareTag("Shelf"))
                   {
                        if(objectGrabbable != null)
                        {
                            //TODO: Run ItemPotion method of checking if item is correctly placed.
                            ItemPotion selectedShelf = hit.collider.gameObject.GetComponent<ItemPotion>();
                            Transform parentTransform = objectGrabbable.transform;
                            ItemPotion parentMixture = parentTransform.GetComponent<ItemPotion>();

                                objectGrabbable.Drop();
                                objectGrabbable = null;
                                DropUIButton.SetActive(false);
                                parentTransform.position = selectedShelf.transform.position;

                        }
                   }

                   else if (hit.collider.gameObject.CompareTag("Tool"))
                   {
                    ToolInteraction selectedTool = hit.collider.gameObject.GetComponent<ToolInteraction>();
                    //if player has an mixture on hand and the workstation is not full
                    if(objectGrabbable != null && !selectedTool.isFull)
                    {
                        Transform parentTransform = objectGrabbable.transform;
                        ItemPotion parentMixture = parentTransform.GetComponent<ItemPotion>();
                        //set UI elements
                        objectGrabbable.transform.parent = null;

                        GameObject slot = hit.collider.gameObject;
                        
                        // Transform slot = transform.Find("Tool Slot");
                        objectGrabbable.transform.SetParent(slot.transform, false);
                        Transform toolSlot = slot.transform.Find("Tool Slot");
                        if (toolSlot != null)
                        {
                            objectGrabbable.transform.position = toolSlot.position;
                        }
                        objectGrabbable.Drop();
                        objectGrabbable = null;
                        DropUIButton.SetActive(false);
                        
                        parentTransform.position = selectedTool.transform.position;
                        //run tool logic
                        selectedTool.ToolWhenClicked(parentMixture);
                    }
                    //else if player has no mixture on hand and the workstation is full
                    else if (objectGrabbable == null && selectedTool.isFull)
                    {
                        //activate UI and pickup mixture
                        DropUIButton.SetActive(true);
                                objectGrabbable.Grab(objectGrabPointTransform);

                                dropButton.onClick.RemoveAllListeners();
                                dropButton.onClick.AddListener(() => 
                                {
                                    if(objectGrabbable != null)
                                    {
                                        objectGrabbable.Drop();
                                        objectGrabbable = null;
                                        DropUIButton.SetActive(false);
                                    }
                                });

                        //make workstation usable for other mixtures again
                        selectedTool.isFull = false;    
                    }
                    //if workstation is full or no mixture on hand
                    else
                    {
                        Debug.Log("Mixture is full/No mixture on hand");
                    }

                   }
               
           }
        }
    }
}
