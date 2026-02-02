using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FrogBaseState
{
    public abstract void EnterState(FrogBehaviour frog);

    public abstract void UpdateState(FrogBehaviour frog);
}
