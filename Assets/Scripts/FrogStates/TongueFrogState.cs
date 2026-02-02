using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TongueFrogState : FrogBaseState
{
    public override void EnterState(FrogBehaviour frog)
    {
        Debug.Log("Enter Tongue");
        frog.ExtendTongue();
    }

    public override void UpdateState(FrogBehaviour frog)
    {
        //frog.UpdateTongueVisuals();
    }
}
