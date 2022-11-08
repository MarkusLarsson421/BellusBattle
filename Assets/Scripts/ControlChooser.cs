using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlChooser : MonoBehaviour
{
    private List<PlayerInput> playerInputs = new List<PlayerInput>();
    private static bool toggledLeft;
    private static bool toggledRight;

    
    

    public void LeftControllerMode()
    {
        toggledLeft = !toggledLeft;
        toggledRight = false;
        ControlScheme();


    }
    public void RightControllerMode()
    {
        toggledRight = !toggledRight;
        //toggledLeft = false;
        ControlScheme();
    }

    public void AddToList(PlayerInput input)
    {
        playerInputs.Add(input);
    }


    void ControlScheme()
    {
        if (toggledLeft)
        {
            foreach (PlayerInput input in playerInputs)
            {
                input.SwitchCurrentActionMap("PlayerAccessibility");
            }
            Debug.Log("sätt in left controller control scheme här");
        }
        if (toggledRight)
        {
            Debug.Log("sätt in right controller control scheme här");
            
        }
        if(!toggledRight && !toggledLeft)
        {
            Debug.Log("Sätt in normal kotroller här");
        }
    }
}
