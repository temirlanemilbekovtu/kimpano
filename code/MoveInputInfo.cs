namespace Godot;

public class MoveInputInfo
{
    public MoveKey      MoveKey     { get; private set; }
    public bool         IsPressed   { get; private set; }
    public float        TimeMsec    { get; private set; }

    public MoveInputInfo(MoveKey moveMoveKey, bool isPressed, float timeMsec) {
        MoveKey = moveMoveKey;
        IsPressed = isPressed;
        TimeMsec = timeMsec;
    }
}