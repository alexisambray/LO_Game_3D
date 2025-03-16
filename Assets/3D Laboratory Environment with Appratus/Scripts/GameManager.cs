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
    public UseInteraction useInteraction;
    public int daysLeftToCompletion;
    private GameObject mixtureSentToLab; // Track the active mixture being processed
    public Transform useSlot;
    [SerializeField] private int mixturesPlacedInShelf = 0;
    [SerializeField] private int totalMixturesToPlace;

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
        CheckAllMixturesInFDA();
    }

    private void UpdateDayUI()
    {
        if (dayText != null)
        {
            dayText.text = $"Day: {dayCounter}";
        }
    }

    private void CheckAllMixturesInFDA()
    {
        ItemPotion[] mixtures = FindObjectsOfType<ItemPotion>(true);
        foreach(ItemPotion mixture in mixtures)
        {
            if(mixture.daysInFDA > 0)
            {
                mixture.daysInFDA--;
                Debug.Log($"Mixture {mixture.name} now has {mixture.daysInFDA} days left.");
            }

            if(mixture.daysInFDA == 0)
            {
                mixture.gameObject.SetActive(true);
                mixture.interactable = true;
            }
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
        if (Physics.Raycast(ray, out RaycastHit hit, 3f, ~LayerMask.GetMask("UI")))
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
        
        if (Physics.Raycast(ray, out RaycastHit hit, 3f, ~LayerMask.GetMask("UI")))
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
            Debug.Log("Get that thirty");
            dropUIButton.SetActive(true);
            dropButton.onClick.RemoveAllListeners();
            dropButton.onClick.AddListener(DropItem);

            mixtureChecklistUIButton.SetActive(true);
            mixtureChecklistButton.onClick.RemoveAllListeners();
            mixtureChecklistButton.onClick.AddListener(OpenMixtureChecklist);

            objectGrabbable = toolObj.GetComponentInChildren<ObjectGrabbable>();
            objectGrabbable.Grab(objectGrabPointTransform);

            Debug.Log($"Tool object: {toolObj.name}, isFull before: {tool.isFull}");
            tool.isFull = false;
            Debug.Log($"Tool object: {toolObj.name}, isFull after: {tool.isFull}");

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
        useInteraction.ClearSelectedChecks();
    }

    public void CloseUseChecklist()
    {
        useChecklistCanvas.SetActive(false);
    }   

public void HandleMixtureSubmission(UseInteraction mixtureSentToLab, int totalDays, Dictionary<string, bool> selectedChecks)
    {
        if (GameManager.Instance.IsPotionBeingProcessed()) // Prevent multiple submissions
        {
            Debug.Log("You must wait until the current potion is analyzed!");
            return;
        }
        if (objectGrabbable == null)
        {
        Debug.LogError("Mixture is null. Cannot process submission.");
        return;
        }

        ItemPotion potion = objectGrabbable.gameObject.GetComponent<ItemPotion>();
        potion.daysInFDA = totalDays;
        // Update test results based on selected toggles
        potion.testedForFoodAndBeverage = selectedChecks.ContainsKey("Food and Beverage");
        potion.testedForMedicine = selectedChecks.ContainsKey("Medicine");
        potion.testedForHouseCleaning = selectedChecks.ContainsKey("House Supplies");
        potion.testedForAgriculture = selectedChecks.ContainsKey("Agriculture");
        potion.testedForCosmetics = selectedChecks.ContainsKey("Cosmetic");
        potion.testedForPersonalHygiene = selectedChecks.ContainsKey("Personal Hygiene");

        Debug.Log($"Potion updated in GameManager: {potion.name}, Days in FDA: {potion.daysInFDA}");
        
        
        //TODO: Refactor this to allow a queue of boxes (Move mixture to it's respective slot for pickup)
        objectGrabbable.transform.SetParent(useSlot, false);
        objectGrabbable.transform.localPosition = Vector3.zero;
        objectGrabbable.transform.rotation = Quaternion.identity;

        //Disable mixture and UI
        objectGrabbable.gameObject.SetActive(false);
        DropItem();
        CloseUseChecklist();
        dropUIButton.SetActive(false);
        mixtureChecklistUIButton.SetActive(false);
        Debug.Log("Potion has been sent to the FDA!");
    }

    public bool IsPotionBeingProcessed()
    {
        return mixtureSentToLab != null;
    }

    public void ToggleMixtureChecklist()
    {
        if(mixtureChecklist != null)
        {
            mixtureChecklist.OpenAppearanceChecklistPanel();
        }
    }

    public void SetTotalMixtures(int total)
    {
        totalMixturesToPlace = total;
        mixturesPlacedInShelf = 0;
    }

    public void RegisterPlacedMixture()
    {
        mixturesPlacedInShelf++;

        if (mixturesPlacedInShelf == totalMixturesToPlace)
        {
            Debug.Log("All mixtures placed! Checking if they are correct...");
            ValidateShelfPlacement();
        }
    }

    private void ValidateShelfPlacement()
    {
        ItemPotion[] shelfMixtures = FindObjectsOfType<ItemPotion>();
        bool allCorrect = true;

        foreach (ItemPotion potion in shelfMixtures)
        {
            if (potion.CompareTag("Shelf"))
            {
                ItemPotion placedPotion = FindMatchingPotion(potion);

                if (placedPotion == null || !potion.DoesItemClickOnShelf(placedPotion, potion))
                {
                    allCorrect = false;
                    Debug.Log("Potion does not match the shelf.");
                    
                    // Handle incorrect placement
                    placedPotion.transform.position -= new Vector3(1f, 0, 0);
                    Rigidbody rigidBody = placedPotion.GetComponent<Rigidbody>();
                    rigidBody.isKinematic = false;
                    rigidBody.velocity = Vector3.zero;
                    rigidBody.angularVelocity = Vector3.zero;
                    placedPotion.transform.SetParent(null);

                    // Re-enable shelf interaction
                    potion.isFull = false;
                    potion.interactable = true;
                    placedPotion.interactable = true;
                }
            }
        }

        if (allCorrect)
        {
            Debug.Log("All mixtures are correctly placed!");
            // Add success logic here
        }
    }

    private ItemPotion FindMatchingPotion(ItemPotion shelfPotion)
    {
        ItemPotion[] allMixtures = FindObjectsOfType<ItemPotion>();

        foreach (ItemPotion mixture in allMixtures)
        {
            if (mixture.isFull && mixture.DoesItemClickOnShelf(mixture, shelfPotion))
            {
                return mixture;
            }
        }
        return null;
    }
}

