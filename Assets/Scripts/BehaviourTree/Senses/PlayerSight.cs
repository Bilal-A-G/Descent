using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSight : MonoBehaviour
{
    public MonsterVariables variables;
    public float playerVisionRange;
    public EventObject playerCantSeeEvent;
    public SkinnedMeshRenderer spiderRenderer;

    void Update()
    {
        playerCantSeeEvent.Invoke(!(spiderRenderer.isVisible && 
            (variables.player.position - variables.spider.position).magnitude < playerVisionRange));
    }
}
