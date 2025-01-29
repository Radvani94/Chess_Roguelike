using UnityEngine;

public class GS_PlacePiece : GS_BaseState
{
    public GS_PlacePiece(GameLoopStateMachine _stateMachineRef) : base(_stateMachineRef)
    {

    }

    public override void Enter()
    {
        base.Enter();
        //_stateMachine._RollEvent += PiecePlaced;
        ChessEventSystem.StartListening("PiceCollected", PiecePlaced);


        Debug.Log("Place Piece Enter");
    }

    public override void Exit()
    {
        base.Exit();
        //_stateMachine._RollEvent -= PiecePlaced;
        ChessEventSystem.StopListening("PiceCollected", PiecePlaced);

        Debug.Log("Place Piece Exit");
    }

    public void PiecePlaced()
    {
        _stateMachine.ChangeState(new GS_ResolveMove(_stateMachine));

    }
}
