using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameStage : BaseState
{
    public override void EnterStage(StageManager stageManager)
    {
        stageManager.waitForString = true;
    }

    public override void UpdateStage(StageManager stageManager)
    {

    }
}
