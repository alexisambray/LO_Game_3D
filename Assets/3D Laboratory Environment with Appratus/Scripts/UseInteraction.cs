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


    private Dictionary<string, int> toggleDays = new Dictionary<string, int>(); // Store days per check
    private Dictionary<string, bool> selectedChecks = new Dictionary<string, bool>(); // Track which checks were done
    public int daysLeftToCompletion = 0;
    public Button sendMixtureButton;

    // Start is called before the first frame update
    void Start()
    {
         // Assign names and days for different checks
        toggleDays.Add("Food and Beverage", 1);
        toggleDays.Add("House Supplies", 2);
        toggleDays.Add("Agriculture", 3);
        toggleDays.Add("Medicine", 4);
        toggleDays.Add("Personal Hygiene", 3);
        toggleDays.Add("Cosmetic", 2);

        // Debug: Check if toggles are assigned
        foreach (var toggle in checkToggles)
        {
            Debug.Log($"Toggle assigned: {toggle.name}");
        }

        // Assign button click listener
        if (sendMixtureButton != null)
        {
            sendMixtureButton.onClick.AddListener(SendMixtureToScientist);
        }
    }

    public void SendMixtureToScientist()
    {
        GameManager.Instance.HandleMixtureSubmission(this, daysLeftToCompletion);
    }

    // Calculate total retrieval days based on selected toggles
    public int CalculateTotalDays()
{
    int totalDays = 0;
    selectedChecks.Clear();

    // Dictionary to map toggles to their corresponding names and days
    Dictionary<Toggle, (string name, int days)> toggleMappings = new Dictionary<Toggle, (string, int)>
    {
        { FoodAndBeverage, ("Food and Beverage", 1) },
        { Medicine, ("Medicine", 4) },
        { Cosmetics, ("Cosmetic", 2) },
        { HouseCleaning, ("House Supplies", 2) },
        { PersonalHygiene, ("Personal Hygiene", 3) },
        { Agriculture, ("Agriculture", 3) }
    };

    foreach (var toggleEntry in toggleMappings)
    {
        Toggle toggle = toggleEntry.Key;
        string checkName = toggleEntry.Value.name;
        int days = toggleEntry.Value.days;

        if (toggle.isOn) // If the toggle is checked
        {
            selectedChecks[checkName] = true;
            totalDays += days;
        }
    }

    return totalDays;
}


  public void RetrieveMixture()
    {
        if (daysLeftToCompletion > 0)
        {
            Debug.Log("Mixture is still being processed! " + daysLeftToCompletion + " days left.");
            return;
        }

        if (mixturePrefab != null)
        {
            mixturePrefab.SetActive(true); // Show potion again
            Debug.Log("Retrieved potion after testing: " + string.Join(", ", selectedChecks.Keys));
        }
    }

    public Dictionary<string, bool> GetSelectedChecks()
    {
        return new Dictionary<string, bool>(selectedChecks);
    }
}
