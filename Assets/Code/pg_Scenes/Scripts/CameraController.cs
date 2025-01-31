using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    [SerializeField] MouseController ms_controller;
    private CharacterController _character;


    private GameObject _playerRef;
    private bool _playerFound = false;


    private bool input_click;
    public float speed = 0.1F;

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

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            transform.Translate(-touchDeltaPosition.x * speed, -touchDeltaPosition.y * speed, 0);
        }


        if (!_playerFound)
        {
            _playerRef = GameObject.FindGameObjectWithTag("Player")?.gameObject;
            if (_playerRef)
            {
                _playerFound = true;
                MoveToPlayer();
            }
        }
        else
        {
            //Player Exists

        }
    }

    private void OnEnable()
    {
        ChessEventSystem.StartListening("Input_Click", ClickInput);
        ChessEventSystem.StartListening("Input_Point", PointInput);
        ChessEventSystem.StartListening("MovementComplete", MoveToPlayer);
    }

    private void OnDisable()
    {
        ChessEventSystem.StopListening("Input_Click", ClickInput);
        ChessEventSystem.StopListening("Input_Point", PointInput);
        ChessEventSystem.StopListening("MovementComplete", MoveToPlayer);

    }
    private void ClickInput()
    {
        input_click = true;
    }

    private void PointInput(object data)
    {
        input_click = true;
        input_point = (Vector2)data;
        //Debug.Log(input_point);
    }

    //Move Camera based on point down and point vector offsets

    public void MoveToPlayer()
    {
        Vector3 moveTo = new Vector3(_playerRef.transform.position.x, _playerRef.transform.position.y, -10f);
        transform.DOMove(moveTo, 1f).SetEase(Ease.InOutExpo);
    }
}
