using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : NodeBase
{
    public MonsterMovementController controller;
    public MonsterVariables variables;

    public override void OnTransition()
    {
        controller.Run();
    }

    public override void Tick()
    {
        controller.Move(variables.playerLastKnownPos);
        variables.lookAtPos = variables.playerLastKnownPos;

        //Play scary chase sounds here
    }

    public override void OnExit()
    {
        controller.Walk();
    }
}
