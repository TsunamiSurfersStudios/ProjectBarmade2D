using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.Serialization;

namespace NPC
{
    public class NPCSpawner : MonoBehaviour
    {
        private int executeTime; // TODO: Unclear variable name

        [SerializeField] int minSpawnWait;
        [SerializeField] int maxSpawnWait;
        [FormerlySerializedAs("NPCObject")] [SerializeField] GameObject npcObject;
        [SerializeField] GameObject spawnPoint;

        private bool isSpawning = false;
        List<NPCController> controllerList = new List<NPCController>();

        private void Start()
        {
            GameEventManager.Instance.Subscribe(GameEventManager.Command.SpawnNPC, SpawnOnCommand);
            GameEventManager.Instance.Subscribe(GameEventManager.Command.StartDay, SpawnOnCommand);
        }
        public void SetWaitTimes(int minWait, int maxWait)
        {
            minSpawnWait = minWait;
            maxSpawnWait = maxWait;
        }

        public void StartSpawning()
        {
            executeTime = Mathf.RoundToInt(Time.time) + Random.Range(minSpawnWait, maxSpawnWait);
            isSpawning = true;
        }

        public void StopSpawning()
        {
            isSpawning = false; 
        }

        public void EndDay()
        {
            foreach (NPCController controller in controllerList)
            {
                controller.Leave();
            }
            controllerList.Clear();
        }

        void Update()
        {
            if (!isSpawning)
            {
                return;
            }

            if (Mathf.RoundToInt(Time.time) == executeTime && Spawn())
            {
                executeTime = executeTime + Random.Range(minSpawnWait, maxSpawnWait);
            }
        }

        private void SpawnOnCommand()
        {
            Spawn();
        }
        private bool Spawn()
        {
            GameObject seat = GetAvaliableSeat();
            if (seat)
            {
                Vector2 spawnPosition = spawnPoint.transform.position; // Get the spawn position from the spawn point
                GameObject NPC = Instantiate(npcObject, spawnPosition, Quaternion.identity);
                NPCController controller = NPC.GetComponent<NPCController>();
                controllerList.Add(controller);

                controller.SetSeat(seat); // Give NPC seat property
                NPC.SetActive(true); // Show NPC
                seat.GetComponent<NPCObjects>().SetOccupied(true); // Set seat as occupied
                GameEventManager.Instance.TriggerEvent(GameEventManager.GameEvent.NPCSpawned, NPC);

                return true;
            }
            return false;
        }

        private GameObject GetAvaliableSeat()
        {
            foreach (GameObject seat in GameObject.FindGameObjectsWithTag("Seat")) // TODO: This could be a function bool isSeatAvaliable()
            {
                // Check if the seat is occupied
                if (!seat.GetComponent<NPCObjects>().GetOccupied())
                {
                    return seat;
                }
            }
            return null;
        }
    }
}