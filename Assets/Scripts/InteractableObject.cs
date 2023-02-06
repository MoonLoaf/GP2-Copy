using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class InteractableObject : MonoBehaviour
{
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private Quest _quest;
    [SerializeField] private PlayerType _interactedBy;
    [SerializeField] private CameraMode _cameraMode;

    [SerializeField, Tooltip("If cameraType is Puzzlemode")]
    private Transform _cameraTransform;

    [SerializeField] private List<UnityEvent> events;
    [SerializeField] private List<Player> _playersInside = new();

    private bool _interact;

    private void OnTriggerEnter(Collider other) {

        var cameraState = GameManager.Instance.CurrentGameState;
        var player = other.GetComponentInParent<Player>();

        switch (_interactedBy) {
            case PlayerType.Ghost:
                if (other.CompareTag("Ghost")) {
                    player.UpdateCameraTransform(_cameraTransform);
                    
                    // _quest.CompleteQuest(); den här måste kallas på när pusslet blir klarat
                    _interact = true;
                }
                break;
            case PlayerType.Human:
                if (other.CompareTag("Player")) {
                    player.UpdateCameraTransform(_cameraTransform);
                    
                    _quest.CompleteQuest();
                    _interact = true;
                }

                break;
            case PlayerType.Both:
                if (other.CompareTag("Player") || other.CompareTag("Ghost")) {
                    player.UpdateCameraTransform(_cameraTransform);
                    
                    _quest.CompleteQuest();
                    _interact = true;
                }
                break;
        }
    }

    private void OnTriggerExit(Collider other) {

        var cameraState = GameManager.Instance.CurrentGameState;
        var player = other.GetComponentInParent<Player>();

        switch (_interactedBy) {
            case PlayerType.Ghost:
                if (other.CompareTag("Ghost")) {
                    player.UpdateCameraTransform();
                    _interact = false;
                }
                break;
            case PlayerType.Human:
                if (other.CompareTag("Player")) {
                    player.UpdateCameraTransform();
                    _interact = false;
                }
                break;
            case PlayerType.Both:
                if (other.CompareTag("Player") || other.CompareTag("Ghost")) {
                    player.UpdateCameraTransform();
                    _interact = false;
                }
                break;
        }
    }

    private void OnTriggerStay(Collider other) {

        var cameraState = GameManager.Instance.CurrentGameState;
        var player = other.GetComponentInParent<Player>();

        switch (_interactedBy) {
            case PlayerType.Ghost:
                if (player.InteractionComponent.Interact)
                {
                    SwitchCameraMode("Ghost", cameraState, other.gameObject);
                }
                break;
            case PlayerType.Human:
                if (player.InteractionComponent.Interact)
                {
                    SwitchCameraMode("Player", cameraState, other.gameObject);
                }
                break;
            case PlayerType.Both:
                if (player.InteractionComponent.Interact)
                {
                    SwitchCameraMode("Ghost", "Player", cameraState, other.gameObject);
                }
                break;
        }
    }
    private void SwitchCameraMode(string tag, GameState currentState, GameObject other) {

        if (other.CompareTag(tag)) {

            currentState = (currentState == GameState.NormalState)
            ? GameState.TransitionToPuzzle
            : (currentState == GameState.PuzzleState
            ? GameState.TransitionToNormal
            : currentState);

            GameManager.Instance.CurrentGameState = currentState;
        }
    }
    private void SwitchCameraMode(string tag1, string tag2, GameState currentState, GameObject other) {

        if (other.CompareTag(tag1) || other.CompareTag(tag2)) {

            currentState = (currentState == GameState.NormalState)
            ? GameState.TransitionToPuzzle
            : (currentState == GameState.PuzzleState
            ? GameState.TransitionToNormal
            : currentState);

            GameManager.Instance.CurrentGameState = currentState;
        }
    }
}

public enum PlayerType
{
    Ghost,
    Human,
    Both
}

public enum CameraMode
{
    Normal,
    Puzzle
}