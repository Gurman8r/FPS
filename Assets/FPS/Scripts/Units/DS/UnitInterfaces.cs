using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    /* Interfaces
    * * * * * * * * * * * * * * * */
    public interface ISpawnHandler
    {
        void OnSpawn(UnitEvent ev);
        void OnDeath(UnitEvent ev);
    }

    public interface IDamageTarget
    {
        void OnRecieveDamage(UnitEvent ev);
    }

    public interface IHealingTarget
    {
        void OnRecieveHealing(UnitEvent ev);
    }

    public interface IDamageSource
    {
        void OnDoDamage(UnitEvent ev);
    }

    public interface IHealingSource
    {
        void OnDoHealing(UnitEvent ev);
    }
}
