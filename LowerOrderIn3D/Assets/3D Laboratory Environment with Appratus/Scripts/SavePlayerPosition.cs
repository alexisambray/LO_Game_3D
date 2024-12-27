using UnityEngine;

public class SavePlayerPosition : MonoBehaviour
{
    public static void SavePosition(Vector3 position, GameObject Player)
    {
        PlayerPrefs.SetFloat("PlayerX", position.x);
        PlayerPrefs.SetFloat("PlayerY", position.y);
        PlayerPrefs.SetFloat("PlayerZ", position.z);
        PlayerPrefs.Save();
        Debug.Log($"Saved {Player.name} at {position}");
    }
}
