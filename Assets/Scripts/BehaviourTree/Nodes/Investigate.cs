using System.Collections.Generic;
using UnityEngine;

public class Investigate : NodeBase
{
    public MonsterVariables variables;
    public MonsterMovementController controller;
    public float investigateRadius;

    public Color debugColour;
    public float debugRadius;
    private Vector3 m_currentInvestigatePos;

    public override void OnTransition()
    {
        variables.lookAtPos = variables.playerLastKnownPos;
    }

    public override void Tick()
    {
        float randomX = Random.Range(0.0f, 1.0f);
        float randomY = Random.Range(0.0f, 1.0f);
        float randomZ = Random.Range(0.0f, 1.0f);

        Vector3 randomOffset = new Vector3(randomX, randomY, randomZ).normalized;
        float randomMagnitude = Random.Range(0.0f, investigateRadius * variables.alertness);

        RaycastHit hit;
        Vector3 moveToPos = variables.playerLastKnownPos + randomOffset * randomMagnitude;

        if (Physics.Raycast(variables.playerLastKnownPos, randomOffset, out hit))
        {
            if((hit.point - variables.playerLastKnownPos).magnitude < investigateRadius)
                moveToPos = hit.point;
        }

        variables.lookAtPos = moveToPos;
        controller.Move(moveToPos);
        m_currentInvestigatePos = moveToPos;
        //Play looking around/inspecting animations
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = debugColour;
        Gizmos.DrawSphere(m_currentInvestigatePos, debugRadius);
        Gizmos.DrawWireSphere(variables.playerLastKnownPos, investigateRadius * variables.alertness);
    }
}
