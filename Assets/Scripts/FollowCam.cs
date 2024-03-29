using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI;

    [Header("Set in inspector")]
    [SerializeField] private float easing = 0.05f;
    [SerializeField] private Vector2 minXY = Vector2.zero;

    [Header("Set dynamically")]
    public float camZ;


    private void Awake()
    {
        camZ = transform.position.z;
    }

    private void FixedUpdate()
    {
        Vector3 destination;
        if (POI == null)
        {
            destination = Vector3.zero;
        }
        else
        {
            destination = POI.transform.position;

            if (POI.tag == "Projectile")
            {
                if (POI.GetComponent<Rigidbody>().IsSleeping())
                {
                    POI = null;
                    return;
                }
            }
        }

        destination = Vector3.Lerp(transform.position, destination, easing);

        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);

        destination.z = camZ;

        transform.position = destination;

        Camera.main.orthographicSize = destination.y + 10;
    }
}
