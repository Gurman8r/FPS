using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public static class TriggerExecutes
    {
        public static void OnSpawn(UnitEventData ev)
        {
            if (ev.source)
            {
                ev.source.health.SetDead(false);
                ev.source.health.SetCurrent(ev.source.health.maximum);
            }
        }

        public static void OnDeath(UnitEventData ev)
        {
            if (ev.source && !ev.source.health.isDead)
            {
                ev.source.health.SetDead(true);
            }
        }


        public static void OnDoDamage(DamageEventData ev)
        {
            if (ev.source) { }
        }

        public static void OnReceiveDamage(DamageEventData ev)
        {
            if (ev.target)
            {
                ev.target.health.ApplyDamage(ev.damage);

                if (ev.source)
                {
                    ev.source.triggers.OnDoDamage(ev);
                }

                if (!ev.target.health.isDead && ev.target.health.CheckDead())
                {
                    ev.target.triggers.OnDeath(new DeathEventData(ev.target));
                }
            }
        }


        public static void OnDoHealing(HealingEventData ev)
        {
            if (ev.target)
            {
                ev.target.triggers.OnReceiveHealing(ev);
            }
        }

        public static void OnReceiveHealing(HealingEventData ev)
        {
            if (ev.target)
            {
                ev.target.health.ApplyHealing(ev.healing);
            }
        }

    }

}
