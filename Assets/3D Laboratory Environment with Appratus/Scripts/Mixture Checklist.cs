using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static ObjectPool;

public class MixtureChecklist : MonoBehaviour
{
    ObjectGrabbable mixture;
    Canvas UI;
    public ImageManager imageManager;
    public List<Toggle> checkToggles = new List<Toggle>();


    [Header("Checklist UI Elements")]   
    public GameObject appearanceChecklist;
    public GameObject useChecklist;
    public GameObject extendedAppearanceChecklist;
    public Image resultImage;
    public Text resultText;
    

    [Header("Appearance Button Elements")]
    public Button flashlightButton;
    public GameObject flashlightPanel;
    public Image flashlightResultImage; 

    public Button centrifugeButton;
    public GameObject centrifugePanel;
    public Image centrifugeResultImage; 
   
    public Button microscopeButton;
    public GameObject microscopePanel;
    public Image microscopeResultImage; 

    [Header("Uses Toggle Elements")]
    public GameObject foodAndBeverageCheckMark;
    public GameObject foodAndBeverageXMark;

    public GameObject medicineCheckMark;
    public GameObject medicineXMark;

    public GameObject cosmeticsCheckMark;
    public GameObject cosmeticsXMark;

    public GameObject houseCleaningCheckMark;
    public GameObject houseCleaningXMark;

    public GameObject personalHygieneCheckMark;
    public GameObject personalHygieneXMark;

    public GameObject agricultureCheckMark;
    public GameObject agricultureXMark;

    void Start()
    {
        checkToggles = GetComponentsInChildren<Toggle>().ToList();
    }

    public void OpenUI()
    {
        UI.gameObject.SetActive(true);
        UpdateStatsOnUI();
    }

    public void CloseUI()
    {
        UI.gameObject.SetActive(false);
        removeTickMarks();
    }

    public void UpdateStatsOnUI()
    {
        removeTickMarks();
        useChecklist.SetActive(true);
        extendedAppearanceChecklist.SetActive(false);
        
        ItemPotion currentPotion = mixture.gameObject.GetComponent<ItemPotion>();

        //update appearance and use status
        if(currentPotion != null)
        {
            //appearance
            Debug.Log("update stats" + currentPotion.appearance + currentPotion.uses);
            //get current status of tools the mixture is already tested with
            flashlightButton.interactable = currentPotion.testedWithFlashlight;
            centrifugeButton.interactable = currentPotion.testedWithCentrifuge;
            microscopeButton.interactable = currentPotion.testedWithMicroscope;

            //use 
            UpdateToggleStatus(currentPotion);
        }
    }

public void removeTickMarks()
{
    //remove uses
    Debug.Log($"Child: " + useChecklist.transform.GetChild(0).GetChild(0).GetChild(0).gameObject);
    useChecklist.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(false);
    useChecklist.transform.GetChild(1).GetChild(0).GetChild(0).gameObject.SetActive(false);
    useChecklist.transform.GetChild(2).GetChild(0).GetChild(0).gameObject.SetActive(false);
    useChecklist.transform.GetChild(3).GetChild(0).GetChild(0).gameObject.SetActive(false);
    useChecklist.transform.GetChild(4).GetChild(0).GetChild(0).gameObject.SetActive(false);
    useChecklist.transform.GetChild(5).GetChild(0).GetChild(0).gameObject.SetActive(false);

    appearanceChecklist.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(false);
    appearanceChecklist.transform.GetChild(1).GetChild(0).GetChild(0).gameObject.SetActive(false);
    appearanceChecklist.transform.GetChild(2).GetChild(0).GetChild(0).gameObject.SetActive(false);
}

public void SetCurrentMixture(GameObject mixtureObject)
{
    if (mixtureObject.TryGetComponent(out ItemPotion potion))
    {
        this.mixture = mixtureObject.GetComponent<ObjectGrabbable>(); // Store the mixture reference
        Debug.Log("Mixture set to checklist!");
        UpdateStatsOnUI();
    }
}

public void UpdateToggleStatus(ItemPotion currentPotion)
{
    // Dictionary to map uses to their check and X mark GameObjects
    Dictionary<Uses, (GameObject checkMark, GameObject xMark)> toggleObjects = new Dictionary<Uses, (GameObject, GameObject)>
    {
        { Uses.FoodAndBeverage, (foodAndBeverageCheckMark, foodAndBeverageXMark) },
        { Uses.Medicine, (medicineCheckMark, medicineXMark) },
        { Uses.Cosmetics, (cosmeticsCheckMark, cosmeticsXMark) },
        { Uses.HouseCleaning, (houseCleaningCheckMark, houseCleaningXMark) },
        { Uses.PersonalHygiene, (personalHygieneCheckMark, personalHygieneXMark) },
        { Uses.Agriculture, (agricultureCheckMark, agricultureXMark) }
    };

    // Dictionary to store whether each use was tested
    Dictionary<Uses, bool> testResults = new Dictionary<Uses, bool>
    {
        { Uses.FoodAndBeverage, currentPotion.testedForFoodAndBeverage },
        { Uses.Medicine, currentPotion.testedForMedicine },
        { Uses.HouseCleaning, currentPotion.testedForHouseCleaning },
        { Uses.Agriculture, currentPotion.testedForAgriculture },
        { Uses.Cosmetics, currentPotion.testedForCosmetics },
        { Uses.PersonalHygiene, currentPotion.testedForPersonalHygiene }
    };

    foreach (var use in toggleObjects.Keys)
    {
        bool wasTested = testResults[use]; // Check if the potion was tested for this use
        bool isMatch = currentPotion.uses == use; // Check if this use matches the potion's intended use

        GameObject checkMark = toggleObjects[use].checkMark;
        GameObject xMark = toggleObjects[use].xMark;

        if (wasTested)
        {
            checkMark.SetActive(isMatch); // Show ✅ if it matches
            xMark.SetActive(!isMatch); // Show ❌ if it doesn't match
        }
        else
        {
            checkMark.SetActive(false); // Hide both if not tested
            xMark.SetActive(false);
        }
    }
}

public void UpdateToggleGraphics(ItemPotion currentPotion, Dictionary<string, Toggle> checkToggles, Sprite checkMark, Sprite xMark, Sprite blank)
{
    // Dictionary linking potion uses to test results
    Dictionary<Uses, bool> testResults = new Dictionary<Uses, bool>
    {
        { Uses.FoodAndBeverage, currentPotion.testedForFoodAndBeverage },
        { Uses.Medicine, currentPotion.testedForMedicine },
        { Uses.HouseCleaning, currentPotion.testedForHouseCleaning },
        { Uses.Agriculture, currentPotion.testedForAgriculture },
        { Uses.Cosmetics, currentPotion.testedForCosmetics },
        { Uses.PersonalHygiene, currentPotion.testedForPersonalHygiene }
    };

    foreach (var pair in checkToggles)
    {
        string toggleName = pair.Key;
        Toggle toggle = pair.Value;

        if (Enum.TryParse(toggleName, out Uses toggleUse) && testResults.ContainsKey(toggleUse))
        {
            bool wasTested = testResults[toggleUse];

            if (wasTested)
            {
                bool isCorrect = (currentPotion.uses == toggleUse);
                toggle.image.sprite = isCorrect ? checkMark : xMark; // ✅ or ❌
            }
            else
            {
                toggle.image.sprite = blank; // No test done
            }
        }
    }
}

public void OpenAppearanceChecklistPanel()
{
    //opens the detailed panel and closes the other one. 
    bool isExtendendPanelOpen = extendedAppearanceChecklist.activeSelf;

    extendedAppearanceChecklist.SetActive(!isExtendendPanelOpen);
    useChecklist.SetActive(isExtendendPanelOpen);

    //updates and assign correct image depending on the mixture results
    ItemPotion potion = null;
    if(mixture != null)
    {
        potion = mixture.gameObject.GetComponent<ItemPotion>();
    }

    if (potion != null)
    {
        foreach (WorkTools tool in System.Enum.GetValues(typeof(WorkTools)))
        {
            UpdateChecklist(potion, tool);
        }
    }
}

public void OpenAppearanceChecklistPanelForMicroscope()
{
    //opens the detailed panel and closes the other one. 
    bool isExtendendPanelOpen = extendedAppearanceChecklist.activeSelf;

    extendedAppearanceChecklist.SetActive(!isExtendendPanelOpen);
    useChecklist.SetActive(isExtendendPanelOpen);

    //updates and assign correct image depending on the mixture results
    ItemPotion potion = null;
    if(mixture != null)
    {
        potion = mixture.gameObject.GetComponent<ItemPotion>();
    }

    if (potion != null)
    {
        UpdateChecklist(potion, WorkTools.Microscope);
    }
}

public void OpenAppearanceChecklistPanelForFlashlight()
{
    //opens the detailed panel and closes the other one. 
    bool isExtendendPanelOpen = extendedAppearanceChecklist.activeSelf;

    extendedAppearanceChecklist.SetActive(!isExtendendPanelOpen);
    useChecklist.SetActive(isExtendendPanelOpen);

    //updates and assign correct image depending on the mixture results
    ItemPotion potion = null;
    if(mixture != null)
    {
        potion = mixture.gameObject.GetComponent<ItemPotion>();
    }

    if (potion != null)
    {
        UpdateChecklist(potion, WorkTools.Flashlight);
    }
}

public void OpenAppearanceChecklistPanelForCentrifuge()
{
    //opens the detailed panel and closes the other one. 
    bool isExtendendPanelOpen = extendedAppearanceChecklist.activeSelf;

    extendedAppearanceChecklist.SetActive(!isExtendendPanelOpen);
    useChecklist.SetActive(isExtendendPanelOpen);

    //updates and assign correct image depending on the mixture results
    ItemPotion potion = null;
    if(mixture != null)
    {
        potion = mixture.gameObject.GetComponent<ItemPotion>();
    }

    if (potion != null)
    {
        UpdateChecklist(potion, WorkTools.Centrifuge);
    }
}

public void UpdateChecklist(ItemPotion potion, WorkTools workTools)
{
    if (imageManager == null)
    {
    Debug.LogError("ImageManager is not assigned!");
    return;
    }

    if (imageManager != null && potion != null)
    {
        resultImage.sprite = imageManager.GetImage(potion.appearance, potion.uses, workTools);
        resultText.text = imageManager.GetText(potion.appearance, potion.uses, workTools);
    }
}

}
