using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class Wander : NodeBase
{
    public MonsterMovementController controller;
    public MonsterVariables variables;
    public float horizontalRotation;
    public float verticalRotation;
    public List<Transform> waypoints;

    public float debugSphereRadius;
    public Color debugColor;
    private Transform m_currentWaypoint;
    private int m_currentWaypointIndex = 0;

    private Vector3 m_wanderPoint;

    public override void OnExit()
    {
        controller.Move(variables.spider.position);
    }

    public override void Tick()
    {
        m_currentWaypoint = waypoints[m_currentWaypointIndex];

        RaycastHit hit;
        float rotationXZ = Random.Range(-horizontalRotation, horizontalRotation);
        float rotationYZ = Random.Range(-verticalRotation, verticalRotation);

        Vector3 raycastDirectionXZ = Quaternion.AngleAxis(rotationXZ, m_currentWaypoint.up) * m_currentWaypoint.forward;
        Vector3 finalRaycastDirection = Quaternion.AngleAxis(rotationYZ, m_currentWaypoint.right) * raycastDirectionXZ;

        if (Physics.Raycast(m_currentWaypoint.position, finalRaycastDirection.normalized, out hit))
        {
            controller.Move(hit.point);
            m_wanderPoint = hit.point;
        }

        variables.lookAtPos = variables.playerLastKnownPos;

        m_currentWaypointIndex++;
        if (m_currentWaypointIndex >= waypoints.Count)
            m_currentWaypointIndex = 0;
    }

    private void OnDrawGizmos()
    {
        if (m_wanderPoint == controller.runner.transform.position)
            return;

        Gizmos.color = debugColor;
        Gizmos.DrawSphere(m_wanderPoint, debugSphereRadius);
    }
}
