using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch : MonoBehaviour
{
    public MonsterVariables variables;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name != variables.playerName)
            return;

        variables.playerLastKnownPos = variables.player.position;
        variables.alertness += 1.0f;
    }
}
