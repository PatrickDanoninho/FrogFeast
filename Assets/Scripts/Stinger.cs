using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stinger : MonoBehaviour, ITongueInteractable
{
    StageManager stageManager;

    public enum States
    {
        Inactive,
        Entering,
        Dashing,
        Leaving,
        killed
    }
    public States currentState;
    public float stateTimer;

    // Start is called before the first frame update
    void Start()
    {
        stageManager = StageManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case States.Inactive:
                break;
            case States.Entering:
                UpdateEntering();
                break;
            case States.Dashing:
                UpdateDashing();
                break;
            case States.Leaving:
                UpdateLeaving();
                break;
            case States.killed:
                UpdateKilled();
                break;
            default:
                break;
        }
    }
    public void OnTongueTipHit()
    {
        Explode();
    }

    public void Spawn()
    {
        if (stageManager == null)
            stageManager = StageManager.Instance;
        stateTimer = 0f;
        ChangeState(States.Entering);
    }

    void ChangeState(States newState)
    {
        stateTimer = 0f;
        currentState = newState;
    }

    void UpdateEntering()
    {

    }

    void UpdateDashing() 
    {

    }
    void UpdateLeaving() 
    {

    }

    void UpdateKilled() 
    {

    }

    public void Explode()
    {

    }
}
