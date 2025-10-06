using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] GameObject Screen;
    [SerializeField] GameObject LoadingBar;

    public void Init()
    {
        Screen.SetActive(true);
        LoadingBar.transform.localScale = new Vector3(0, 1, 1);
    }

    public IEnumerator SetLoadingBar(float percent)
    {
        float xStart = LoadingBar.transform.localScale.x;
        float increment = (percent - xStart) / 10f;
        while(LoadingBar.transform.localScale.x < percent)
        {
            float value = LoadingBar.transform.localScale.x;
            LoadingBar.transform.localScale = new Vector3(value + increment, 1, 1);
            yield return new WaitForSeconds(0.001f);
        }
        if (LoadingBar.transform.localScale.x >= 1) 
        { 
            LoadingBar.transform.localScale = Vector3.one;
            yield return new WaitForSeconds(1);
            HideScreen();
        }
    }

    void HideScreen()
    { 
        Screen.SetActive(false);
    }
}
