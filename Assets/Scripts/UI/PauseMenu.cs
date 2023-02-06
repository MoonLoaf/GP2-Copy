using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour {
    [SerializeField] private GameObject _pauseUI;
    private bool _isPaused = false;
    [SerializeField] private PlayerInput _playerInput;
    
    
    public void PauseUnPause(InputAction.CallbackContext context) {
        if (context.started) {
            _isPaused = !_isPaused;
            _pauseUI.SetActive(_isPaused);
            Time.timeScale = _isPaused ? 0 : 1;
        }
    }

    public void MouseToggle(InputAction.CallbackContext context) {
        if (context.started) {
            if (_playerInput.currentControlScheme == "KeyboardMouse") {
                Cursor.lockState = _isPaused ? CursorLockMode.None : CursorLockMode.Locked;
                Cursor.visible = _isPaused;
            }
        }
    }
    public void OnResume() {
        _isPaused = !_isPaused;
        _pauseUI.SetActive(_isPaused);
        Time.timeScale = _isPaused ? 0 : 1;
        Cursor.lockState = _isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = _isPaused;
    }
    public void OnQuit() {
        Application.Quit();
    }
}
