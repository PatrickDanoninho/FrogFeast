using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStage : BaseState
{
    public override void EnterStage(StageManager stageManager)
    {
        Debug.Log("Entered GameStage");
        stageManager.gameManager.ShowUI(GameManager.UIState.InGame);

    }

    public override void UpdateStage(StageManager stageManager)
    {
        stageManager.SpwanFly(4);
    }
}
