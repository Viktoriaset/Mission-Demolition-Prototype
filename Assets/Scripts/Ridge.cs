using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ridge : MonoBehaviour
{
    [Header("Set in inspector")]
    [SerializeField] private GameObject mountain;
    [SerializeField] private int numMountainMin = 6;
    [SerializeField] private int numMountainMax = 10;
    [SerializeField] private Vector3 mountainOffsetScale = Vector3.one;
    [SerializeField] private Vector2 mountainScaleRangeX = new Vector2(2, 5);
    [SerializeField] private Vector2 mountainScaleRangeY = new Vector2(3, 8);

    private List<GameObject> mountainList;

    private void Start()
    {
        mountainList = new List<GameObject>();

        int num = Random.Range(numMountainMin, numMountainMax);
        for (int i = 0; i < num; i++)
        {
            GameObject mo = Instantiate(mountain);
            mountainList.Add(mo);
            Transform moTrans = mo.transform;
            moTrans.SetParent(transform);

            Vector3 scale = Vector3.one;
            scale.x = Random.Range(mountainScaleRangeX.x, mountainScaleRangeX.y);
            scale.y = Random.Range(mountainScaleRangeY.x, mountainScaleRangeY.y);
            moTrans.localScale = scale;

            Vector3 offset = Random.insideUnitSphere;
            offset.x *= mountainOffsetScale.x;
            offset.y += scale.y / 2;
            offset.z = scale.y;
            moTrans.localPosition = offset;
        }
    }

}
