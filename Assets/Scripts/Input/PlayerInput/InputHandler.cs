using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public Vector3 _moveDir;
    public Vector2 _mouseDelta;
    public bool _interact;
    public bool _pause;

    private void Awake()
    {
        DontDestroyOnLoad(transform.root);
    }

    public void MouseDeltaInput(InputAction.CallbackContext context)
    {
        _mouseDelta = context.ReadValue<Vector2>();
    }

    public void MoveInput(InputAction.CallbackContext context)
    {
        _moveDir.x = context.ReadValue<Vector2>().x;
        _moveDir.z = context.ReadValue<Vector2>().y;
        _moveDir.Normalize();
    }

    public void InteractInput(InputAction.CallbackContext context)
    {
        if (context.started)
            _interact = true;
        else if (context.canceled)
            _interact = false;
    }

    public void PauseInput(InputAction.CallbackContext context)
    {
        _pause = context.started;
    }

}
