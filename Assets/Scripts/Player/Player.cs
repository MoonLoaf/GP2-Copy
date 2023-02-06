using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int _playerIndex;
    [SerializeField] private GhostData _ghostData;
    [SerializeField] private ThridPersonCameraData _cameraData;
    private Camera _camera;
    private ThirdPersonCamera _cameraScript;
    [SerializeField] private PlayerType _type;
    

    private Rigidbody _rigidbody;

    private InputHandler _inputHandler;

    private List<PlayerComponent> _playerComponents = new List<PlayerComponent>();

    private ThirdPersonMovement _movementComponent;
    public PlayerInteraction InteractionComponent;
    

    private void Awake() {
        if (_type == PlayerType.Ghost)
            _camera = GameObject.Find("DogCamera").GetComponentInChildren<Camera>(); // we should do this in inspector since its only 1 scene
        else
            _camera = GameObject.Find("HumanCamera").GetComponentInChildren<Camera>();
            
        _rigidbody = GetComponent<Rigidbody>();
        _cameraScript = _camera.GetComponent<ThirdPersonCamera>();
        _inputHandler = DeviceManager.Instance.GetInputHandler(_playerIndex);

        if (_inputHandler == null)
            return;

        _movementComponent = new ThirdPersonMovement(this, _camera, _inputHandler, _ghostData, _rigidbody);
        InteractionComponent = new PlayerInteraction(this, _inputHandler);
        AddComponent(_movementComponent);
        AddComponent(InteractionComponent);
    }

    public void StopMovement() => _movementComponent.StopMovement();

    #region Components
    private void Start()
    {
        StartComponents();
    }
    private void Update()
    {
        LogicUpdateComponents();
    }
    private void FixedUpdate()
    {
        PhysicsUpdateComponents();
    }

    private void StartComponents() {
        for (int i = 0; i < _playerComponents.Count; i++)
        {
            _playerComponents[i].StartComponent();
        }
    }
    private void LogicUpdateComponents() {
        for (int i = 0; i < _playerComponents.Count; i++)
        {
            _playerComponents[i].LogicUpdate();
        }
    }
    private void PhysicsUpdateComponents() {
        for (int i = 0; i < _playerComponents.Count; i++)
        {
            _playerComponents[i].PhysicsUpdate();
        }
    }

    private void AddComponent(PlayerComponent component) {
        _playerComponents.Add(component);
    }

    #endregion

    private void GetInput(int playerIndex)
    {
        _inputHandler = DeviceManager.Instance.GetInputHandler(_playerIndex);

        AddComponent(new ThirdPersonMovement(this, _camera, _inputHandler, _ghostData, _rigidbody));
        AddComponent(new PlayerInteraction(this, _inputHandler));
    }

    public void UpdateCameraTransform(Transform cameraTransform = null) {
        _cameraScript.UpdateCameraTransform(cameraTransform);
    }

    [ContextMenu("GetInput")]
    public void GetInputMenuItem()
    {
        GetInput(_playerIndex);
    }
}
