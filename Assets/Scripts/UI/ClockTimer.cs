using Game;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ClockTimer : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI timerText;
        [SerializeField] TextMeshProUGUI dayOfWeekText;

        private void Start()
        {
            TimeController.Instance.OnDayChanged += UpdateDayOfWeek;
            UpdateDayOfWeek(TimeController.Instance.DayOfWeek());
        }

        void Update()
        {
            if (TimeController.Instance != null && timerText != null)
            {
                timerText.text = TimeController.Instance.GetTimeString();
            }
        }

        void UpdateDayOfWeek(TimeController.Day dayOfWeek)
        {
            if (TimeController.Instance != null && dayOfWeekText != null)
            {
                dayOfWeekText.text = dayOfWeek.ToString();
            }
        }
    }
}