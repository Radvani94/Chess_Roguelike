using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;


public class GameLoopStateMachine : MonoBehaviour
{

    private GS_BaseState _currentState;
    public GS_BaseState CurrentState { get { return _currentState; } }

    public UnityAction _RollEvent;
    public UnityAction _PlaceEvent;

    public Button _RollButton;
    public Button _PlaceButton;
    public Button _ResolveMove;
    public Button _ResolveBoard;
    public Button _NextRound;

    public TMP_Text _RoundText;

    private int _round = 0;
    public int Round { get { return _round; } }

    

    public void ChangeState(GS_BaseState newState)
    {
        _currentState.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    public void InitializeStateMachine(GS_BaseState initialState)
    {
        _round = 1;
        
        _currentState = initialState;
        _currentState.Enter();
    }

    public void IncrementRound()
    {
        _round++;
    }

    public void UpdateUI()
    {
        _RoundText.text = _round.ToString();
    }

    private void Start()
    {
        _RollButton.GetComponent<Button>().onClick.AddListener(CheckHandRolled);
        _PlaceButton.GetComponent<Button>().onClick.AddListener(CheckPiecePlaced);
        _ResolveMove.GetComponent<Button>().onClick.AddListener(CheckResolveMove);
        _ResolveBoard.GetComponent<Button>().onClick.AddListener(CheckResolveBoard);
        _NextRound.GetComponent<Button>().onClick.AddListener(CheckNextRound);


        Debug.Log("Initializing New State Machine -  Roll Hand");
        InitializeStateMachine(new GS_RollHand(this));
    }
    private void Update()
    {
        UpdateUI();
    }

    public void CheckHandRolled()
    {
        ChessEventSystem.TriggerEvent("HandRolled");
    }

    public void CheckPiecePlaced()
    {
        
        ChessEventSystem.TriggerEvent("PiceCollected");
    }

    public void CheckResolveMove()
    {

        ChessEventSystem.TriggerEvent("MoveResolved");
    }

    public void CheckResolveBoard()
    {

        ChessEventSystem.TriggerEvent("BoardResolved");
    }

    public void CheckNextRound()
    {

        ChessEventSystem.TriggerEvent("NextRound");
    }
}
