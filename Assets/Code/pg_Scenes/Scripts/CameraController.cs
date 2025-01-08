using UnityEngine;
using UnityEngine.Events;

public class CameraController : MonoBehaviour
{
    [SerializeField] MouseController ms_controller;
    private CharacterController _character;


    private bool input_click;


    private Vector2 input_point;
    private void Awake()
    {
        
    }

    private void Start()
    {
        input_click = false;
        input_point = Vector2.zero;
    }
    private void Update()
    {
        input_click = false;
        input_point = Vector2.zero;
    }
    private void OnEnable()
    {
        ChessEventSystem.StartListening("Input_Click", ClickInput);
        ChessEventSystem.StartListening("Input_Point", PointInput);
    }

    private void OnDisable()
    {
        ChessEventSystem.StopListening("Input_Click", ClickInput);
        ChessEventSystem.StopListening("Input_Point", PointInput);

    }
    private void ClickInput()
    {
        input_click = true;
    }

    private void PointInput(object data)
    {
        input_click = true;
        input_point = (Vector2)data;
        Debug.Log(input_point);
    }

    //Move Camera based on point down and point vector offsets
}
