using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionNode : Node
{
    public enum TypeAction
    {
        Herd,
        Arrive,
        Evade
    }

    public TypeAction type;

    public override void Execute(Boid boid)
    {
        if (type == TypeAction.Herd)
        {
            boid.Flocking();
        }
        else if (type == TypeAction.Evade)
        {
            boid.Evading();
        }
        else if (type == TypeAction.Arrive)
        {
            boid.Arriving();
        }
    }


}
