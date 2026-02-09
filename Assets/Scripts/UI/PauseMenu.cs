using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
   
    private bool isPaused;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {

            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
   
        }
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);

        TimeController.Instance.StartTime();
        isPaused = false;
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);

        TimeController.Instance.StopTime();
        isPaused = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        //SceneManager.LoadScene("MainMenu"); 
    }

}
