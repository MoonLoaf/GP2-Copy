using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Quest")]public class Quest : ScriptableObject {
    public QuestState CurrentQuestState = QuestState.Inactive;
    [TextArea] public string Hint;
    public Quest LinkedQuest;
    public List<Quest> QuestPrerequisites;
    public float HintTime;

    public void UpdateQuestState() {
        if (QuestPrerequisites != null && QuestPrerequisites.Count > 0) {
            if (QuestPrerequisites.TrueForAll(quest => quest.CurrentQuestState == QuestState.Completed)) {
                CurrentQuestState = QuestState.Active;
            }
        }
    }
    
    public void CompleteQuest() { // needs to be called from completion object.
        if (CurrentQuestState == QuestState.Active) {
            CurrentQuestState = QuestState.Waiting;
        }
        
        if (LinkedQuest != null) {
            if (LinkedQuest.CurrentQuestState == QuestState.Waiting) {
                LinkedQuest.CurrentQuestState = QuestState.Completed;
                CurrentQuestState = QuestState.Completed;
            }
        }
        else CurrentQuestState = QuestState.Completed;

        GameManager.Instance.UpdateQuests();
    }
}

public enum QuestState {
    Inactive, Active, Completed, NotCompleted, Waiting
}