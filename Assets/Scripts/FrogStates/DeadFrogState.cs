using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadFrogState : FrogBaseState
{
    public override void EnterState(FrogBehaviour frog)
    {
        Debug.Log("Enter Dead");
    }

    public override void UpdateState(FrogBehaviour frog)
    {

    }
}
