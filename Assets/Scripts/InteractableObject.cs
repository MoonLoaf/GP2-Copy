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
                    player.UpdateCameraTransform(_cameraTransform);
                    // if (cameraState == GameState.TransitionToPuzzle) {
                    // }

                    _interact = false;
                }

                break;
            case PlayerType.Human:
                if (other.CompareTag("Player")) {
                    player.UpdateCameraTransform();
                    // if (cameraState == GameState.TransitionToPuzzle) {
                    //     //StartCoroutine(TransitionToPuzzle(_camera pos, ResetCameraTransformte));
                    // }

                    _interact = false;
                }

                break;
            case PlayerType.Both:
                if (other.CompareTag("Player") || other.CompareTag("Ghost")) {
                    player.UpdateCameraTransform();
                    // if (cameraState == GameState.TransitionToPuzzle) {
                    //     //StartCoroutine(TransitionToPuzzle(_camera pos, cameraState));
                    // }
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
                if (other.CompareTag("Ghost")) {
                    player.UpdateCameraTransform(_cameraTransform);
                    if(player.InteractionComponent.Interact && GameManager.Instance.CurrentGameState == GameState.NormalState){
                        ToInteraction();
                    }
                    else if (player.InteractionComponent.Interact && GameManager.Instance.CurrentGameState == GameState.PuzzleState) {
                        FromInteraction();
                    }
                }

                break;
            case PlayerType.Human:
                if (other.CompareTag("Player")) {
                    player.UpdateCameraTransform();
                    if(player.InteractionComponent.Interact && GameManager.Instance.CurrentGameState == GameState.NormalState){
                        ToInteraction();
                    }
                    else if (player.InteractionComponent.Interact && GameManager.Instance.CurrentGameState == GameState.PuzzleState) {
                        FromInteraction();
                    }
                }

                break;
            case PlayerType.Both:
                if (other.CompareTag("Player") || other.CompareTag("Ghost")) {
                    player.UpdateCameraTransform();
                    if(player.InteractionComponent.Interact && GameManager.Instance.CurrentGameState == GameState.NormalState) {
                        GameManager.Instance.CurrentGameState = GameState.TransitionToPuzzle;
                    }
                    else if (player.InteractionComponent.Interact && GameManager.Instance.CurrentGameState == GameState.PuzzleState) {
                        GameManager.Instance.CurrentGameState = GameState.TransitionToNormal;
                    }
                }
                break;
        }
    }

    private void Update() {
        if (!_interact) {
            return;
        }

        // if (Input.GetKeyDown(KeyCode.F)) {
        //     // Input manager add buttons
        //     if (_cameraMode == CameraMode.Puzzle && GameManager.Instance.CurrentGameState == GameState.NormalState) {
        //         GameManager.Instance.CurrentGameState = GameState.TransitionToPuzzle;
        //     }
        //
        //     if (_quest.CurrentQuestState == QuestState.Completed) {
        //         if (events == null || events.Count < 1) return; // ska ske engång när du klarat av questet. 
        //         foreach (var gameEvent in events) {
        //             gameEvent?.Invoke();
        //         }
        //     }
        // }
        // if (Input.GetKeyDown(KeyCode.Escape)) {
        //     if (GameManager.Instance.CurrentGameState == GameState.PuzzleState) {
        //         GameManager.Instance.CurrentGameState = GameState.TransitionToNormal;
        //     }
        // }
    }

    private void ToInteraction() {
        if (_cameraMode == CameraMode.Puzzle && GameManager.Instance.CurrentGameState == GameState.NormalState) {
            GameManager.Instance.CurrentGameState = GameState.TransitionToPuzzle;
        }

        if (_quest.CurrentQuestState == QuestState.Completed) {
            if (events == null || events.Count < 1) return; // ska ske engång när du klarat av questet. 
            foreach (var gameEvent in events) {
                gameEvent?.Invoke();
            }
        }
    }

    private void FromInteraction() {
        if (GameManager.Instance.CurrentGameState == GameState.PuzzleState) {
            GameManager.Instance.CurrentGameState = GameState.TransitionToNormal;
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