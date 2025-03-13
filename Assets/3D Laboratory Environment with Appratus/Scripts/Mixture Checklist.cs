using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static ObjectPool;

public class MixtureChecklist : MonoBehaviour
{
    ObjectGrabbable mixture;
    Canvas UI;
    public List<Toggle> checkToggles = new List<Toggle>();


    [Header("Checklist UI Elements")]   
    public GameObject appearanceChecklist;
    public GameObject useChecklist;
    

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


    [Header("Images Stored")]
    public Image trueSolutionFoodFlashlightImage;
    public Image trueSolutionFoodCentrifugeImage;
    public Image trueSolutionFoodMicroscopeImage;

    public Image trueSolutionMedicineFlashlightImage;
    public Image trueSolutionMedicineCentrifugeImage;
    public Image trueSolutionMedicineMicroscopeImage;

    public Image trueSolutionCosmeticsFlashlightImage;
    public Image trueSolutionCosmeticsCentrifugeImage;
    public Image trueSolutionCosmeticsMicroscopeImage;

    public Image trueSolutionCleaningFlashlightImage;
    public Image trueSolutionCleaningCentrifugeImage;
    public Image trueSolutionCleaningMicroscopeImage;

    public Image trueSolutionHygieneFlashlightImage;
    public Image trueSolutionHygieneCentrifugeImage;
    public Image trueSolutionHygieneMicroscopeImage;

    public Image trueSolutionAgricultureFlashlightImage;
    public Image trueSolutionAgricultureCentrifugeImage;
    public Image trueSolutionAgricultureMicroscopeImage;

    public Image suspensionFoodFlashlightImage;
    public Image suspensionFoodCentrifugeImage;
    public Image suspensionFoodMicroscopeImage;

    public Image suspensionMedicineFlashlightImage;
    public Image suspensionMedicineCentrifugeImage;
    public Image suspensionMedicineMicroscopeImage;

    public Image suspensionCosmeticsFlashlightImage;
    public Image suspensionCosmeticsCentrifugeImage;
    public Image suspensionCosmeticsMicroscopeImage;

    public Image suspensionCleaningFlashlightImage;
    public Image suspensionCleaningCentrifugeImage;
    public Image suspensionCleaningMicroscopeImage;

    public Image suspensionHygieneFlashlightImage;
    public Image suspensionHygieneCentrifugeImage;
    public Image suspensionHygieneMicroscopeImage;

    public Image suspensionAgricultureFlashlightImage;
    public Image suspensionAgricultureCentrifugeImage;
    public Image suspensionAgricultureMicroscopeImage;

    public Image colloidFoodFlashlightImage;
    public Image colloidFoodCentrifugeImage;
    public Image colloidFoodMicroscopeImage;

    public Image colloidMedicineFlashlightImage;
    public Image colloidMedicineCentrifugeImage;
    public Image colloidMedicineMicroscopeImage;

    public Image colloidCosmeticsFlashlightImage;
    public Image colloidCosmeticsCentrifugeImage;
    public Image colloidCosmeticsMicroscopeImage;

    public Image colloidCleaningFlashlightImage;
    public Image colloidCleaningCentrifugeImage;
    public Image colloidCleaningMicroscopeImage;

    public Image colloidHygieneFlashlightImage;
    public Image colloidHygieneCentrifugeImage;
    public Image colloidHygieneMicroscopeImage;

    public Image colloidAgricultureFlashlightImage;
    public Image colloidAgricultureCentrifugeImage;
    public Image colloidAgricultureMicroscopeImage;


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



public Image GetMixtureResultImage(Appearance appearance, Uses use, WorkTools tool)
{
    Dictionary<(Appearance, Uses, WorkTools), Image> mixtureImages = new Dictionary<(Appearance, Uses, WorkTools), Image>
    {
        // 🔵 True Solution
        { (Appearance.TrueSolution, Uses.FoodAndBeverage, WorkTools.Flashlight), trueSolutionFoodFlashlightImage },
        { (Appearance.TrueSolution, Uses.FoodAndBeverage, WorkTools.Centrifuge), trueSolutionFoodCentrifugeImage },
        { (Appearance.TrueSolution, Uses.FoodAndBeverage, WorkTools.Microscope), trueSolutionFoodMicroscopeImage },

        { (Appearance.TrueSolution, Uses.Medicine, WorkTools.Flashlight), trueSolutionMedicineFlashlightImage },
        { (Appearance.TrueSolution, Uses.Medicine, WorkTools.Centrifuge), trueSolutionMedicineCentrifugeImage },
        { (Appearance.TrueSolution, Uses.Medicine, WorkTools.Microscope), trueSolutionMedicineMicroscopeImage },

        { (Appearance.TrueSolution, Uses.Cosmetics, WorkTools.Flashlight), trueSolutionCosmeticsFlashlightImage },
        { (Appearance.TrueSolution, Uses.Cosmetics, WorkTools.Centrifuge), trueSolutionCosmeticsCentrifugeImage },
        { (Appearance.TrueSolution, Uses.Cosmetics, WorkTools.Microscope), trueSolutionCosmeticsMicroscopeImage },

        { (Appearance.TrueSolution, Uses.HouseCleaning, WorkTools.Flashlight), trueSolutionCleaningFlashlightImage },
        { (Appearance.TrueSolution, Uses.HouseCleaning, WorkTools.Centrifuge), trueSolutionCleaningCentrifugeImage },
        { (Appearance.TrueSolution, Uses.HouseCleaning, WorkTools.Microscope), trueSolutionCleaningMicroscopeImage },

        { (Appearance.TrueSolution, Uses.PersonalHygiene, WorkTools.Flashlight), trueSolutionHygieneFlashlightImage },
        { (Appearance.TrueSolution, Uses.PersonalHygiene, WorkTools.Centrifuge), trueSolutionHygieneCentrifugeImage },
        { (Appearance.TrueSolution, Uses.PersonalHygiene, WorkTools.Microscope), trueSolutionHygieneMicroscopeImage },

        { (Appearance.TrueSolution, Uses.Agriculture, WorkTools.Flashlight), trueSolutionAgricultureFlashlightImage },
        { (Appearance.TrueSolution, Uses.Agriculture, WorkTools.Centrifuge), trueSolutionAgricultureCentrifugeImage },
        { (Appearance.TrueSolution, Uses.Agriculture, WorkTools.Microscope), trueSolutionAgricultureMicroscopeImage },

        // 🔴 Suspension
        { (Appearance.Suspension, Uses.FoodAndBeverage, WorkTools.Flashlight), suspensionFoodFlashlightImage },
        { (Appearance.Suspension, Uses.FoodAndBeverage, WorkTools.Centrifuge), suspensionFoodCentrifugeImage },
        { (Appearance.Suspension, Uses.FoodAndBeverage, WorkTools.Microscope), suspensionFoodMicroscopeImage },

        { (Appearance.Suspension, Uses.Medicine, WorkTools.Flashlight), suspensionMedicineFlashlightImage },
        { (Appearance.Suspension, Uses.Medicine, WorkTools.Centrifuge), suspensionMedicineCentrifugeImage },
        { (Appearance.Suspension, Uses.Medicine, WorkTools.Microscope), suspensionMedicineMicroscopeImage },

        { (Appearance.Suspension, Uses.Cosmetics, WorkTools.Flashlight), suspensionCosmeticsFlashlightImage },
        { (Appearance.Suspension, Uses.Cosmetics, WorkTools.Centrifuge), suspensionCosmeticsCentrifugeImage },
        { (Appearance.Suspension, Uses.Cosmetics, WorkTools.Microscope), suspensionCosmeticsMicroscopeImage },

        { (Appearance.Suspension, Uses.HouseCleaning, WorkTools.Flashlight), suspensionCleaningFlashlightImage },
        { (Appearance.Suspension, Uses.HouseCleaning, WorkTools.Centrifuge), suspensionCleaningCentrifugeImage },
        { (Appearance.Suspension, Uses.HouseCleaning, WorkTools.Microscope), suspensionCleaningMicroscopeImage },

        { (Appearance.Suspension, Uses.PersonalHygiene, WorkTools.Flashlight), suspensionHygieneFlashlightImage },
        { (Appearance.Suspension, Uses.PersonalHygiene, WorkTools.Centrifuge), suspensionHygieneCentrifugeImage },
        { (Appearance.Suspension, Uses.PersonalHygiene, WorkTools.Microscope), suspensionHygieneMicroscopeImage },

        { (Appearance.Suspension, Uses.Agriculture, WorkTools.Flashlight), suspensionAgricultureFlashlightImage },
        { (Appearance.Suspension, Uses.Agriculture, WorkTools.Centrifuge), suspensionAgricultureCentrifugeImage },
        { (Appearance.Suspension, Uses.Agriculture, WorkTools.Microscope), suspensionAgricultureMicroscopeImage },

        // 🟢 Colloid
        { (Appearance.Colloid, Uses.FoodAndBeverage, WorkTools.Flashlight), colloidFoodFlashlightImage },
        { (Appearance.Colloid, Uses.FoodAndBeverage, WorkTools.Centrifuge), colloidFoodCentrifugeImage },
        { (Appearance.Colloid, Uses.FoodAndBeverage, WorkTools.Microscope), colloidFoodMicroscopeImage },

        { (Appearance.Colloid, Uses.Medicine, WorkTools.Flashlight), colloidMedicineFlashlightImage },
        { (Appearance.Colloid, Uses.Medicine, WorkTools.Centrifuge), colloidMedicineCentrifugeImage },
        { (Appearance.Colloid, Uses.Medicine, WorkTools.Microscope), colloidMedicineMicroscopeImage },

        { (Appearance.Colloid, Uses.Cosmetics, WorkTools.Flashlight), colloidCosmeticsFlashlightImage },
        { (Appearance.Colloid, Uses.Cosmetics, WorkTools.Centrifuge), colloidCosmeticsCentrifugeImage },
        { (Appearance.Colloid, Uses.Cosmetics, WorkTools.Microscope), colloidCosmeticsMicroscopeImage },

        { (Appearance.Colloid, Uses.HouseCleaning, WorkTools.Flashlight), colloidCleaningFlashlightImage },
        { (Appearance.Colloid, Uses.HouseCleaning, WorkTools.Centrifuge), colloidCleaningCentrifugeImage },
        { (Appearance.Colloid, Uses.HouseCleaning, WorkTools.Microscope), colloidCleaningMicroscopeImage },

        { (Appearance.Colloid, Uses.PersonalHygiene, WorkTools.Flashlight), colloidHygieneFlashlightImage },
        { (Appearance.Colloid, Uses.PersonalHygiene, WorkTools.Centrifuge), colloidHygieneCentrifugeImage },
        { (Appearance.Colloid, Uses.PersonalHygiene, WorkTools.Microscope), colloidHygieneMicroscopeImage },

        { (Appearance.Colloid, Uses.Agriculture, WorkTools.Flashlight), colloidAgricultureFlashlightImage },
        { (Appearance.Colloid, Uses.Agriculture, WorkTools.Centrifuge), colloidAgricultureCentrifugeImage },
        { (Appearance.Colloid, Uses.Agriculture, WorkTools.Microscope), colloidAgricultureMicroscopeImage },
    };

    // Return the correct image if found
    if (mixtureImages.TryGetValue((appearance, use, tool), out Image resultImage))
    {
        return resultImage;
    }

    return null; // No image found
}

}
