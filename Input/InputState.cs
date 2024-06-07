public class InputState
{
    public bool Held, Pressed, Released;
    public static float threshold = 0.5f;
    protected bool stateChange;
    public virtual void UpdateInput(float currentInputValue)
    {
        stateChange = Held;
        Held = currentInputValue >= threshold;
        Pressed = Held && !stateChange;
        Released = !Held && stateChange;
    }
}
