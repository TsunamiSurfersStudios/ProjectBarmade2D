using Game;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] GameObject pauseMenu;
        PlayerInteractions interactions;

        [SerializeField] KeyCode KEYBIND = KeyCode.Escape;

        private bool isPaused;

        // Start is called before the first frame update
        void Start()
        {
            pauseMenu = GameObject.Find("PauseMenu");
            pauseMenu.SetActive(false);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPaused) PauseGame();
                else ResumeGame();
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
            if (Application.isPlaying)
                Application.Quit();
            else Debug.Log("Quitting Game");
        }

        public void MainMenu()
        {
            SceneManager.LoadScene("MainMenu"); 
        }
    }
}
