using UnityEngine;

public class InputAxis : InputState
{
    public Vector2 axisPosition;

    public void UpdateInput(float currentInputValue, Vector2 axisPosition)
    {
        UpdateInput(currentInputValue);
        this.axisPosition = axisPosition;
    }
}
