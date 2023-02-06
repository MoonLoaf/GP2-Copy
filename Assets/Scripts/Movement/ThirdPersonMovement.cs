using System.Runtime.CompilerServices;
using UnityEngine;

public class ThirdPersonMovement : PlayerComponent
{
    private Camera _camera;
    private InputHandler _inputHandler;
    private GhostData _ghostData;
    private Rigidbody _rb;
    private Player _player;
    public bool _canMove;


    public ThirdPersonMovement(Player player, Camera camera, InputHandler inputHandler, GhostData ghostData, Rigidbody rb) {
        _player = player;
        _camera = camera;
        _inputHandler = inputHandler;
        _ghostData = ghostData;
        _rb = rb;
        _canMove = true;
    }

    public override void PhysicsUpdate() {

        if (!_canMove)
            return;

        if (GameManager.Instance.CurrentGameState == GameState.NormalState) {
            HandleMovement();
            HandleRotation();
        }
        else if (_rb.velocity != Vector3.zero) {
            StopMovement();
        }
    }

    public void StopMovement() {
        _rb.velocity = Vector3.zero;
        //_rb.isKinematic = false;
    }

    private void HandleMovement() {
        Vector2 inputVector = new Vector2(_inputHandler._moveDir.z, _inputHandler._moveDir.x);

        Vector3 moveDir = _camera.transform.forward * inputVector.x;
        moveDir += _camera.transform.right * inputVector.y;
        moveDir.y = 0;
        moveDir.Normalize();
        moveDir *= _ghostData._moveSpeed;
        moveDir.y = _rb.velocity.y;

        if (inputVector.magnitude == 0) {
            _rb.velocity /= _ghostData._deaccel;
        }
        else {
            _rb.velocity = moveDir;
            //   AudioManager.Instance.Play("FootstepTest", transform.position);
        }
    }

    private void HandleRotation() {
        Vector3 targetDir;

        targetDir = _camera.transform.forward * _inputHandler._moveDir.z;
        targetDir += _camera.transform.right * _inputHandler._moveDir.x;
        targetDir.y = 0;

        if (targetDir == Vector3.zero) {
            targetDir = _rb.transform.forward;
        }

        Quaternion tr = Quaternion.LookRotation(targetDir);
        _rb.transform.rotation =
            Quaternion.Slerp(_rb.transform.rotation, tr, _ghostData._rotationSpeed * Time.deltaTime);
    }
}