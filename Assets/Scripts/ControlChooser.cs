using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlChooser : MonoBehaviour
{
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
        toggledLeft = false;
        ControlScheme();
    }
    void ControlScheme()
    {
        if (toggledLeft)
        {
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
