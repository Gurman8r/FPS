using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Basically per-unit event system
// Based on UnityEngine.EventSystems.EventTrigger
// https://github.com/Pinkuburu/Unity-Technologies-ui/blob/master/UnityEngine.UI/EventSystem/EventTrigger.cs

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
        public class TriggerEvent : UnityEvent<UnitEventData> { }

        [Serializable]
        public class Entry
        {
            public TriggerType  eventID;
            public TriggerEvent callback;
        }


        /* Variables
        * * * * * * * * * * * * * * * */
        public List<Entry> delegates;


        /* Functions
        * * * * * * * * * * * * * * * */
        private void Execute(TriggerType id, UnitEventData ev)
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
        public virtual void OnSpawn(SpawnEvent ev)
        {
            Execute(TriggerType.OnSpawn, ev);
            CombatExecutes.OnSpawn(ev);
        }

        public virtual void OnDeath(DeathEventData ev)
        {
            Execute(TriggerType.OnDeath, ev);
            CombatExecutes.OnDeath(ev);
        }

        public virtual void OnReceiveDamage(DamageEventData ev)
        {
            Execute(TriggerType.OnReceiveDamage, ev);
            CombatExecutes.OnReceiveDamage(ev);
        }

        public virtual void OnDoDamage(DamageEventData ev)
        {
            Execute(TriggerType.OnDoDamage, ev);
            CombatExecutes.OnDoDamage(ev);
        }

        public virtual void OnReceiveHealing(HealingEventData ev)
        {
            Execute(TriggerType.OnReceiveHealing, ev);
            CombatExecutes.OnReceiveHealing(ev);
        }

        public virtual void OnDoHealing(HealingEventData ev)
        {
            Execute(TriggerType.OnDoHealing, ev);
            CombatExecutes.OnDoHealing(ev);
        }
    }

}
