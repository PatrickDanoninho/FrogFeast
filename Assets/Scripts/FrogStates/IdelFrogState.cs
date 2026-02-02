using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdelFrogState : FrogBaseState
{
    public override void EnterState(FrogBehaviour frog)
    {
        Debug.Log("Enter Idle");
        frog.RetractTongue();
    }

    public override void UpdateState(FrogBehaviour frog)
    {
        frog.GetValidTap();
    }
}
