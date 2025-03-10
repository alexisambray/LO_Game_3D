using UnityEngine;

public class OutlineController : MonoBehaviour
{
    public Transform player;
    private Outline outline;
    private float pulseSpeed = 0.5f;
    private float maxWidth = 7f;
    private float minWidth = 2f;
    private float farDistanceWidth = 1.5f;
    private bool isPulsing = false;
    private float pulseTimer = 0f;

    void Start()
    {
        outline = GetComponent<Outline>();
        if(player == null)
        {
            player = GameObject.Find("Player").transform;
        }
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= 10f)
        {
            if (!isPulsing)
            {
                isPulsing = true;
                pulseTimer = 0f;
            }


            pulseTimer += Time.deltaTime * pulseSpeed;
            float newWidth = Mathf.Lerp(minWidth, maxWidth, Mathf.PingPong(pulseTimer, 1));
            outline.OutlineWidth = newWidth;
        }
        else
        {
            if (isPulsing)
            {
                isPulsing = false;
                outline.OutlineWidth = farDistanceWidth;
            }
        }
    }

    void ToggleOutline(ObjectGrabbable objectGrabbable)
    {
        //only enable outline when player picks up the object
        if(objectGrabbable == null)
        {
            outline.enabled = false;
        }
        else
        {
            outline.enabled = true;
        }
    }
}
