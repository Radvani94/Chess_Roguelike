using UnityEngine;

public class GS_BaseState
{
    protected GameLoopStateMachine _stateMachine;
    protected GameLoopStateMachine StateMachine { get { return _stateMachine; } }

    protected float startTime;
    protected bool isExitingState = false;

    public GS_BaseState(GameLoopStateMachine _stateMachineRef)
    {

        _stateMachine = _stateMachineRef;
        
    }

    public virtual void Enter()
    {
        startTime = Time.time;
    }
    public virtual void LogicUpdate()
    {
        TransitionChecks();
    }
    public virtual void PhysicsUpdate()
    {

    }
    public virtual void TransitionChecks()
    {

    }
    public virtual void Exit()
    {
        isExitingState = true;
    }
}
