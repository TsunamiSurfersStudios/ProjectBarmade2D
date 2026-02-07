using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void LoadGame()
    {
        SceneManager.LoadScene("PlayTests"); // TODO: make this an actual loadgame logic 
    }
    public void NewGame()
    {
        SceneManager.LoadScene("PlayTests"); // for now loads the scene PlayTests
    }

    public void Settings()
    {
        Debug.Log("Implement settings here");
        SceneManager.LoadScene("Settings");
    }
}
