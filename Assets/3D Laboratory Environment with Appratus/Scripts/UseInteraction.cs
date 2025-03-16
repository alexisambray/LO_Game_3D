using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseInteraction : MonoBehaviour
{
    public GameObject mixturePrefab; // Assign the mixture prefab in Unity
    public List<Toggle> checkToggles; // Assign toggles for user selections

    public Toggle FoodAndBeverage;
    public Toggle Medicine;
    public Toggle Cosmetics;
    public Toggle HouseCleaning;
    public Toggle PersonalHygiene;
    public Toggle Agriculture;
    public ItemPotion potion;


    private Dictionary<string, int> toggleDays = new Dictionary<string, int>(); // Store days per check
    private Dictionary<string, bool> selectedChecks = new Dictionary<string, bool>(); // Track which checks were done
    public int daysLeftToCompletion = 0;
    public Button sendMixtureButton;

    // Start is called before the first frame update
    void Start()
    {
         // Assign names and days for different checks
        toggleDays.Add("Food and Beverage", 1);
        toggleDays.Add("House Supplies", 1);
        toggleDays.Add("Agriculture", 1);
        toggleDays.Add("Medicine", 1);
        toggleDays.Add("Personal Hygiene", 1);
        toggleDays.Add("Cosmetic", 1);
        
        // Debug: Check if toggles are assigned
        foreach (var toggle in checkToggles)
        {
            toggle.isOn = false;
            Debug.Log($"Toggle assigned: {toggle.name}");
        }

        // Assign button click listener
        if (sendMixtureButton != null)
        {
            sendMixtureButton.onClick.AddListener(SendMixtureToScientist);
        }
    }

    public void ClearSelectedChecks()
    {
        foreach (var toggle in checkToggles)
        {
            toggle.isOn = false;
        }
    }

    public void SendMixtureToScientist()
    {
        daysLeftToCompletion = CalculateTotalDays();
        GameManager.Instance.HandleMixtureSubmission(this, daysLeftToCompletion, selectedChecks);
    }

        // Calculate total retrieval days based on selected toggles
    public int CalculateTotalDays()
    {
        int totalDays = 0;
        selectedChecks.Clear();

        foreach (var toggle in checkToggles) // Use the actual list
        {
            if (toggle == null) continue; // Prevent null errors

            Debug.Log($"{toggle.name} isOn BEFORE: {toggle.isOn}");

            if (toggle.isOn)
            {
                selectedChecks[toggle.name] = true;
                totalDays++;
            }

            Debug.Log($"{toggle.name} isOn AFTER: {toggle.isOn}");
        }

        return totalDays;
    }

    public Dictionary<string, bool> GetSelectedChecks()
    {
        return new Dictionary<string, bool>(selectedChecks);
    }
}
