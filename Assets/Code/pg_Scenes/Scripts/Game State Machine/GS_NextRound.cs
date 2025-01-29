using UnityEngine;

public class GS_NextRound : GS_BaseState
{
    public GS_NextRound(GameLoopStateMachine _stateMachineRef) : base(_stateMachineRef)
    {

    }

    public override void Enter()
    {
        base.Enter();
        //_stateMachine._RollEvent += PiecePlaced;
        ChessEventSystem.StartListening("NextRound", NextRound);


        Debug.Log("Next Round Enter");
    }

    public override void Exit()
    {
        base.Exit();
        //_stateMachine._RollEvent -= PiecePlaced;
        ChessEventSystem.StopListening("NextRound", NextRound);

        Debug.Log("Next Round Exit");
    }

    public void NextRound()
    {
        _stateMachine.IncrementRound();
        _stateMachine.ChangeState(new GS_RollHand(_stateMachine));

    }
}
