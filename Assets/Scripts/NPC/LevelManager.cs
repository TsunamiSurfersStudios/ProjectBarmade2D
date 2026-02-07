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
    [SerializeField] public bool spawnOnStart { get; private set; } = true ;
    public List<SpawnRate> GetSpawnRates()
    {
        return spawnRates;
    }

    public void SetDefaultWaitTimes(NPCSpawner spawner)
    {
        spawner.SetWaitTimes(defaultMinWait, defaultMaxWait);
    }
}

    public class LevelManager : MonoBehaviour
{
    [Header("General")]
    [SerializeField][Range(0,23)] int startHour;
    [SerializeField][Range(1, 25)] int hoursOpen;
    [SerializeField] bool startOnAwake;
    [Tooltip("Number of hours before closing to stop spawning NPCs")]
    [SerializeField] int bufferToStopSpawning = 1;
    [Header("Individual Level Controls")]
    [SerializeField] List<SpawnSchedule> spawnSchedules;

    [SerializeField] NPCSpawner npcSpawner;

    public int currLevel { get; private set; } = 0;
    private int hoursPassed = 0;

    private static LevelManager _instance;
    public static LevelManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<LevelManager>();
            return _instance;
        }
    }
    private void Start()
    {
        GameEventManager.Instance.Subscribe(GameEventManager.Command.StartDay, StartLevel);
        if (startOnAwake)
            StartLevel();
    }
    void UpdateForHour(int hour)
    {
        hoursPassed++;
        if ( hoursPassed == hoursOpen - bufferToStopSpawning)
        {
            // Stop spawning
            npcSpawner.StopSpawning();
        }
        if (hoursPassed >= hoursOpen) // Check Day Over
        {
            EndLevel();
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
        if (currLevel < spawnSchedules.Count)
        {
            SpawnSchedule currSchedule = spawnSchedules[currLevel];
            if (currSchedule.spawnOnStart)
                npcSpawner.StartSpawning();
        }
            
    }

    private void EndLevel()
    {
        TimeController.Instance.OnHourChanged -= UpdateForHour;
        TimeController.Instance.StopTime();
        npcSpawner.EndDay();
        currLevel++;
        GameEventManager.Instance.TriggerEvent(GameEventManager.GameEvent.LevelComplete);
    }
}

#if UNITY_EDITOR
// Custom property drawer for SpawnSchedule list
[CustomPropertyDrawer(typeof(SpawnSchedule))]
public class SpawnScheduleDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Get the index from the property path
        string path = property.propertyPath;
        int index = -1;

        // Extract index from path like "spawnSchedules.Array.data[0]"
        if (path.Contains("[") && path.Contains("]"))
        {
            int startIndex = path.IndexOf("[") + 1;
            int length = path.IndexOf("]") - startIndex;
            if (int.TryParse(path.Substring(startIndex, length), out index))
            {
                label.text = "Level " + index;
            }
        }

        // Draw the property with custom label
        EditorGUI.PropertyField(position, property, label, true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}
#endif