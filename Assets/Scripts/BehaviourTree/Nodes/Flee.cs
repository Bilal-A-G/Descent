using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flee : NodeBase
{
    public List<Transform> fleePoints;
    public MonsterMovementController controller;
    public MonsterVariables variables;
    public float minPlayerDistanceFromWaypoint;
    public float thresholdBeforeKill;

    public EventObject deManifest;
    public EventObject manifest;

    public override void OnTransition()
    {
        int bestWaypointIndex = -1;
        float furthestDistance = 0.0f;

        for (int i = 0; i < fleePoints.Count; i++)
        {
            Vector3 toWaypoint = variables.player.position - fleePoints[i].position;

            if (toWaypoint.magnitude > furthestDistance)
            {
                furthestDistance = toWaypoint.magnitude;
                bestWaypointIndex = i;
            }
        }

        controller.Run();
        controller.Move(fleePoints[bestWaypointIndex].position);
    }

    public override void Tick()
    {
        if ((variables.player.position - variables.spider.position).magnitude <= thresholdBeforeKill)
        {
            deManifest.Invoke(false);
            manifest.Invoke(true);
            return;
        }
    }
}
