using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public static class UnitExecutes
    {
        public static void OnSpawn(UnitEvent ev)
        {
            if (ev.source)
            {
                UnitManager.instance?.Register(ev.source);
                ev.source.health.SetDead(false);
                ev.source.health.SetCurrent(ev.source.health.maximum);
            }
        }

        public static void OnDeath(UnitEvent ev)
        {
            if (ev.source && !ev.source.health.isDead)
            {
                UnitManager.instance?.Unregister(ev.source);
                ev.source.health.SetDead(true);
            }
        }


        public static void OnDoDamage(DamageEvent ev)
        {
            if (ev.source) { }
        }

        public static void OnReceiveDamage(DamageEvent ev)
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
                    ev.target.triggers.OnDeath(ev);
                }
            }
        }


        public static void OnDoHealing(HealingEvent ev)
        {
            if (ev.target)
            {
                ev.target.triggers.OnReceiveHealing(ev);
            }
        }

        public static void OnReceiveHealing(HealingEvent ev)
        {
            if (ev.target)
            {
                ev.target.health.ApplyHealing(ev.healing);
            }
        }
    }

}
