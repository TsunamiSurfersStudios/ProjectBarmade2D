using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.HDROutputUtils;

public class StartGame : MonoBehaviour
{
    [SerializeField] Service[] Services;
    [SerializeField] GameObject LoadingScreenUI;



    void Start()
    {
        Load();
    }

    void Load()
    {
        LoadingScreenUI = GameObject.Instantiate(LoadingScreenUI);
        LoadingScreen loadingScreen = LoadingScreenUI.GetComponent<LoadingScreen>();
        loadingScreen.Init();

        /*int processCount = Services.Length * 2;
        int progress = 0;
        foreach (Service service in Services)
        {
            service.OnStart();
            loadingScreen.SetLoadingBar(progress++ / processCount);
        }
        foreach (Service service in Services)
        {
            service.OnLoad();
            loadingScreen.SetLoadingBar(progress++ / processCount);
        }*/

        IEnumerator update = loadingScreen.SetLoadingBar(1);
        StartCoroutine(update);
    }

}
