using UnityEngine;


public class InputState
{
    //public InputState(string inputName) => this.inputName = inputName;

    public bool Held, Pressed, Released;
    public static float threshold = 0.5f;
    protected bool stateChange;
    //protected string inputName;

    public virtual void UpdateInput(float currentInputValue)
    {
        stateChange = Held;
        Held = currentInputValue >= threshold;
        Pressed = Held && !stateChange;
        Released = !Held && stateChange;
    }

    /*public virtual void TestInput()
    {
        if (Pressed)
        {
            Debug.Log(inputName + " Was Pressed");
        }
        if (Held)
        {
            Debug.Log(inputName + " Was Held");
        }
        if (Released)
        {
            Debug.Log(inputName + " Was Released");
        }
    }*/
}
