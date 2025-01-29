using UnityEngine;

public class GS_ResolveBoard : GS_BaseState
{
    public GS_ResolveBoard(GameLoopStateMachine _stateMachineRef) : base(_stateMachineRef)
    {

    }

    public override void Enter()
    {
        base.Enter();
        //_stateMachine._RollEvent += PiecePlaced;
        ChessEventSystem.StartListening("BoardResolved", BoardResolved);


        Debug.Log("Board Resolve Enter");
    }

    public override void Exit()
    {
        base.Exit();
        //_stateMachine._RollEvent -= PiecePlaced;
        ChessEventSystem.StopListening("BoardResolved", BoardResolved);

        Debug.Log("Board Resolve Exit");
    }

    public void BoardResolved()
    {
        _stateMachine.ChangeState(new GS_NextRound(_stateMachine));

    }
}
