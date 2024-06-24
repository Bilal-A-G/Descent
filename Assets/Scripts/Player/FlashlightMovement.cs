using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightMovement : MonoBehaviour
{
    [SerializeField] private Transform flashlightHolder;
    [SerializeField] private Transform flashlight;
    [SerializeField] private float lerpMovementSpeed;
    [SerializeField] private float lerpRotationSpeed;

    void Update()
    {
        flashlight.position = Vector3.Lerp(flashlight.position, flashlightHolder.position, Time.deltaTime * lerpMovementSpeed);
        flashlight.rotation = Quaternion.Lerp(flashlight.rotation, flashlightHolder.rotation, Time.deltaTime * lerpRotationSpeed);
    }
}
