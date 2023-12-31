using System.Collections;
using System.Collections.Generic;
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
        if (!aimingMod) return;

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

        if (Input.GetMouseButtonUp(0))
        {
            aimingMod = false;
            projectileRB.isKinematic = false;
            projectileRB.velocity = -mouseDelta * velocityMult;

            FollowCam.POI = projectile;
            projectile = null;
        }
    }
}
