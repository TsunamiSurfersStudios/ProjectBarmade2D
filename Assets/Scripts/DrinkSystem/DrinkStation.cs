using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkStation : MonoBehaviour
{
    [SerializeField] private int limesVolume;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public int GetLimesVolume()
    {
        return limesVolume;
    }

    public void SetLimesVolume(int volume)
    {
        limesVolume = volume;
    }
}
