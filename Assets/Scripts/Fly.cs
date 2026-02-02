using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour, ITongueInteractable
{
    StageManager stageManager;

    public enum State 
    {
        Inactive,
        Entering,
        Idle,
        Leaving,
        Eaten
    }

    public State currentState {  get; private set; }
    float stateTimer;

    private void Awake()
    {
        stageManager = StageManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.Inactive:
                break;
            case State.Entering:
                UpdateEntering();
                break;
            case State.Idle:
                UpdateIdle();
                break;
            case State.Leaving:
                UpdateLeaving();
                break;
            case State.Eaten:
                UpdateEaten();
                break;
            default:
                break;
        }
    }

    public void OnTongueTipHit() 
    {
        Eat();
    }

    public void Spawn() 
    {
        if(stageManager == null)
            stageManager = StageManager.Instance;
        stateTimer = 0f;
        ChangeState(State.Entering);
    }

    void ChangeState(State newState) 
    {
        stateTimer = 0f;
        currentState = newState;
    }

    void UpdateEntering() 
    {
        stateTimer += Time.deltaTime;

        if (stateTimer > 0.3f)
            ChangeState(State.Idle);
    }

    void UpdateIdle() 
    {
        stateTimer += Time.deltaTime;

        if(stateTimer > 2f)
            ChangeState(State.Leaving);
    }

    void UpdateLeaving() 
    {
        stateTimer += Time.deltaTime;

        if (stateTimer > 0.3f)
            Despawn();
    }

    void UpdateEaten() 
    {
        stageManager.UpdateGameScore();
        ChangeState(State.Inactive);
        Despawn();
    }

    public void Eat() 
    {
        if (currentState == State.Eaten)
            return;

        ChangeState(State.Eaten);
    }

    void Despawn() 
    {
        ChangeState(State.Inactive);
        stageManager.ResetInteractable(stageManager.flyList, gameObject);
    }
}
