using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    /* Interfaces
    * * * * * * * * * * * * * * * */
    public interface ISpawnHandler
    {
        void OnSpawn(UnitEvent unitEvent);
        void OnDeath(UnitEvent unitEvent);
    }

    public interface IDamageTarget
    {
        void OnRecieveDamage(UnitEvent unitEvent);
    }

    public interface IHealingTarget
    {
        void OnRecieveHealing(UnitEvent unitEvent);
    }

    public interface IDamageSource
    {
        void OnDoDamage(UnitEvent unitEvent);
    }

    public interface IHealingSource
    {
        void OnDoHealing(UnitEvent unitEvent);
    }
}
