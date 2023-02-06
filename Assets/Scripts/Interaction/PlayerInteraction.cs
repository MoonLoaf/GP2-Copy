using UnityEditor.Timeline;
using UnityEngine;

public class PlayerInteraction : PlayerComponent
{
    [SerializeField] private PlayerType _interActionType;
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private Player _player;
    [SerializeField] public bool Interact;

    public PlayerInteraction(Player player, InputHandler inputHandler)
    {
        _inputHandler = inputHandler;
        _player = player;
    }

    public override void LogicUpdate()
    {
       Interact = _inputHandler._interact;
    }
}
