using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [ExecuteInEditMode]
    public abstract class CombatObject : MonoBehaviour
    {
        public static readonly string Tag = "Combat";

        /* Variables
        * * * * * * * * * * * * * * * */
        [Header("Combat Object Settings")]
        [SerializeField] CombatData m_data;

        [Header("Combat Object Runtime")]
        [SerializeField] float      m_timer;
        [SerializeField] List<Unit> m_hitList;


        /* Properties
        * * * * * * * * * * * * * * * */
        public CombatData data
        {
            get { return m_data; }
            set { m_data = value; }
        }

        public float timer
        {
            get { return m_timer; }
            protected set { m_timer = value; }
        }

        public List<Unit> hitList
        {
            get { return m_hitList; }
        }


        /* Core
        * * * * * * * * * * * * * * * */
        protected virtual void Start()
        {
            if(Application.isPlaying)
            {
                if (gameObject.tag != Tag) gameObject.tag = Tag;
            }
        }

        protected virtual void Update()
        {
            if(Application.isPlaying)
            {
                if (data.lifeSpan > 0f)
                {
                    if (timer >= data.lifeSpan)
                    {
                        Kill();

                        return;
                    }
                    else
                    {
                        timer += Time.deltaTime;
                    }
                }
            }
        }

        /* Functions
        * * * * * * * * * * * * * * * */
        protected bool AddHit(Unit other)
        {
            if (other)
            {
                if(!hitList.Contains(other))
                {
                    hitList.Add(other);

                    return true;
                }
            }
            return false;
        }

        protected bool CheckHit(RaycastHit hit, out Unit other)
        {
            if(hit.transform.tag == Unit.Tag)
            {
                if ((other = hit.transform.GetComponent<Unit>()))
                {
                    if (other != data.owner)
                    {
                        return true;
                    }
                }
            }
            other = null;
            return false;
        }

        protected bool CheckHit(Collider hit, out Unit other)
        {
            if(hit)
            {
                if (hit.gameObject.tag == Unit.Tag)
                {
                    if ((other = hit.GetComponent<Unit>()))
                    {
                        return (other != data.owner);
                    }
                }
            }
            other = null;
            return false;
        }

        protected virtual void OnHit(Unit other)
        {
            other.triggers.Broadcast(EventType.OnRecieveDamage, new UnitEvent
            {
                combat = data
            });

            if (data.owner)
            {
                data.owner.triggers.Broadcast(EventType.OnDoDamage, new UnitEvent
                {
                    combat = data
                });
            }
        }


        public virtual void Kill()
        {
            if (Application.isPlaying)
            {
                Destroy(gameObject);
            }
            else
            {
                DestroyImmediate(gameObject);
            }
        }

        public virtual void Spawn()
        {
            timer = 0f;
            gameObject.SetActive(true);
        }


        public void SpawnObject(CombatObject value)
        {
            CombatObject obj;
            if(value && (obj = Instantiate(value, null)))
            {
                obj.transform.position = transform.position;
                obj.transform.rotation = transform.rotation;
                obj.data.owner = data.owner;
                obj.Spawn();
            }
        }
    }

}
