using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputsManager
{
    public static bool PhoneInputs = false;

    public static Vector3 GetPosition()
    {
        if (PhoneInputs && Input.touchCount != 0) return Input.GetTouch(0).position;
        else if(!PhoneInputs) return Input.mousePosition;

        Debug.Log("GetPosition impossible");
        return Vector3.zero;
    }

    public static bool Click()
    {
        if (PhoneInputs && Input.touchCount != 0) return Input.GetTouch(0).phase == TouchPhase.Began;
        else if(!PhoneInputs) return Input.GetMouseButtonDown(0);

        return false;
    }

    public static bool IsDown()
    {
        if (PhoneInputs) return Input.touchCount >= 1;
        else return Input.GetMouseButton(0);
    }

    public static bool Release()
    {
        if (PhoneInputs) return Input.GetTouch(0).phase == TouchPhase.Ended;
        else return Input.GetMouseButtonUp(0);
    }
}
