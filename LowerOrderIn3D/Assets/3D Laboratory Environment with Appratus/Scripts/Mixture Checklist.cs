using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ObjectPool;

public class MixtureChecklist : MonoBehaviour
{
    ObjectGrabbable mixture;
    Canvas UI;
    GameObject appearanceChecklist;
    GameObject useChecklist;

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
        //clear UI
        removeTickMarks();
        //ticks the checkmarks based on what the player is holding
        ItemPotion stats = mixture.gameObject.GetComponent<ItemPotion>();
        if(stats != null && stats.UseFound)
        {
            if(stats.uses == Uses.FoodAndBeverage)
            {
                useChecklist.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
            }
            else if(stats.uses == Uses.FoodAndBeverage)
            {
                useChecklist.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
            }
            else if(stats.uses == Uses.FoodAndBeverage)
            {
                useChecklist.transform.GetChild(2).GetChild(1).gameObject.SetActive(true);
            }
            else if(stats.uses == Uses.FoodAndBeverage)
            {
                useChecklist.transform.GetChild(3).GetChild(1).gameObject.SetActive(true);
            }
            else if(stats.uses == Uses.FoodAndBeverage)
            {
                useChecklist.transform.GetChild(4).GetChild(1).gameObject.SetActive(true);
            }
            else if(stats.uses == Uses.FoodAndBeverage)
            {
                useChecklist.transform.GetChild(5).GetChild(1).gameObject.SetActive(true);
            }
        }

        else if (stats != null && stats.AppearanceFound)
        {
            if(stats.appearance == Appearance.TrueSolution)
            {
                appearanceChecklist.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
            }
            else if(stats.appearance == Appearance.Suspension)
            {
                appearanceChecklist.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
            }
            else if(stats.appearance == Appearance.Colloid)
            {
                appearanceChecklist.transform.GetChild(2).GetChild(1).gameObject.SetActive(true);
            }
        }
    }

public void removeTickMarks()
{
    //remove uses
    useChecklist.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
    useChecklist.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
    useChecklist.transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
    useChecklist.transform.GetChild(3).GetChild(1).gameObject.SetActive(false);
    useChecklist.transform.GetChild(4).GetChild(1).gameObject.SetActive(false);
    useChecklist.transform.GetChild(5).GetChild(1).gameObject.SetActive(false);

    //remove appearance
    appearanceChecklist.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
    appearanceChecklist.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
    appearanceChecklist.transform.GetChild(2).GetChild(1).gameObject.SetActive(false);

}

}
