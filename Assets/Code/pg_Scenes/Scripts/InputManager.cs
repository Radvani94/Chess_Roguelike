using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;



public class InputManager : MonoBehaviour
{
    
    InputAction clickAction;
    InputAction pointAction;
    InputAction holdAction;



    private void Start()
    {
        
        clickAction = InputSystem.actions.FindAction("Click");
        pointAction = InputSystem.actions.FindAction("Point");
        //holdAction = InputSystem.actions.FindAction("Hold");
          
    }

    void Update()
    {
        
        Vector2 pointValue = pointAction.ReadValue<Vector2>();
        ChessEventSystem.TriggerEvent("Input_Point", pointValue);

        if (clickAction.IsPressed())
        {
            ChessEventSystem.TriggerEvent("Input_Click");
        }

        /*if(holdAction.IsInProgress())
        {
            Debug.Log("HOLDING");
        }*/

    }

}
