using System.Collections;
using UnityEngine;

public class LoadPlayerPosition : MonoBehaviour
{
    IEnumerator LoadPositionAfterFrame()
    {
        yield return null; // Wait for one frame
        SetPlayerPosition();
    }

    private void Start()
    {
        StartCoroutine(LoadPositionAfterFrame());
    }


    private void SetPlayerPosition()
    {
        Vector3 newPosition;

        if (PlayerPrefs.HasKey("PlayerX") && PlayerPrefs.HasKey("PlayerY") && PlayerPrefs.HasKey("PlayerZ"))
        {
            float x = PlayerPrefs.GetFloat("PlayerX");
            float y = PlayerPrefs.GetFloat("PlayerY");
            float z = PlayerPrefs.GetFloat("PlayerZ");
            newPosition = new Vector3(x, y, z);

            Debug.Log("Position Loaded: " + newPosition + " for GameObject: " + gameObject.name);
        }
        else
        {
            newPosition = new Vector3(-50.0f, 0.65f, -15.0f);
            Debug.Log("No saved position found. Using default: " + newPosition + " for GameObject: " + gameObject.name);
        }
    }
}
