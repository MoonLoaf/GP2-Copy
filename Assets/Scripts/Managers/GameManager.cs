using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

public class GameManager : GenericSingleton<GameManager>
{
    // hints and objectives.. 
    public List<Quest> Quests;
    [SerializeField] private GameState _currentGameState = GameState.NormalState;

    public GameState CurrentGameState {
        get => _currentGameState;
        set {
            if (value != _currentGameState) {
                _currentGameState = value;
                foreach (var listener in _gameStateListeners) {
                    listener.Initialize(value);
                }
            } else {
                _currentGameState = value;
            }
        }
    }
    private List<IGameStateListener> _gameStateListeners = new List<IGameStateListener>();

    private void Awake() {
        Quests = Resources.LoadAll<Quest>("QuestSO").ToList();
        //Quests.ForEach(EditorUtility.SetDirty);
    }

    public void UpdateQuests() {
        // this should be called on quest Complete
        foreach (var quest in Quests) {
            quest.UpdateQuestState();
        }
    }
    public void GetHint() {
    }

    public void AddGameStateListener(IGameStateListener gameStateListener) {
        _gameStateListeners.Add(gameStateListener);
    }
}

public interface IGameStateListener
{
    public void Initialize(GameState currentState);
}

public enum GameState
{
    NormalState,
    PuzzleState,
    TransitionToPuzzle,
    TransitionToNormal
}