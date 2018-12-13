using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public interface ISpawnHandler
    {
        void OnSpawn(SpawnEvent ev);
    }

    public interface IDeathHandler
    {
        void OnDeath(DeathEventData ev);
    }

    public interface IDamageTarget
    {
        void OnReceiveDamage(DamageEventData ev);
    }

    public interface IDamageSource
    {
        void OnDoDamage(DamageEventData ev);
    }

    public interface IHealingTarget
    {
        void OnReceiveHealing(HealingEventData ev);
    }

    public interface IHealingSource
    {
        void OnDoHealing(HealingEventData ev);
    }
}
