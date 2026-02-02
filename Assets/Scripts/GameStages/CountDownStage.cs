using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDownStage : BaseState
{
    public override void EnterStage(StageManager stageManager)
    {
        Debug.Log("Entered CountDownStage");
    }

    public override void UpdateStage(StageManager stageManager)
    {
        stageManager.CountDown();
    }
}
