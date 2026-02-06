using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStage : BaseState
{
    public override void EnterStage(StageManager stageManager)
    {
        Debug.Log("Entered GameStage");
        stageManager.gameManager.ShowUI(GameManager.UIState.InGame);

        MusicManager.Instance.PlayMusic();

        MusicManager.OnBeat += stageManager.SpawnFly;
        MusicManager.OnMarker += stageManager.WaitForMarker;
    }

    public override void UpdateStage(StageManager stageManager)
    {
    }
}
