using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
