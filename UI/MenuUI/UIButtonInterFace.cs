
public interface UIButtonInterFace
{
    public enum ButtonState
    {
        UnVisible,
        NotSelected,
        Selected
    }

    public void SetState(ButtonState state);
    public ButtonState GetState();

    public void ActionButton();
}