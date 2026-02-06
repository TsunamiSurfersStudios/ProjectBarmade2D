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

    private NPCOrdering orderingSystem;
    private int currentNodeIndex = 0; // Current node in the dialogue tree

    // TODO: Temporary random name assignment. Will be rewritten later.
    private static readonly string[] randomNames = { //Will be deleted
        "John Weak", "Mario Kart", "Ras Putin", "Joseph Stalin",
        "John Doe", "Josh Sawyer", "Jane Doe", "Leon Kennedy", "Quentin Tarantino"
    };
    private string npcName;//Will be deleted

    private void Start()
    {
        orderingSystem = gameObject.GetComponent<NPCOrdering>();
        npcName = randomNames[Random.Range(0, randomNames.Length)];//Will be deleted
    }
    public void StartConversation()
    {
        currentNodeIndex = 0;
        dialogueCanvas.SetActive(true);
        ShowCurrentNode(); 
    }

    void ShowCurrentNode()
    {
        if (currentNodeIndex <= dialogueData.GetNodeAmount())
        {
            DialogueNode node = dialogueData.GetNode(currentNodeIndex);
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
        //NPCNameDisplay.text = dialogueData.GetName(); //TODO: Refactor this to use the NPC's name from dialog data. This is just a patch to get the system working for playtesting
        NPCNameDisplay.text = npcName;//Will be deleted
        NPCTextDisplay.text = NPCText;
    }

    void HideDialogue()
    {
        dialogueCanvas.SetActive(false);  // Hides the whole dialogue panel
        choicesPanel.SetActive(false);   // Hides the choices if they�re visible
        //TODO: Need to make player's movement be properly enabled again after they are done talking
    }
    void ShowChoices(PlayerNode[] choices)
    {
        Button[] choiceButtons = choicesPanel.GetComponentsInChildren<Button>();
        choicesPanel.SetActive(true);

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i < choices.Length)
            {
                Button currButton = choiceButtons[i];
                PlayerNode currChoice = choices[i];

                if (currChoice.BeginsOrder() && orderingSystem.OrderActive())
                {
                    currButton.gameObject.SetActive(false);
                }
                else
                {
                    ColorBlock colors = currButton.colors;
                    colors.normalColor = currChoice.BeginsOrder() ? Color.yellow : Color.white;
                    currButton.colors = colors;

                    currButton.gameObject.SetActive(true);
                    currButton.GetComponentInChildren<TextMeshProUGUI>().text = currChoice.GetChoiceText();
                    currButton.onClick.RemoveAllListeners();


                    int index = i;
                    choiceButtons[i].onClick.AddListener(() => OnPlayerChoice(index));
                }
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }

    void OnPlayerChoice(int nextIndex)
    {
        choicesPanel.SetActive(false);
        DialogueNode currentNode = dialogueData.GetNode(currentNodeIndex) ;

        if (currentNode.ChoicesNotNull() && nextIndex < currentNode.GetChoiceLength())
        {
            
            PlayerNode playerChoice = currentNode.GetChoice(nextIndex);
                if (playerChoice.BeginsOrder())
                {
                    CreateOrder();
                }
                else
                {
                    int branchPath = playerChoice.GetBranchPath();
                    if (branchPath >= 0 && branchPath <= dialogueData.GetNodeAmount())
                    {
                        currentNodeIndex = branchPath;
                        ShowCurrentNode();
                    }
                    else
                    {
                        HideDialogue();
                    }
                }
        }
        else
        {
            HideDialogue();
        }
    }

    void CreateOrder()
    {
        orderingSystem.CreateOrder(npcName);
        Recipe recipe = orderingSystem.GetOrder();
        OrderNode orderNode = new OrderNode(recipe.GetDrinkName());
        dialogueData.AddNode(orderNode);
        currentNodeIndex = dialogueData.GetNodeAmount();
        ShowCurrentNode();
    }
}