using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manifest : NodeBase
{
    public MonsterMovementController controller;
    public GameObject monsterBody;

    public override void OnTransition()
    {
        monsterBody.SetActive (true);
        controller.Manifest();
        controller.Walk();
    }
}
