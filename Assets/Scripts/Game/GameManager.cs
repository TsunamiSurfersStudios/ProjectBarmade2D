using UnityEngine;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        public int currentLevel { get; private set; }
        private static GameManager _instance;
        public static GameManager Instance { get { return _instance; } }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this);
            }
            else
            {
                _instance = this;
            }
            DontDestroyOnLoad(this);
        }

        public void Start()
        {
            currentLevel = 0; // start at 0
            GameEventManager.Instance.Subscribe(GameEventManager.GameEvent.LevelComplete, LevelUp);
        }

        public void LevelUp()
        {
            currentLevel += 1;
            Debug.Log($"current level: {currentLevel}");
        }
    }
}
