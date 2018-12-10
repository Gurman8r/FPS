using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public interface ISpawnHandler
    {
        void OnSpawn(UnitEvent ev);
    }

    public interface IDeathHandler
    {
        void OnDeath(UnitEvent ev);
    }

    public interface IDamageTarget
    {
        void OnReceiveDamage(DamageEvent ev);
    }

    public interface IDamageSource
    {
        void OnDoDamage(DamageEvent ev);
    }

    public interface IHealingTarget
    {
        void OnReceiveHealing(HealingEvent ev);
    }

    public interface IHealingSource
    {
        void OnDoHealing(HealingEvent ev);
    }
}
