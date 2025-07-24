using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerNode
{
    [SerializeField] string choiceText;
    [SerializeField] int branchPath;

    public string GetChoiceText() { return choiceText; }
    public int GetBranchPath() { return branchPath; }
}

[System.Serializable]
public class DialogueNode
{
    [TextArea] // TODO: It should be clear in script what this is doing
    [SerializeField] string NPCText;
    [SerializeField] PlayerNode[] playerChoices;

    [SerializeField] bool isOrder;
    [SerializeField] bool requiresRecipe;

    public string GetText() { return NPCText; }
    public PlayerNode[] getPlayerChoices() { return playerChoices; }

}
//Allow this ScriptableObject as an asset in the editor
[CreateAssetMenu(fileName = "NewDialogueData", menuName = "Dialogue/DialogueData")]  // TODO: We should steal this format for drink creation

[System.Serializable]
public class DialogueData : ScriptableObject
{
    [SerializeField] string NPCName;
    [SerializeField] DialogueNode[] dialogueNodes;

    public string GetName() { return NPCName; }
    public DialogueNode[] getDialogueNodes() {  return dialogueNodes; }
}
