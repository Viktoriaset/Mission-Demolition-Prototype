using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackground : MonoBehaviour
{
    private float startPos;
    private GameObject cam;
    [SerializeField] private float parallaxEffect;

    private void Start()
    {
        cam = GameObject.Find("Main Camera");
        startPos = transform.position.x;
    }

    private void Update()
    {
        float distance = (cam.transform.position.x * parallaxEffect);
        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);
    }
}
