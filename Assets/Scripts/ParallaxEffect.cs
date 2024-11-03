using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Transform cameraTransform;
    private Vector3 lastCameraPosition;
    public float parallaxFactor;

    void Start()
    {
        lastCameraPosition = cameraTransform.position;
    }

    void Update()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += deltaMovement * parallaxFactor;
        lastCameraPosition = cameraTransform.position;
    }
}
