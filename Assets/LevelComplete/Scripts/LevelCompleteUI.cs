using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompleteUI : MonoBehaviour
{
    [SerializeField] Button LoseButton;
    [SerializeField] Button WinButton;
    [SerializeField] GameObject WinScreen;
    [SerializeField] GameObject LossScreen;

    // Start is called before the first frame update
    void Awake()
    {
        GameState.onWinLevel += DisplayWinScreen;
        GameState.onLoseLevel += DisplayLossScreen;
        LoseButton.onClick.AddListener(delegate { GameState.Instance.LoseLevel(); });
        WinButton.onClick.AddListener(delegate { GameState.Instance.WinLevel(); });
    }

    void DisplayWinScreen()
    {
        WinScreen.SetActive(true);
        LossScreen.SetActive(false);
        Debug.Log($"{GameState.Instance.currentLevel.Name} cleared!");
        Debug.Log(GameState.Instance.currentLevel.ToString());
    }
    void DisplayLossScreen()
    {
        LossScreen.SetActive(true);
        WinScreen.SetActive(false);
        Debug.Log($"{GameState.Instance.currentLevel.Name} lost! :(");
    }
}
