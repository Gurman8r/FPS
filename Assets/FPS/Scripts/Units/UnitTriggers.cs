using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Basically per-unit event system
// Based on UnityEngine.EventSystems.EventTrigger
// https://github.com/tenpn/unity3d-ui/blob/master/UnityEngine.UI/EventSystem/EventTrigger.cs

namespace FPS
{
    [DisallowMultipleComponent]
    public class UnitTriggers : UnitBehaviour
        , ISpawnHandler
        , IDamageSource
        , IHealingSource
        , IDamageTarget
        , IHealingTarget
    {

        /* DS
        * * * * * * * * * * * * * * * */
        [Serializable]
        public class TriggerEvent : UnityEvent<UnitEvent> { }

        [Serializable]
        public class Entry
        {
            public EventType    eventID;
            public TriggerEvent callback;
        }


        /* Variables
        * * * * * * * * * * * * * * * */
        public List<Entry> delegates;


        /* Core
        * * * * * * * * * * * * * * * */
        private void Execute(EventType id, UnitEvent unitEvent)
        {
            if (delegates != null)
            {
                for (int i = 0, imax = delegates.Count; i < imax; ++i)
                {
                    Entry ent = delegates[i];

                    if (ent.eventID == id && ent.callback != null)
                    {
                        ent.callback.Invoke(unitEvent);
                    }
                }
            }
        }

        public void Broadcast(EventType id, UnitEvent unitEvent)
        {
            BroadcastMessage(id.ToString(), unitEvent);
        }


        /* Functions
        * * * * * * * * * * * * * * * */
        public virtual void OnSpawn(UnitEvent unitEvent)
        {
            Execute(EventType.OnSpawn, unitEvent);
        }

        public virtual void OnDeath(UnitEvent unitEvent)
        {
            Execute(EventType.OnDeath, unitEvent);
        }

        public virtual void OnRecieveDamage(UnitEvent unitEvent)
        {
            Execute(EventType.OnRecieveDamage, unitEvent);
        }

        public virtual void OnRecieveHealing(UnitEvent unitEvent)
        {
            Execute(EventType.OnRecieveHealing, unitEvent);
        }

        public virtual void OnDoDamage(UnitEvent unitEvent)
        {
            Execute(EventType.OnDoDamage, unitEvent);
        }

        public virtual void OnDoHealing(UnitEvent unitEvent)
        {
            Execute(EventType.OnDoHealing, unitEvent);
        }
    }

}
