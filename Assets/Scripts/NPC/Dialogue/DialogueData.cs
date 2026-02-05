using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerNode
{
    [SerializeField] string choiceText;
    [SerializeField] int branchPath;
    [SerializeField] bool beginsOrder = false;

    public string GetChoiceText() { return choiceText; }
    public int GetBranchPath() { return branchPath; }
    public bool BeginsOrder() { return beginsOrder; }
    public PlayerNode(string text, int path)
    {
        choiceText = text;
        branchPath = path;
    }
    public PlayerNode() { }
}

[System.Serializable]
public class DialogueNode
{
    [TextArea]
    [SerializeField] protected string NPCText;
    [SerializeField] protected PlayerNode[] playerChoices;

    public string GetText() { return NPCText; }
    public PlayerNode[] getPlayerChoices() { return playerChoices; }
    public int GetChoiceLength() { return playerChoices.Length; }
    public PlayerNode GetChoice(int index) { return playerChoices[index]; }
    public bool ChoicesNotNull() { return playerChoices != null; }

}

public class OrderNode : DialogueNode
{  
    public OrderNode(string name)
    {
        NPCText = "I'd like a " + name + ", please.";
        playerChoices = new PlayerNode[] { new PlayerNode("Coming right .", -1)};
    }
}


[CreateAssetMenu(fileName = "NewDialogueData", menuName = "Dialogue/DialogueData")]  
[System.Serializable]
public class DialogueData : ScriptableObject
{
    [SerializeField] string NPCName;
    [SerializeField] List<DialogueNode> dialogueNodes;

    bool hasOrdered;

    public string GetName() { return NPCName; }
    public void AddNode(DialogueNode node) { dialogueNodes.Add(node); }
    
    public int GetNodeAmount() { return dialogueNodes.Count - 1; }
    public DialogueNode GetNode(int index) { return dialogueNodes[index]; }
}
