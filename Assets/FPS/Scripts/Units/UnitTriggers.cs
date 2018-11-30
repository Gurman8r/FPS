using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ML
{
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
        public class TriggerEvent : UnityEvent<UnitEventData> { }

        [Serializable]
        public class Entry
        {
            public UnitTriggerType  eventID;
            public TriggerEvent     callback;
        }


        /* Variables
        * * * * * * * * * * * * * * * */
        public List<Entry> delegates;


        /* Properties
        * * * * * * * * * * * * * * * */


        /* Functions
        * * * * * * * * * * * * * * * */
        private void Execute(UnitTriggerType id, UnitEventData eventData)
        {
            if (delegates != null)
            {
                for (int i = 0, imax = delegates.Count; i < imax; ++i)
                {
                    Entry ent = delegates[i];

                    if (ent.eventID == id && ent.callback != null)
                    {
                        ent.callback.Invoke(eventData);
                    }
                }
            }
        }

        public virtual void OnSpawn(UnitEventData eventData)
        {
            Execute(UnitTriggerType.OnSpawn, eventData);
        }

        public virtual void OnDeath(UnitEventData eventData)
        {
            Execute(UnitTriggerType.OnDeath, eventData);
        }

        public virtual void OnRecieveDamage(UnitEventData eventData)
        {
            Execute(UnitTriggerType.OnRecieveDamage, eventData);
        }

        public virtual void OnRecieveHealing(UnitEventData eventData)
        {
            Execute(UnitTriggerType.OnRecieveHealing, eventData);
        }

        public virtual void OnDoDamage(UnitEventData eventData)
        {
            Execute(UnitTriggerType.OnDoDamage, eventData);
        }

        public virtual void OnDoHealing(UnitEventData eventData)
        {
            Execute(UnitTriggerType.OnDoHealing, eventData);
        }
    }

}
