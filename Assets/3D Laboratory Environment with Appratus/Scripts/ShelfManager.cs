using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfManager : MonoBehaviour
{
    public static ShelfManager Instance;
    public int totalShelves = 18;
    private Dictionary<int, GameObject> shelfSlots = new Dictionary<int, GameObject>();
    private int placedCount = 0;

    // True Solution (TS)
    public GameObject TS_FB;   // TrueSolution_FoodAndBeverage
    public GameObject TS_Med;  // TrueSolution_Medicine
    public GameObject TS_C;  // TrueSolution_Cosmetics
    public GameObject TS_HC;   // TrueSolution_HouseCleaning
    public GameObject TS_PH;   // TrueSolution_PersonalHygiene
    public GameObject TS_A; // TrueSolution_Agriculture

    // Suspension (S)
    public GameObject S_FB;   // Suspension_FoodAndBeverage
    public GameObject S_Med;  // Suspension_Medicine
    public GameObject S_C;  // Suspension_Cosmetics
    public GameObject S_HC;   // Suspension_HouseCleaning
    public GameObject S_PH;   // Suspension_PersonalHygiene
    public GameObject S_A; // Suspension_Agriculture

    // Colloid (C)
    public GameObject C_FB;   // Colloid_FoodAndBeverage
    public GameObject C_Med;  // Colloid_Medicine
    public GameObject C_C;  // Colloid_Cosmetics
    public GameObject C_HC;   // Colloid_HouseCleaning
    public GameObject C_PH;   // Colloid_PersonalHygiene
    public GameObject C_A; // Colloid_Agriculture

    private Dictionary<int, GameObject> correctPositions = new Dictionary<int, GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeCorrectPositions()
    {
        // Define the correct placements for each slot
        correctPositions[0] = TS_FB;
        correctPositions[1] = TS_Med;
        correctPositions[2] = TS_C;
        correctPositions[3] = TS_HC;
        correctPositions[4] = TS_PH;
        correctPositions[5] = TS_A;

        correctPositions[6] = S_FB;
        correctPositions[7] = S_Med;
        correctPositions[8] = S_C;
        correctPositions[9] = S_HC;
        correctPositions[10] = S_PH;
        correctPositions[11] = S_A;

        correctPositions[12] = C_FB;
        correctPositions[13] = C_Med;
        correctPositions[14] = C_C;
        correctPositions[15] = C_HC;
        correctPositions[16] = C_PH;
        correctPositions[17] = C_A;
    }

    public bool PlaceMixture(int slotIndex, GameObject mixture)
    {
        if (shelfSlots.ContainsKey(slotIndex))
        {
            Debug.Log("Slot already occupied!");
            return false;
        }

        shelfSlots[slotIndex] = mixture;
        placedCount++;

        if (placedCount == totalShelves)
        {
            CheckMixtures();
        }

        return true;
    }

    public void RemoveMixture(int slotIndex)
    {
        if (shelfSlots.ContainsKey(slotIndex))
        {
            shelfSlots.Remove(slotIndex);
            placedCount--;
        }
    }

    private void CheckMixtures()
    {
        List<int> incorrectSlots = new List<int>();

        foreach (var slot in shelfSlots)
        {
            int slotIndex = slot.Key;
            GameObject mixture = slot.Value;

            if (!IsCorrectPlacement(slotIndex, mixture))
            {
                incorrectSlots.Add(slotIndex);
            }
            else
            {
                // Correctly placed mixtures cannot be moved
                Rigidbody rb = mixture.GetComponent<Rigidbody>();
                if (rb != null) rb.isKinematic = true;
            }
        }

        // Remove incorrect mixtures
        foreach (int slotIndex in incorrectSlots)
        {
            GameObject mixture = shelfSlots[slotIndex];
            Rigidbody rb = mixture.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.isKinematic = false;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            RemoveMixture(slotIndex);
        }
    }

    private bool IsCorrectPlacement(int slotIndex, GameObject mixture)
    {
        return correctPositions.ContainsKey(slotIndex) && shelfSlots[slotIndex] == correctPositions[slotIndex];
    }

}
