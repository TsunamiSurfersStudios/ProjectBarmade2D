using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class NPCDialogue : MonoBehaviour
{
    [SerializeField] DialogueData dialogueData;
    [SerializeField] GameObject dialogueCanvas;
    [SerializeField] GameObject choicesPanel;
    [SerializeField] TextMeshProUGUI NPCNameDisplay;
    [SerializeField] TextMeshProUGUI NPCTextDisplay;
   
    private Button[] choiceButtons;
    private int currentNodeIndex = 0; // Current node in the dialogue tree

    private void Start()
    {
       choiceButtons = choicesPanel.GetComponentsInChildren<Button>();
    }
    public void StartConversation()
    {
        currentNodeIndex = 0;
        dialogueCanvas.SetActive(true);
        ShowCurrentNode();
    }

    void ShowCurrentNode()
    {
        if (currentNodeIndex < dialogueData.getDialogueNodes().Length)
        {
            DialogueNode node = dialogueData.getDialogueNodes()[currentNodeIndex];
            ShowDialogue(node.GetText());
            ShowChoices(node.getPlayerChoices());
        }
        else
        {
            HideDialogue();
        }
    }

    void ShowDialogue(string NPCText)
    {
        NPCNameDisplay.text = dialogueData.GetName();
        NPCTextDisplay.text = NPCText;
    }

    void HideDialogue() // TODO: We should not be handling this here
    {
        dialogueCanvas.SetActive(false);  // Hides the whole dialogue panel
        choicesPanel.SetActive(false);   // Hides the choices if they’re visible
    }
    void ShowChoices(PlayerNode[] choices) // TODO: We should not be handling this here
    {
        choicesPanel.SetActive(true);

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i < choices.Length)
            {
                choiceButtons[i].gameObject.SetActive(true);
                choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = choices[i].GetChoiceText();
                choiceButtons[i].onClick.RemoveAllListeners();
                
                int index = i;
                choiceButtons[i].onClick.AddListener(() => OnPlayerChoice(index));
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }

    void OnPlayerChoice(int index)
    {
        choicesPanel.SetActive(false);
        DialogueNode currentNode = dialogueData.getDialogueNodes()[currentNodeIndex];

        if (currentNode.getPlayerChoices() != null && index < currentNode.getPlayerChoices().Length)
        {
            PlayerNode nextNode = currentNode.getPlayerChoices()[index];
            int branchPath = nextNode.GetBranchPath();
            if (branchPath >= 0 && branchPath < dialogueData.getDialogueNodes().Length)
            {
                currentNodeIndex = branchPath;
                ShowCurrentNode();
            }
            else
            {
                HideDialogue();
            }
        }
        else
        {
            HideDialogue();
        }
    }
}
