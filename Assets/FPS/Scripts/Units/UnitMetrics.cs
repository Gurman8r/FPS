using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [DisallowMultipleComponent]
    public class UnitMetrics : UnitBehaviour
        , ISpawnHandler
        , IDamageSource
        , IHealingSource
        , IDamageTarget
        , IHealingTarget
    {
        /* Variables
        * * * * * * * * * * * * * * * */
        public int      timesSpawned;
        public int      timesKilled;
        public float    damageDealt;
        public float    damageRecieved;
        public float    healingDone;
        public float    healingRecieved;

        /* Functions
        * * * * * * * * * * * * * * * */
        public void OnSpawn(UnitEvent ev)
        {
            timesSpawned++;
        }

        public void OnDeath(UnitEvent ev)
        {
            timesKilled++;
        }

        public void OnDoDamage(UnitEvent ev)
        {
            damageDealt += ev.data.damage.amount;
        }

        public void OnDoHealing(UnitEvent ev)
        {
            healingDone += ev.data.healing.amount;
        }

        public void OnRecieveDamage(UnitEvent ev)
        {
            damageRecieved += ev.data.damage.amount;
        }

        public void OnRecieveHealing(UnitEvent ev)
        {
            healingRecieved += ev.data.healing.amount;
        }
    }
}
