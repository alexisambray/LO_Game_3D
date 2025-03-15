using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool isHoldingItem { get; set; }

    public TMP_Text dayText;
    [SerializeField] private int dayCounter = 0;

    public GameObject mixturePrefab;
    public Transform mixtureSlot1, mixtureSlot2, mixtureSlot3;
    public Transform player;
    public Camera playerCamera;
    
    [SerializeField] private LayerMask pickUpLayerMask;
    [SerializeField] private Transform objectGrabPointTransform;
    [SerializeField] private GameObject dropUIButton;
    [SerializeField] private GameObject mixtureChecklistUIButton;
    
    private Button dropButton;
    private Button mixtureChecklistButton;
    private ObjectGrabbable objectGrabbable;
    private List<GameObject> mixturePrefabs = new List<GameObject>();
    public MovementController movementController;
    private List<OutlineController> outlineObjects = new List<OutlineController>();
    public OutlineController outlineController;
    public MixtureChecklist mixtureChecklist;
    public GameObject mixtureChecklistCanvas;
    public GameObject useChecklistCanvas;
    private Dictionary<UseInteraction, (int retrievalDay, Dictionary<string, bool> checks)> mixturesInQueue 
        = new Dictionary<UseInteraction, (int, Dictionary<string, bool>)>();
    public int daysLeftToCompletion;
    private GameObject mixtureSentToLab; // Track the active mixture being processed
    public Transform useSlot;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        dropButton = dropUIButton.GetComponent<Button>();
        mixtureChecklistButton = mixtureChecklistUIButton.GetComponent<Button>();
    }

    private void Start()
    {
        OutlineController[] foundObjects = FindObjectsOfType<OutlineController>(true);
        outlineObjects.AddRange(foundObjects);

        Debug.Log("Found " + foundObjects.Length + " objects with OutlineController");

        InitializeMixtures();
        StartDay();

        OnItemDropOutline();
    }

    private void InitializeMixtures()
    {
        ObjectPool.Appearance[] appearances = (ObjectPool.Appearance[])System.Enum.GetValues(typeof(ObjectPool.Appearance));
        ObjectPool.Uses[] uses = (ObjectPool.Uses[])System.Enum.GetValues(typeof(ObjectPool.Uses));

        foreach (var appearance in appearances)
        {
            foreach (var use in uses)
            {
                GameObject newMixture = Instantiate(mixturePrefab);
                newMixture.SetActive(false);

                if (newMixture.TryGetComponent(out ItemPotion potion))
                {
                    potion.appearance = appearance;
                    potion.uses = use;
                }
                mixturePrefabs.Add(newMixture);
            }
        }
        ShuffleList(mixturePrefabs);
    }

    private void ShuffleList(List<GameObject> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randIndex = Random.Range(0, i + 1);
            (list[i], list[randIndex]) = (list[randIndex], list[i]);
        }
    }

    public void InstantiateMixtures()
    {
        TryPlaceMixture(mixtureSlot1);
        TryPlaceMixture(mixtureSlot2);
        TryPlaceMixture(mixtureSlot3);
    }

    private void TryPlaceMixture(Transform slot)
    {
        if (slot.childCount > 0 || mixturePrefabs.Count == 0) return;

        GameObject spawnedMixture = mixturePrefabs[0];
        mixturePrefabs.RemoveAt(0);

        spawnedMixture.SetActive(true);
        spawnedMixture.transform.SetParent(slot, false);
        spawnedMixture.transform.localPosition = Vector3.zero;
    }

    public void StartDay()
    {
        dayCounter++;
        UpdateDayUI();
        InstantiateMixtures();

        if(daysLeftToCompletion > 0)
        {
            daysLeftToCompletion--;
        }
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
        //for testing on pc
        if (Input.GetMouseButtonDown(0))
        {
            HandleRaycast();
        }
        DetectTouchInput();

    }

    private void DetectTouchInput()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            // Call IsTap() from MovementController
            if (movementController.IsTap(touch))
            {
                HandleRaycast(touch.position);
            }
        }
    }

    private void HandleRaycast()
    {
        //TODO: Debug and fix handling objects even when controlling the UI element
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 2f, ~LayerMask.GetMask("UI")))
        {
            Debug.Log("Hit " + hit.collider.gameObject);
            switch (hit.collider.gameObject.tag)
            {
                case "Box":
                    HandleBoxInteraction(hit.collider.gameObject);
                    break;
                case "Mixture":
                    HandleMixtureInteraction(hit.collider.gameObject);
                    break;
                case "Shelf":
                    HandleShelfPlacement(hit.collider.gameObject);
                    break;
                case "Tool":
                    HandleToolInteraction(hit.collider.gameObject);
                    break;
                case "Use":
                    HandleUseInteraction(hit.collider.gameObject);
                    break;
            }
        }
    }

//for mobile interactions
private void HandleRaycast(Vector2 touchPosition)
    {
        //TODO: Debug and fix handling objects even when controlling the UI element
        Ray ray = playerCamera.ScreenPointToRay(touchPosition);
        
        if (Physics.Raycast(ray, out RaycastHit hit, 2f, ~LayerMask.GetMask("UI")))
        {
            switch (hit.collider.gameObject.tag)
            {
                case "Box":
                    HandleBoxInteraction(hit.collider.gameObject);
                    break;
                case "Mixture":
                    HandleMixtureInteraction(hit.collider.gameObject);
                    break;
                case "Shelf":
                    HandleShelfPlacement(hit.collider.gameObject);
                    break;
                case "Tool":
                    HandleToolInteraction(hit.collider.gameObject);
                    break;
                case "Use":
                    HandleUseInteraction(hit.collider.gameObject);
                    break;
            }
        }
    }

    private void HandleBoxInteraction(GameObject box)
    {
        Animator animator = box.GetComponentInChildren<Animator>();
        if (animator != null && !animator.GetBool("isClicked"))
        {
            animator.SetBool("isClicked", true);
        }
        else if (box.transform.Find("Cardboard Box"))
        {
            box.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    private void HandleMixtureInteraction(GameObject mixture)
    {

        if (objectGrabbable == null && mixture.TryGetComponent(out ItemPotion potion) && potion.interactable)
        {
            if (mixture.TryGetComponent(out objectGrabbable))
            {
                dropUIButton.SetActive(true);
                objectGrabbable.Grab(objectGrabPointTransform);
                dropButton.onClick.RemoveAllListeners();
                dropButton.onClick.AddListener(DropItem);

                mixtureChecklistUIButton.SetActive(true);
                mixtureChecklistButton.onClick.RemoveAllListeners();
                mixtureChecklistButton.onClick.AddListener(OpenMixtureChecklist);

                mixtureChecklist.SetCurrentMixture(mixture);

                OnItemPickUpOutline();
            }
        }
    }

    private void HandleShelfPlacement(GameObject shelf)
    {
        if (objectGrabbable == null) return;
        objectGrabbable.Drop();
        objectGrabbable.transform.position = shelf.transform.position;
        objectGrabbable = null;
        dropUIButton.SetActive(false);
        mixtureChecklistUIButton.SetActive(false);
    }

    private void HandleToolInteraction(GameObject toolObj)
    {

        ToolInteraction tool = toolObj.GetComponent<ToolInteraction>();
        ItemPotion item;
        //set item 
        if (objectGrabbable != null)
        {
            item = objectGrabbable.gameObject.GetComponent<ItemPotion>();
        }
        else
        {
            item = null;
        }

        //if player is not holding anything and workstation is done with the mixture
        if (objectGrabbable == null && tool.isFull && tool.interactable)
        {
            dropUIButton.SetActive(true);
            dropButton.onClick.RemoveAllListeners();
            dropButton.onClick.AddListener(DropItem);

            mixtureChecklistUIButton.SetActive(true);
            mixtureChecklistButton.onClick.RemoveAllListeners();
            mixtureChecklistButton.onClick.AddListener(OpenMixtureChecklist);

            objectGrabbable = toolObj.GetComponentInChildren<ObjectGrabbable>();
            objectGrabbable.Grab(objectGrabPointTransform);

            tool.isFull = false;
            Debug.Log("isFull set to false for " + toolObj.name);

            OnItemPickUpOutline();
            
            return;
        }

        if (objectGrabbable == null) return;

        //if mixture has been tested with that tool, do nothing
        if(item.testedWithFlashlight && tool.tools == ObjectPool.WorkTools.Flashlight) return;
        if(item.testedWithMicroscope && tool.tools == ObjectPool.WorkTools.Microscope) return;
        if(item.testedWithCentrifuge && tool.tools == ObjectPool.WorkTools.Centrifuge) return;

        //if player has a mixture, handle placing of mixture on workbench
        if (objectGrabbable != null && !tool.isFull && tool.interactable)
        {
            objectGrabbable.transform.SetParent(toolObj.transform, false);
            objectGrabbable.Drop();
            tool.ToolWhenClicked(objectGrabbable.GetComponent<ItemPotion>());
            objectGrabbable = null;
            dropUIButton.SetActive(false);
            mixtureChecklistUIButton.SetActive(false);
            tool.isFull = true;
            OnItemDropOutline();
            return;
        }
        
    }

    private void HandleUseInteraction(GameObject useObj)
    {
        //open panel if player has something on hand, otherwise throw error
        if(objectGrabbable == null) return;
        OpenUseChecklist();
    }

    private void DropItem()
    {
        objectGrabbable.Drop();
        objectGrabbable = null;
        dropUIButton.SetActive(false);
        mixtureChecklistUIButton.SetActive(false);


        OnItemDropOutline();
    }

    public void OnItemPickUpOutline()
    {
        ItemPotion item = null;
        if(objectGrabbable != null)
        {
            item = objectGrabbable.gameObject.GetComponent<ItemPotion>();
        }

        foreach (var outlineObject in outlineObjects)
        {
            // Check the outlineObject name and disable it if the mixture was tested with the corresponding workstation
            if ((item != null && item.testedWithMicroscope && outlineObject.name == "WorkStation") ||
                (item != null && item.testedWithCentrifuge && outlineObject.name == "WorkStation2") ||
                (item != null && item.testedWithFlashlight && outlineObject.name == "WorkStation3"))
            {
                continue; // Skip toggling this outlineObject
            }

            outlineObject.ToggleOutline(true);
        }
    }

    public void OnItemDropOutline()
    {
        foreach (var outlineObject in outlineObjects)
        {
            outlineObject.ToggleOutline(false);

        }
    }

    public void OpenMixtureChecklist()
    {
        mixtureChecklistCanvas.SetActive(true);
        mixtureChecklist.UpdateStatsOnUI(); 
    }


    public void CloseMixtureChecklist()
    {
        mixtureChecklistCanvas.SetActive(false);
    }

    public void OpenUseChecklist()
    {
        useChecklistCanvas.SetActive(true);
    }

    public void CloseUseChecklist()
    {
        useChecklistCanvas.SetActive(false);
    }   

public void HandleMixtureSubmission(UseInteraction mixtureSentToLab, int totalDays)
    {
        if (GameManager.Instance.IsPotionBeingProcessed()) // Prevent multiple submissions
        {
            Debug.Log("You must wait until the current potion is analyzed!");
            return;
        }
            ProcessMixture(mixtureSentToLab.gameObject, totalDays, mixtureSentToLab.GetSelectedChecks());

        

        objectGrabbable.transform.SetParent(useSlot, false);
        objectGrabbable.transform.localPosition = Vector3.zero;
        objectGrabbable.transform.rotation = Quaternion.identity;

        objectGrabbable.gameObject.SetActive(false);
        DropItem();
        CloseUseChecklist();
        dropUIButton.SetActive(false);
        mixtureChecklistUIButton.SetActive(false);

    }

public void ProcessMixture(GameObject mixtureObject, int days, Dictionary<string, bool> selectedChecks)
{
    if (mixtureSentToLab != null) 
    {
        Debug.Log("You must wait until the current potion is retrieved before sending another.");
        return;
    }

    // Assign the GameObject, not the script component
    mixtureSentToLab = mixtureObject;

    if (mixtureSentToLab.TryGetComponent(out UseInteraction mixture))
    {
        mixture.daysLeftToCompletion = days; // Store how many days it takes
        // StartCoroutine(WaitForRetrieval(mixture));
    }
}

// private IEnumerator WaitForRetrieval(UseInteraction mixture)
// {
//     while (mixture.daysLeftToCompletion > 0)
//     {
//         yield return new WaitForSeconds(1); // Simulating 1 day per second
//         mixture.daysLeftToCompletion--; // Countdown
//         Debug.Log("Days left for potion retrieval: " + mixture.daysLeftToCompletion);
//     }

//     Debug.Log("Potion is ready for retrieval!");
//     mixtureSentToLab = null; // Allow new potion submissions
// }


    public bool IsPotionBeingProcessed()
    {
        return mixtureSentToLab != null;
    }


}

