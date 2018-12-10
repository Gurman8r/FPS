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
        , ISpawnHandler, IDeathHandler
        , IDamageSource, IHealingSource
        , IDamageTarget, IHealingTarget
    {

        /* DS
        * * * * * * * * * * * * * * * */
        [Serializable]
        public class TriggerEvent : UnityEvent<UnitEvent> { }

        [Serializable]
        public class Entry
        {
            public CombatEventType    eventID;
            public TriggerEvent callback;
        }


        /* Variables
        * * * * * * * * * * * * * * * */
        public List<Entry> delegates;


        /* Functions
        * * * * * * * * * * * * * * * */
        private void Execute(CombatEventType id, UnitEvent ev)
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


        /* Triggers
        * * * * * * * * * * * * * * * */
        public virtual void OnSpawn(UnitEvent ev)
        {
            Execute(CombatEventType.OnSpawn, ev);
            UnitExecutes.OnSpawn(ev);
        }

        public virtual void OnDeath(UnitEvent ev)
        {
            Execute(CombatEventType.OnDeath, ev);
            UnitExecutes.OnDeath(ev);
        }

        public virtual void OnReceiveDamage(DamageEvent ev)
        {
            Execute(CombatEventType.OnReceiveDamage, ev);
            UnitExecutes.OnReceiveDamage(ev);
        }

        public virtual void OnDoDamage(DamageEvent ev)
        {
            Execute(CombatEventType.OnDoDamage, ev);
            UnitExecutes.OnDoDamage(ev);
        }

        public virtual void OnReceiveHealing(HealingEvent ev)
        {
            Execute(CombatEventType.OnReceiveHealing, ev);
            UnitExecutes.OnReceiveHealing(ev);
        }

        public virtual void OnDoHealing(HealingEvent ev)
        {
            Execute(CombatEventType.OnDoHealing, ev);
            UnitExecutes.OnDoHealing(ev);
        }
    }

}
