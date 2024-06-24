using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hearing : MonoBehaviour
{
    public EventObject madeQuietSound;
    public EventObject madeMediumSound;
    public EventObject madeLoudSound;

    public EventObject playerSpotted;

    public float quietNoiseAlertness;
    public float mediumNoiseAlertness;
    public float loudNoiseAlertness;

    public float noiseFalloff;

    public MonsterVariables variables;

    private bool m_playerSpotted;

    private void Start()
    {
        playerSpotted.Subscribe((bool spotted) => {m_playerSpotted = spotted;});

        madeQuietSound.Subscribe((bool _) =>
        {
            //Temporary, change it so the monster can hear sounds that do not originate from the player
            AlertMonster(quietNoiseAlertness, variables.player.position);
        });

        madeMediumSound.Subscribe((bool _) => 
        {
            AlertMonster(mediumNoiseAlertness, variables.player.position);
        });

        madeLoudSound.Subscribe((bool _) => 
        {
            AlertMonster(loudNoiseAlertness, variables.player.position);
        });
    }

    private void AlertMonster(float amount, Vector3 soundPosition)
    {
        if (m_playerSpotted)
            return;

        float distanceToMonster = (soundPosition - variables.spider.position).magnitude;
        float attentuatedAlertness = amount - distanceToMonster * noiseFalloff;
        if (attentuatedAlertness < 0)
            attentuatedAlertness = 0;

        variables.alertness += attentuatedAlertness;
        variables.playerLastKnownPos = soundPosition;
    }
}
