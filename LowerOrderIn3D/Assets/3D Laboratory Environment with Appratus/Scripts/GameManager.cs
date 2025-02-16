using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

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

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        StartDay();
    }

    public void InstantiatePrefab(){
        GameObject mixture1 = Instantiate(mixture, mixtureSlot1);
        GameObject mixture2 = Instantiate(mixture, mixtureSlot2);
        GameObject mixture3 = Instantiate(mixture, mixtureSlot3);

        mixture1.transform.localPosition = Vector3.zero;
        mixture2.transform.localPosition = Vector3.zero;
        mixture3.transform.localPosition = Vector3.zero;

        OutlineController Pref1 = mixture1.GetComponentInChildren<OutlineController>(false);
        Pref1.player = player;

        OutlineController Pref2 = mixture1.GetComponentInChildren<OutlineController>(false);
        Pref2.player = player;

        OutlineController Pref3 = mixture1.GetComponentInChildren<OutlineController>(false);
        Pref3.player = player;
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
           float pickUpDistance = 2f;
           if (Physics.Raycast(ray, out hit, pickUpDistance))
           {
            Debug.Log("Hit" + hit.collider.gameObject.name);
               if (hit.collider.gameObject.CompareTag("Mixture"))
               {
                    if(hit.collider.gameObject.transform.GetChild(1).gameObject.activeSelf == true){
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

                   else{
                        if(objectGrabbable == null){
                            if(hit.transform.TryGetComponent(out objectGrabbable)){
                                objectGrabbable.Grab(objectGrabPointTransform);
                            }
                        }
                        // else{
                        //     objectGrabbable.Drop();
                        //     objectGrabbable = null;
                        // }
                   }
               }
           }
        }
    }
}
