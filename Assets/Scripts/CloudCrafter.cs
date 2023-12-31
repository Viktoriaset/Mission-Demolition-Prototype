using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCrafter : MonoBehaviour
{
    [Header("Set in inspector")]
    [SerializeField] private int numClouds = 40;
    [SerializeField] private GameObject cloudPrefab;
    [SerializeField] private Vector3 cloudPosMin = new Vector3(-50, -5, 10);
    [SerializeField] private Vector3 cloudPosMax = new Vector3(150, 100, 10);
    [SerializeField] private float cloudScaleMin = 1;
    [SerializeField] private float cloudScaleMax = 3;
    [SerializeField] private float cloudSpeedMult = 0.5f;

    private GameObject[] cloudInstances;

    private void Awake()
    {
        cloudInstances = new GameObject[numClouds];
        GameObject anchor = GameObject.Find("CloudAnchor");

        GameObject cloud;
        for (int i = 0; i < numClouds; i++)
        {
            cloud = Instantiate(cloudPrefab);

            Vector3 cPos = Vector3.zero;
            cPos.x = Random.Range(cloudPosMin.x, cloudPosMax.x);
            cPos.y = Random.Range(cloudPosMin.y, cloudPosMax.y);

            float scaleU = Random.value;
            float scaleVal = Mathf.Lerp(cloudScaleMin, cloudScaleMax, scaleU);

            // Меньшие облака должны быть ближе к земле
            cPos.y = Mathf.Lerp(cloudPosMin.y, cPos.y, scaleU);

            //Меньшие облака должныть быть дальше
            cPos.z = 100 - 90 * scaleU;

            cloud.transform.position = cPos;
            cloud.transform.localScale = Vector3.one * scaleVal;

            cloud.transform.SetParent(anchor.transform);

            cloudInstances[i] = cloud;
        }
    }

    private void Update()
    {
        foreach(GameObject cloud in cloudInstances)
        {
            float scaleVal = cloud.transform.localScale.x;
            Vector3 cPos = cloud.transform.position;

            cPos.x -= scaleVal * Time.deltaTime * cloudSpeedMult;

            if (cPos.x <= cloudPosMin.x)
            {
                cPos.x = cloudPosMax.x;
            }

            cloud.transform.position = cPos;
        }
    }

}
