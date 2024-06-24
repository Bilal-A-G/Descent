using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBodySegments : MonoBehaviour
{
    [SerializeField] private List<BodySegment> bodySegments;
    [SerializeField] private Transform spider;
    [SerializeField] private float distanceUntilLerp;
    [SerializeField] private float lerpIncrement;
    [SerializeField] private float lerpSpeed;


    [SerializeField] private Transform runner;

    void Update()
    {
        for (int i = 0; i < bodySegments.Count; i++) 
        {
            BodySegment currentSegment = bodySegments[i];

            RaycastHit hit;
            if (Physics.Raycast(currentSegment.segment.position, currentSegment.segment.forward, out hit))
            {
                if ((hit.point - currentSegment.segment.position).magnitude > distanceUntilLerp)
                {
                    currentSegment.localXRotation -= lerpSpeed * Time.deltaTime;
                    continue;
                }

                currentSegment.localXRotation += lerpIncrement * Time.deltaTime;
            }
            else
            {
                currentSegment.localXRotation -= lerpSpeed * Time.deltaTime;
            }

            if(currentSegment.localXRotation < 0)
                currentSegment.localXRotation = 0;

            bodySegments[i] = currentSegment;
        }

        foreach (BodySegment currentSegment in bodySegments)
        {
            currentSegment.segment.transform.localEulerAngles = new Vector3(-currentSegment.localXRotation, 0, 0);
        }
    }
}

[System.Serializable]
public struct BodySegment
{
    public Transform segment;
    [HideInInspector] public float localXRotation;
}
