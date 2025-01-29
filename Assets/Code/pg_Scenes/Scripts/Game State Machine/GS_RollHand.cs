using UnityEngine;

public class GS_RollHand : GS_BaseState
{
    public GS_RollHand(GameLoopStateMachine _stateMachineRef) : base(_stateMachineRef)
    {

    }

    public override void Enter()
    {
        base.Enter();
        //_stateMachine._RollEvent += HandRolled;
        ChessEventSystem.StartListening("HandRolled", HandRolled);

        Debug.Log("Roll Hand Enter");
    }

    public override void Exit()
    {
        base.Exit();
        //_stateMachine._RollEvent -= HandRolled;
        ChessEventSystem.StopListening("HandRolled", HandRolled);

        Debug.Log("Roll Hand Exit");
    }

    public void HandRolled()
    {
        _stateMachine.ChangeState(new GS_PlacePiece(_stateMachine));
    }
}
