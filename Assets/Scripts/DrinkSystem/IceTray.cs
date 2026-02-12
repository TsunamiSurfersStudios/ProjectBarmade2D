using Game;
using Player;
using UnityEngine;

namespace DrinkSystem
{
    public class IceTray : MonoBehaviour
    {
        float trayVolume = 0f; 
        float trayCapacity = 100f;
        [SerializeField] private Animator animator;


        void Update()
        {
            if (!animator)
            {
                return;
            }
            ShowAnimatorState();
        }

        void ShowAnimatorState()
        {
            if (trayVolume <= 100f && trayVolume > 50f)
            {
                animator.SetBool("isHalfEmpty", false); // TODO: This can definetley be optimized. Also this should be in a function not in the update function
                animator.SetBool("isFull", true);
            }
            else if (trayVolume <= 50f && trayVolume > 0f)
            {
                animator.SetBool("isFull", false);
                animator.SetBool("isHalfEmpty", true);
            }
            else
            {
                animator.SetBool("isHalfEmpty", false);
                animator.SetBool("isFull", false);
            }
        }
        public float GetVolume()
        { 
            return trayVolume; 
        }

        public void CheckForIce()
        {
            ItemHolder holder = ItemHolder.Instance;
            GameObject ice = holder.TakeObject();
            if (ice != null && ice.CompareTag("IceBucket"))
            {
                Destroy(ice);
                RefillTray();
            }
            else
            {
                holder.GiveObject(ice);
            }
           
        }
        public void RefillTray(float amount = 100) 
        {
            trayVolume += amount;
            if (trayVolume > trayCapacity)
            {
                trayVolume = trayCapacity;
            }
            Debug.Log("Ice Tray Refilled");
            GameEventManager.Instance.TriggerEvent(GameEventManager.GameEvent.IceTrayRefilled);
        }

        public void EmptyTray(float amount = 100)
        {
            trayVolume -= amount;
            if (trayVolume < 0)
            {
                trayVolume = 0;
            }
        }
    }
}
