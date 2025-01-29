using UnityEngine;

public class GS_ResolveMove : GS_BaseState
{
    public GS_ResolveMove(GameLoopStateMachine _stateMachineRef) : base(_stateMachineRef)
    {

    }

    public override void Enter()
    {
        base.Enter();
        //_stateMachine._RollEvent += PiecePlaced;
        ChessEventSystem.StartListening("MoveResolved", MoveResolved);


        Debug.Log("Move Resolve Enter");
    }

    public override void Exit()
    {
        base.Exit();
        //_stateMachine._RollEvent -= PiecePlaced;
        ChessEventSystem.StopListening("MoveResolved", MoveResolved);

        Debug.Log("Move Resolve Exit");
    }

    public void MoveResolved()
    {
        _stateMachine.ChangeState(new GS_ResolveBoard(_stateMachine));

    }
}
