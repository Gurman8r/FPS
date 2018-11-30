using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ML
{
    /* Trigger Type
    *  Note: There should be an interface with 
    *  a function for each enum
    * * * * * * * * * * * * * * * */
    public enum UnitTriggerType
    {
        OnSpawn, OnDeath,
        OnRecieveDamage, OnRecieveHealing,
        OnDoDamage, OnDoHealing,
    }

    /* Interfaces
    * * * * * * * * * * * * * * * */
    public interface ISpawnHandler
    {
        void OnSpawn(UnitEventData eventData);
        void OnDeath(UnitEventData eventData);
    }

    public interface IDamageTarget
    {
        void OnRecieveDamage(UnitEventData eventData);
    }

    public interface IHealingTarget
    {
        void OnRecieveHealing(UnitEventData eventData);
    }

    public interface IDamageSource
    {
        void OnDoDamage(UnitEventData eventData);
    }

    public interface IHealingSource
    {
        void OnDoHealing(UnitEventData eventData);
    }
}
