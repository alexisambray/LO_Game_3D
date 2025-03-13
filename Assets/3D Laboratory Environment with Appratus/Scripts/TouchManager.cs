using UnityEngine;

public class TouchManager : MonoBehaviour
{
    public bool UIisOpen = false;
    private MovementController movementController;

    private void Start()
    {
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            movementController = player.GetComponent<MovementController>();
        }
    }

    private void Update()
    {
        if (movementController != null)
        {
            movementController.enabled = !UIisOpen;
        }
    }

    public void onUIOpen()
    {
        UIisOpen = true;
    }

    public void onUIClose()
    {
        UIisOpen = false;
    }
}
