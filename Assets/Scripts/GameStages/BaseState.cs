using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    public abstract void EnterStage(StageManager stageManager);
    public abstract void UpdateStage(StageManager stageManager);
}
