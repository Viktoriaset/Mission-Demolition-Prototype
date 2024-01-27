using UnityEngine;

public class Slingshot : MonoBehaviour
{
    private static Slingshot S;

    [Header("Set in inspector")]
    [SerializeField] private GameObject prefabProjectile;
    [SerializeField] private float velocityMult = 8f;

    [Space(5)]
    [Header("Set in dynamically")]
    private GameObject launchPoint;
    private Vector3 launchPos;
    private GameObject projectile;
    private Rigidbody projectileRB;
    private bool aimingMod;

    [SerializeField] private Transform rightArmEnd;
    [SerializeField] private Transform leftArmEnd;
    [SerializeField] private LineRenderer line;

    static public Vector3 LAUNCH_POS
    {
        get
        {
            if (S == null) return Vector3.zero;

            return S.launchPos;
        }
    }

    private void Awake()
    {
        S = this;

        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;

        leftArmEnd = transform.Find("LeftArm").Find("LeftArmEnd");
        rightArmEnd = transform.Find("RightArm").Find("RightArmEnd");
        line = GetComponent<LineRenderer>();
    }

    private void OnMouseEnter()
    {
        launchPoint.SetActive(true);
    }

    private void OnMouseExit()
    {
        launchPoint.SetActive(false);
    }

    private void OnMouseDown()
    {
        aimingMod = true;

        projectile = Instantiate(prefabProjectile);
        projectile.transform.position = launchPos;

        projectileRB = projectile.GetComponent<Rigidbody>();
        projectileRB.isKinematic = true;
    }

    private void Update()
    {
        if (!aimingMod)
        {
            DrawRubber();
            return;
        }

        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        Vector3 mouseDelta = mousePos3D - launchPos;

        float maxMagnitude = GetComponent<SphereCollider>().radius;
        if (mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;

        DrawRubberOnProjectile();

        if (Input.GetMouseButtonUp(0))
        {
            aimingMod = false;
            projectileRB.isKinematic = false;
            projectileRB.velocity = -mouseDelta * velocityMult;

            ProjectileLine.S.poi = null;

            FollowCam.POI = projectile;

            MissionDemolition.ShotFired();
            projectile = null;
        }
    }

    private void DrawRubber()
    {
        line.positionCount = 2;
        line.SetPosition(0, leftArmEnd.position);
        line.SetPosition(1, rightArmEnd.position);

        line.enabled = true;
    }

    private void DrawRubberOnProjectile()
    {
        float distanceX = projectile.transform.position.x - leftArmEnd.position.x;

        SphereCollider projectileSC = projectile.GetComponent<SphereCollider>();
        Vector3 linePosition = projectile.transform.position;
        if (distanceX > 0)
        {
            linePosition.x += projectileSC.radius;
        }
        else
        {
            linePosition.x -= projectileSC.radius;
        }

        line.positionCount = 4;
        line.SetPosition(0, leftArmEnd.position);
        line.SetPosition(1, new Vector3(linePosition.x, linePosition.y, rightArmEnd.position.z));
        line.SetPosition(2, new Vector3(linePosition.x, linePosition.y, leftArmEnd.position.z));
        line.SetPosition(3, rightArmEnd.position);

        line.enabled = true;
    }
}
