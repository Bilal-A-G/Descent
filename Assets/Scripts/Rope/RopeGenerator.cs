using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeGenerator : MonoBehaviour
{
    [SerializeField] private int segmentCount;
    [SerializeField] private float segmentSpacing;
    [SerializeField] private float ropeStiffness;
    [SerializeField] private float errorThreshold;
    [SerializeField] private float drag;
    [SerializeField] private float ropeLength;

    [SerializeField] private GameObject segmentPrefab;

    private List<RopeSegment> ropeSegments;

    public List<RopeSegment> Initialize(Vector3 spawnLocation)
    {
        ropeSegments = new List<RopeSegment>();

        for (int i = 0; i < segmentCount + 1; i++)
        {
            RopeSegment currentSegment = new RopeSegment();

            GameObject instantiatedSegment = Instantiate(segmentPrefab);
            instantiatedSegment.transform.position = spawnLocation;

            Rigidbody rigidbody = instantiatedSegment.AddComponent<Rigidbody>();
            rigidbody.drag = drag;
            rigidbody.angularDrag = drag;

            bool isStartSegment = i == 0;

            rigidbody.isKinematic = isStartSegment;
            rigidbody.useGravity = !isStartSegment;

            currentSegment.gameObject = instantiatedSegment;

            currentSegment.constrain = !isStartSegment;
            if (currentSegment.constrain)
                currentSegment.previousLink = ropeSegments[i - 1].gameObject;

            ropeSegments.Add(currentSegment);
        }

        return ropeSegments;
    }

    void Update()
    {
        for (int i = 0; i < ropeSegments.Count; i++)
        {
            RopeSegment currentSegment = ropeSegments[i];

            if (!currentSegment.constrain)
                continue;

            Rigidbody rigidBody = currentSegment.gameObject.GetComponent<Rigidbody>();

            Vector3 toPreviousPosition = currentSegment.previousLink.transform.position -
                currentSegment.gameObject.transform.position;
            Vector3 errorCorrection = toPreviousPosition.normalized * (toPreviousPosition.magnitude * segmentSpacing);


            if (i != ropeSegments.Count - 1)
            {
                Vector3 toNextPosition = ropeSegments[i + 1].gameObject.transform.position -
                        currentSegment.gameObject.transform.position;

                errorCorrection += 
                    toNextPosition.normalized * (toNextPosition.magnitude * segmentSpacing);
                errorCorrection /= 2;

            }
            else
            {
                if ((currentSegment.gameObject.transform.position -
                    ropeSegments[0].gameObject.transform.position).magnitude < ropeLength)
                {
                    errorCorrection = Vector3.zero;
                }
                else
                    errorCorrection /= ropeLength;
            }

            float error = errorCorrection.magnitude;

            if (error < errorThreshold)
                continue;

            rigidBody.AddForce(errorCorrection * ropeStiffness * error);

            ropeSegments[i] = currentSegment;
        }
    }
}

[System.Serializable]
public struct RopeSegment
{
    public GameObject gameObject;
    public bool constrain;
    public GameObject previousLink;
    public bool onGround;
}
