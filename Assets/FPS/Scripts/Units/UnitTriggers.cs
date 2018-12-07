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
        private void Execute(EventType id, UnitEvent ev)
        {
            if (delegates != null)
            {
                for (int i = 0, imax = delegates.Count; i < imax; ++i)
                {
                    Entry ent = delegates[i];

                    if (ent.eventID == id && ent.callback != null)
                    {
                        ent.callback.Invoke(ev);
                    }
                }
            }
        }

        public void Broadcast(EventType id, UnitEvent ev)
        {
            gameObject.BroadcastMessage(id.ToString(), ev);
        }


        /* Functions
        * * * * * * * * * * * * * * * */
        public virtual void OnSpawn(UnitEvent ev)
        {
            Execute(EventType.OnSpawn, ev);
        }

        public virtual void OnDeath(UnitEvent ev)
        {
            Execute(EventType.OnDeath, ev);
        }

        public virtual void OnReceiveDamage(UnitEvent ev)
        {
            Execute(EventType.OnReceiveDamage, ev);
        }

        public virtual void OnReceiveHealing(UnitEvent ev)
        {
            Execute(EventType.OnReceiveHealing, ev);
        }

        public virtual void OnDoDamage(UnitEvent ev)
        {
            Execute(EventType.OnDoDamage, ev);
        }

        public virtual void OnDoHealing(UnitEvent ev)
        {
            Execute(EventType.OnDoHealing, ev);
        }
    }

}
