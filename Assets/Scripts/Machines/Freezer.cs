using Game;
using Player;
using UnityEngine;

namespace Machines
{
    public class Freezer : MonoBehaviour
    {
        [SerializeField] GameObject bucket;
        public void GetIce()
        {
            ItemHolder holder = ItemHolder.Instance;

            if (holder && holder.IsEmpty())
            {
                holder.Spawn(bucket);
                GameEventManager.Instance.TriggerEvent(GameEventManager.GameEvent.IceMachineInteracted);
            }
        }
    }
}
