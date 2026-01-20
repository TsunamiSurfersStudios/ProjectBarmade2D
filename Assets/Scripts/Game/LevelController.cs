using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class SpawnRate
{
    [SerializeField] int startHour;
    [SerializeField] int endHour;
    [SerializeField] int minSpawnWait = 30;
    [SerializeField] int maxSpawnWait = 60;

    public void SetWaitTimes(NPCSpawner spawner)
    {
        spawner.SetWaitTimes(minSpawnWait, maxSpawnWait);
    }
    public int StartHour { get { return startHour; } }
    public int EndHour { get { return endHour; } }
}

[System.Serializable]
public class SpawnSchedule
{
    [SerializeField] int defaultMinWait = 10;
    [SerializeField] int defaultMaxWait = 20;
    [SerializeField] List<SpawnRate> spawnRates;

    public List<SpawnRate> GetSpawnRates()
    {
        return spawnRates;
    }

    public void SetDefaultWaitTimes(NPCSpawner spawner)
    {
        spawner.SetWaitTimes(defaultMinWait, defaultMaxWait);
    }
}

    public class LevelController : MonoBehaviour
{
    [Header("General")]
    [SerializeField][Range(0,23)] int startHour;
    [SerializeField][Range(1, 24)] int hoursOpen;
    [Header("Individual Level Controls")]
    [SerializeField] List<SpawnSchedule> spawnSchedules;

    [SerializeField] NPCSpawner npcSpawner;

    private int currLevel = 0;
    private int hoursPassed = 0;

    private void Start()
    {
        StartLevel();
    }
    void UpdateForHour(int hour)
    {
        hoursPassed++;
        if (hoursPassed >= hoursOpen) // Check Day Over
        {
            TimeController.Instance.OnHourChanged -= UpdateForHour;
            TimeController.Instance.StopTime();
            currLevel++;
            npcSpawner.StopSpawning();
            // Level up here
            return;
        }
        UpdateSpawnWaitTimes();
    }

    private void UpdateSpawnWaitTimes()
    {
        if (currLevel < spawnSchedules.Count) // Update NPC Spawning
        {
            foreach (SpawnRate rate in spawnSchedules[currLevel].GetSpawnRates())
            {
                if (TimeController.Instance.IsTimeBetween(rate.StartHour, rate.EndHour))
                {
                    rate.SetWaitTimes(npcSpawner);
                    return;
                }
                spawnSchedules[currLevel].SetDefaultWaitTimes(npcSpawner);
            }
        }
    }

    public void StartLevel()
    {
        TimeController.Instance.StartTime(startHour);
        TimeController.Instance.OnHourChanged += UpdateForHour;
        UpdateSpawnWaitTimes();
        npcSpawner.StartSpawning();
    }

    public int GetCurrentLevel()
    {
        return currLevel;
    }


}