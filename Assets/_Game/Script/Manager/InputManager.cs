using UnityEngine;
using UnityEngine.InputSystem;
public class InputManager : Singleton<InputManager>
{
    private InputSystem inputActions;
    public InputAction MoveAction { get; private set; }

    public void OnInit()
    {
        if (inputActions == null)
        {
            inputActions = new InputSystem();
            MoveAction = inputActions.Player.Move;
            inputActions.Enable();
        }
    }

    private void OnDestroy()
    {
        if (inputActions != null)
        {
            inputActions.Disable();
        }
    }
}