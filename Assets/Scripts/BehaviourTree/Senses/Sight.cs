using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : MonoBehaviour
{
    public EventObject onSpotted;
    public EventObject onPartiallySpotted;
    public EventObject onEnteredKillRange;
    public float killRange;
    public Transform eyes;
    public Transform headLook;
    public string playerName;
    public MonsterVariables variables;

    public float headRotateSpeed;
    public float headRotateOffset;

    public List<Vision> visions;

    private Transform m_player;

    void Start()
    {
        m_player = GameObject.Find(playerName).transform;
    }

    void Update()
    {
        if (variables.alertness > 0.3f)
        {
            headLook.transform.position =
                Vector3.Lerp(headLook.transform.position, variables.lookAtPos, 
                Time.deltaTime * headRotateSpeed * variables.alertness);
        }
        else
        {
            headLook.transform.position =
                Vector3.Lerp(headLook.transform.position, eyes.position + variables.spider.forward * headRotateOffset, 
                Time.deltaTime * headRotateSpeed);
        }

        foreach (Vision vision in visions) 
        {
            Vector3 toPlayer = m_player.position - eyes.position;

            Vector3 toPlayerXZ = Vector3.ProjectOnPlane(toPlayer, eyes.up);
            Vector3 toPlayerYZ = Vector3.ProjectOnPlane(toPlayer, eyes.right);
            Vector3 forward = (headLook.position - eyes.position).normalized;

            if (toPlayerXZ.magnitude > vision.distance)
                continue;

            float dotProductXZ = Vector3.Dot(toPlayerXZ.normalized, Vector3.ProjectOnPlane(forward, eyes.up));
            float dotProductYZ = Vector3.Dot(toPlayerYZ.normalized, Vector3.ProjectOnPlane(forward, eyes.right));

            if (dotProductXZ < vision.coneFOVXZ || dotProductYZ < vision.coneFOVYZ)
                continue;

            RaycastHit hit;
            if (!Physics.Raycast(eyes.position, toPlayer.normalized, out hit))
                continue;

            if (hit.collider.name != playerName)
                continue;

            variables.alertness += vision.strength * Time.deltaTime;
            variables.playerLastKnownPos = hit.point;
        }

        if ((variables.spider.position - variables.player.position).magnitude <= killRange)
        {
            onEnteredKillRange.Invoke(true);
        }
        else
        {
            onEnteredKillRange.Invoke(false);
        }

        if(variables.alertness >= 0.96f)
        {
            onSpotted.Invoke(true);
            onPartiallySpotted.Invoke(true);
        }
        else if(variables.alertness < 0.96f && variables.alertness > 0.4f)
        {
            onPartiallySpotted.Invoke(true);
            onSpotted.Invoke(false);
        }
        else
        {
            onSpotted.Invoke(false);
            onPartiallySpotted.Invoke(false);
        }
    }

    [Header("Debug")]
    public bool visualize;
    public int numLines;
    public float step;

    private void OnDrawGizmos()
    {
        if(!visualize) 
            return;

        int index = 0;

        foreach(Vision vision in visions)
        {
            for (int i = -numLines; i < numLines; i++)
            {
                Vector3 original = eyes.forward;
                Vector3 rotatedXZ = (Quaternion.AngleAxis(step * i, eyes.up) * original).normalized;

                if (Vector3.Dot(original, rotatedXZ) >= vision.coneFOVXZ)
                {
                    Debug.DrawLine(eyes.position, eyes.position + rotatedXZ * vision.distance, vision.debugColour);
                }

                Vector3 rotatedYZ = (Quaternion.AngleAxis(step * i, eyes.right) * original).normalized;

                if (Vector3.Dot(original, rotatedYZ) >= vision.coneFOVYZ)
                {
                    Debug.DrawLine(eyes.position, eyes.position + rotatedYZ * vision.distance, vision.debugColour);
                }
            }

            index++;
        }
    }
}

[System.Serializable]
public struct Vision
{
    [Range(-1.0f, 1.0f)] public float coneFOVXZ;
    [Range(-1.0f, 1.0f)] public float coneFOVYZ;
    public float distance;
    public float strength;

    [Header("Debug")]
    public Color debugColour;
    public string name;
}
